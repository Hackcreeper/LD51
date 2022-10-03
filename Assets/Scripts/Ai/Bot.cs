using System;
using Ai.Data;
using Ai.Enum;
using Cooking;
using Cooking.Enum;
using UnityEngine;
using UnityEngine.AI;

namespace Ai
{
    [RequireComponent(typeof(Player.Player), typeof(NavMeshAgent))]
    public class Bot : MonoBehaviour
    {
        public LayerMask interactableLayerMask;
        public LayerMask entityLayerMask;

        private Player.Player _player;
        private NavMeshAgent _agent;
        private Vector3 _startPosition;

        private RecipeTask _currentTask;
        private TaskManager _taskManager;
        private TaskProgress _progress;
        private Interactable _currentTarget;

        private void Awake()
        {
            _player = GetComponent<Player.Player>();

            _agent = GetComponent<NavMeshAgent>();
            _agent.updateRotation = false;
        }

        private void Start()
        {
            _startPosition = transform.position;
        }

        public void StartTask(RecipeTask task, TaskManager taskManager)
        {
            Debug.Log($"Doing task: {task.ingredient.label} ({task.station.ToString()})");

            // First we need to get the ingredient
            _currentTask = task;
            _taskManager = taskManager;
            _progress = TaskProgress.TakeIngredient;

            // Search to the correct ingredient chest
            var results = new Collider[50];
            var found = Physics.OverlapSphereNonAlloc(transform.position, 100f, results, interactableLayerMask);

            IngredientChest correctChest = null;
            for (var i = 0; i < found; i++)
            {
                var chest = results[i].GetComponent<IngredientChest>();
                if (!chest)
                {
                    continue;
                }

                if (chest.ingredient != task.ingredient)
                {
                    continue;
                }

                correctChest = chest;
                break;
            }

            if (!correctChest)
            {
                Debug.LogError("No ingredient chest found for: " + task.ingredient.label);
                return;
            }

            Debug.Log($"Found chest: {correctChest.name}");
            _currentTarget = correctChest;
            _agent.SetDestination(correctChest.standingPosition.position);
        }

        private void Update()
        {
            if (_currentTask == null || _progress == TaskProgress.Idle)
            {
                return;
            }

            CheckRaycasts();

            switch (_progress)
            {
                case TaskProgress.TakeIngredient:
                    HandleTakeIngredient();
                    break;

                case TaskProgress.WalkToStation:
                    HandleWalkToStation();
                    break;

                case TaskProgress.WalkToSecondStation:
                    HandleWalkToStation();
                    break;

                case TaskProgress.PutOnPlate:
                    HandlePutOnPlate();
                    break;

                case TaskProgress.Done:
                case TaskProgress.WaitForProcessing:
                case TaskProgress.WaitForSecondProcessing:
                    break;

                default:
                    Debug.LogWarning("Unhandled: " + _progress);
                    break;
            }
        }

        private void CheckRaycasts()
        {
            RaycastHit hit;
            var blockedFront = Physics.Raycast(transform.position, transform.forward, out hit, 2f, entityLayerMask);
            if (blockedFront)
            {
                _agent.Move((transform.position - hit.transform.position) * Time.deltaTime * _agent.speed);
                return;
            }
            
            var blockedLeft = Physics.Raycast(transform.position, -transform.right, out hit, 2f, entityLayerMask);
            if (blockedLeft)
            {
                _agent.Move((transform.position - hit.transform.position) * Time.deltaTime * _agent.speed);
                return;
            }
            
            var blockedRight = Physics.Raycast(transform.position, transform.right, out hit, 2f, entityLayerMask);
            if (blockedRight)
            {
                _agent.Move((transform.position - hit.transform.position) * Time.deltaTime * _agent.speed);
                return;
            }
        }

        private void HandleTakeIngredient()
        {
            if (_agent.remainingDistance > 0.1f)
            {
                return;
            }

            // TODO: Maybe rotate to station??
            // ((BotMovement)_player.GetPlayerMovement()).SetAgentRotation(_currentTarget.transform.position - transform.position);
            _currentTarget.Interact(_player);

            switch (_currentTask.station)
            {
                case StationType.Cutting:
                    MoveToStationOfType(StationType.Cutting);
                    break;

                case StationType.None:
                    // TODO: Move back to plate
                    break;

                case StationType.Oven:
                    MoveToStationOfType(StationType.Oven);
                    break;

                case StationType.Stove:
                    MoveToStationOfType(StationType.Stove);
                    break;

                case StationType.CuttingAndStove:
                    MoveToStationOfType(StationType.Cutting);
                    break;
            }

            _progress = TaskProgress.WalkToStation;
        }

        private void HandleWalkToStation()
        {
            if (_agent.remainingDistance > 0.1f)
            {
                return;
            }

            _currentTarget.Interact(_player);
            _progress = _progress == TaskProgress.WalkToStation
                ? TaskProgress.WaitForProcessing
                : TaskProgress.WaitForSecondProcessing;
        }

        private void HandlePutOnPlate()
        {
            if (_agent.remainingDistance > 0.2f)
            {
                return;
            }

            _currentTarget.Interact(_player);
            _progress = TaskProgress.Done;

            _agent.destination = _startPosition;
        }

        private void MoveToStationOfType(StationType type)
        {
            var results = new Collider[50];
            var found = Physics.OverlapSphereNonAlloc(transform.position, 100f, results, interactableLayerMask);

            var distance = float.MaxValue;
            CookingStation foundStation = null;

            for (var i = 0; i < found; i++)
            {
                var station = results[i].GetComponent<CookingStation>();
                if (!station)
                {
                    continue;
                }

                if (station.type != type)
                {
                    continue;
                }

                var dis = Vector3.Distance(transform.position, station.transform.position);
                if (dis < distance)
                {
                    distance = dis;
                    foundStation = station;
                }
            }

            if (!foundStation)
            {
                Debug.LogError("No station found for type: " + type);
                return;
            }

            _currentTarget = foundStation;
            _agent.destination = foundStation.standingTarget.position;
        }

        private void StationFinished()
        {
            var station = ((CookingStation)_currentTarget);
            if (!station.freezePlayer)
            {
                station.Interact(_player);
            }

            // If station was chopping / cooking -> move to next station
            // otherwise move to plate
            if (_currentTask.station == StationType.CuttingAndStove &&
                _progress != TaskProgress.WaitForSecondProcessing)
            {
                MoveToStationOfType(StationType.Stove);
                _progress = TaskProgress.WalkToSecondStation;
                return;
            }

            var plate = _taskManager.GetDesignatedPlate();
            _currentTarget = plate;

            _agent.SetDestination(plate.transform.position);
            _progress = TaskProgress.PutOnPlate;
        }
    }
}