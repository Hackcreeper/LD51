using System.Collections;
using UnityEngine;

namespace Feeding
{
    [RequireComponent(typeof(Animator))]
    public class Monster : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int Crunching = Animator.StringToHash("crunching");
        private static readonly int Sucking = Animator.StringToHash("sucking");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void Crunch()
        {
            StartCoroutine(StartCrunch());
        }

        public void StartSuck()
        {
            _animator.SetBool(Sucking, true);
        }
        
        private IEnumerator StartCrunch()
        {
            _animator.SetBool(Crunching, true);
            yield return new WaitForSeconds(.5f);
            _animator.SetBool(Crunching, false);
        }

        public void StopSuck()
        {
            _animator.SetBool(Sucking, false);
        }
    }
}