using UnityEngine;

namespace Util
{
    public class Yeeter : MonoBehaviour
    {
        public float speed = 0.1f;
        
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

            _incrementor += speed * Time.deltaTime;

            transform.position = Vector3.Lerp(_start, _target, _incrementor);
        }
    }
}