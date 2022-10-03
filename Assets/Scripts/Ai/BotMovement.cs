using Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Ai
{
    public class BotMovement : PlayerMovement
    {
        private Vector3 _targetRotation;
        
        protected override void Update()
        {
            var agent = GetComponent<NavMeshAgent>();
            
            var vec3 = agent.velocity.normalized;
        
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
            
            // childModel.rotation = Quaternion.Euler(
            //     0,
            //     Mathf.Lerp(childModel.rotation.eulerAngles.y, _targetRotationY, smoothRotationFactor * Time.deltaTime),
            //     0
            // );
        }
    }
}