using System;
using System.Collections;
using UnityEngine;

namespace Feeding
{
    [RequireComponent(typeof(Animator))]
    public class Monster : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int Crunching = Animator.StringToHash("crunching");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void Crunch()
        {
            StartCoroutine(StartCrunch());
        }

        private IEnumerator StartCrunch()
        {
            _animator.SetBool(Crunching, true);
            yield return new WaitForSeconds(.5f);
            _animator.SetBool(Crunching, false);
        }
    }
}