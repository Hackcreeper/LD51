using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        public float speed = 10f;
        public Transform childModel;
        public float smoothRotationFactor = 10f;
    
        private CharacterController _characterController;
        private Vector2 _velocity;
        private Vector3 _targetRotation;
        private bool _frozen;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        protected virtual void Update()
        {
            if (_frozen)
            {
                return;
            }
            
            var vec3 = new Vector3(_velocity.x, 0f, _velocity.y);
        
            _characterController.SimpleMove(vec3 * speed);

            if (vec3.magnitude > 0.1f)
            {
                _targetRotation = vec3;
            }
        
            var originalRotation = childModel.rotation;
            childModel.LookAt(transform.position + _targetRotation);
            var newRotation = childModel.rotation;
            childModel.rotation = Quaternion.Lerp(originalRotation, newRotation, smoothRotationFactor * Time.deltaTime);
            
            childModel.rotation = Quaternion.Euler(
                0,
                childModel.rotation.eulerAngles.y,
                0
            );
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _velocity = context.ReadValue<Vector2>();
        }

        public void Freeze()
        {
            _frozen = true;
        }

        public void UnFreeze()
        {
            _frozen = false;
        }

        public void OnControlsChanged(PlayerInput i)
        {
            // TODO: Update tooltip
        }
    }
}