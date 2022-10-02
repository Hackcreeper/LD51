using UnityEngine;

namespace Cooking
{
    public class CookingStationBehaviour : StateMachineBehaviour
    {
        private static readonly int Working = Animator.StringToHash("working");

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.normalizedTime > .1f)
            {
                return;
            }
            
            animator.SetBool(Working, false);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<CookingStation>()?.OnAnimationFinish();
        }
    }
}