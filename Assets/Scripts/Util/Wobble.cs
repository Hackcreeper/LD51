using UnityEngine;

namespace Util
{
    public class Wobble : MonoBehaviour
    {
        public float size = 1f;
        public Vector3 basePosition;

        public void SetBasePosition(Vector3 position)
        {
            basePosition = position;
        }

        private void Update()
        {
            transform.position = basePosition;
            transform.Translate(new Vector3(
                0,
                Mathf.Sin(Time.time) * size,
                0
            ), Space.Self);
        }
    }
}