using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    [RequireComponent(typeof(Image))]
    public class Flash : MonoBehaviour
    {
        private Image _image;
        private float _targetAlpha;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void StartFlash()
        {
            StartCoroutine(RunFlash());
        }

        private IEnumerator RunFlash()
        {
            _targetAlpha = 1f;

            yield return new WaitForSeconds(.1f);

            _targetAlpha = 0f;
        }

        private void Update()
        {
            _image.color = new Color(
                _image.color.r,
                _image.color.g,
                _image.color.b,
                Mathf.Lerp(_image.color.a, _targetAlpha, 24f * Time.deltaTime)
            );
        }
    }
}