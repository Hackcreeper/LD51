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
        
        private Player.Player _player;
        private NavMeshAgent _agent;

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
            
            switch (_progress)
            {
                case TaskProgress.TakeIngredient:
                    HandleTakeIngredient();
                    break;
            }
        }

        private void HandleTakeIngredient()
        {
            if (_agent.remainingDistance > 0.1f)
            {
                return;
            }
            
            _currentTarget.Interact(_player);

            switch (_currentTask.station)
            {
                case StationType.Cutting:
                    MoveToStationOfType(StationType.Cutting);
                    break;
            }
            
            _progress = TaskProgress.WalkToStation;
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

            _agent.destination = foundStation.transform.position;
        }
    }
}