using Ui;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerInteraction : MonoBehaviour
    {
        public float searchRadius = 5f;
        public LayerMask interactionLayer;

        private Interactable _currentlySelected;
        private Player _player;

        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        private void Update()
        {
            var closest = FindClosest();
            if (closest == _currentlySelected)
            {
                return;
            }

            _currentlySelected = closest;
            
            if (!closest)
            {
                PopupManager.Instance.Hide();
                return;
            }

            closest.ShowPopup();
        }

        private Interactable FindClosest()
        {
            var results = new Collider[10];
            var colliders = Physics.OverlapSphereNonAlloc(transform.position, searchRadius, results, interactionLayer);

            var closestDistance = float.MaxValue;
            Interactable closest = null;

            for (var i = 0; i < colliders; i++)
            {
                var interactable = results[i].GetComponent<Interactable>();
                if (!interactable)
                {
                    Debug.LogWarning($"GameObject {results[i].name} has interactable layer, but no component.");
                    return closest;
                }

                var distance = Vector3.Distance(transform.position, interactable.transform.position);
                if (distance < closestDistance && interactable.CanInteract())
                {
                    closestDistance = distance;
                    closest = interactable;
                }
            }

            return closest;
        }
        
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!context.performed || !_currentlySelected)
            {
                return;
            }
            
            _currentlySelected.Interact(_player);
        }   
    }
}