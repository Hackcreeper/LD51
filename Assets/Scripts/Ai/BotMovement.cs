using Player;
using UnityEngine;
using UnityEngine.AI;

namespace Ai
{
    public class BotMovement : PlayerMovement
    {
        private Vector3 _agentRotation;
        
        protected override void Update()
        {
            var agent = GetComponent<NavMeshAgent>();
            
            var vec3 = agent.velocity.normalized;
        
            if (vec3.magnitude > 0.1f)
            {
                _agentRotation = vec3;
            }
        
            var originalRotation = childModel.rotation;
            childModel.LookAt(transform.position + _agentRotation);
            var newRotation = childModel.rotation;
            childModel.rotation = Quaternion.Lerp(originalRotation, newRotation, smoothRotationFactor * Time.deltaTime);
            
            childModel.rotation = Quaternion.Euler(
                0,
                childModel.rotation.eulerAngles.y,
                0
            );
        }
    }
}