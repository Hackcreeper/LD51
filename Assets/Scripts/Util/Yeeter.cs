using UnityEngine;

namespace Util
{
    public class Yeeter : MonoBehaviour
    {
        public float speed = 0.004f;
        
        private bool _started;
        private Vector3 _target;
        private Vector3 _start;
        private float _incrementor;

        public void StartTheYeet(Vector3 target)
        {
            _started = true;
            _start = transform.position;
            _target = target;
        }

        private void Update()
        {
            if (!_started)
            {
                return;
            }

            _incrementor += speed;

            transform.position = Vector3.Lerp(_start, _target, _incrementor);
        }
    }
}