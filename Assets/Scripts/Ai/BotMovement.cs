using Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ai
{
    public class BotMovement : PlayerMovement
    {
        public float idleRotation = -120;
        public float stationRotation = 72;
        
        private float _targetRotationY;

        private void Start()
        {
            _targetRotationY = childModel.rotation.eulerAngles.y;
        }

        protected override void Update()
        {
            childModel.rotation = Quaternion.Euler(
                0,
                Mathf.Lerp(childModel.rotation.eulerAngles.y, _targetRotationY, smoothRotationFactor * Time.deltaTime),
                0
            );
        }

        public void RotateToStation()
        {
            _targetRotationY = stationRotation;
        }
        
        public void RotateToIdle()
        {
            _targetRotationY = idleRotation;
        }
    }
}