using UnityEngine;

namespace Scripts
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private static readonly int XParamHash = Animator.StringToHash("X");
        private static readonly int YParamHash = Animator.StringToHash("Y");
        private static readonly int ShootingParamHash = Animator.StringToHash("IsShooting");
        [SerializeField] private int armLayerIndex = 1;
        
        public void SetMoveValue(float x, float y)
        {
            animator.SetFloat(XParamHash, x);
            animator.SetFloat(YParamHash, y);
        }

        public void SetFireState(bool state)
        {
            animator.SetBool(ShootingParamHash, state);
            // animator.SetLayerWeight(armLayerIndex, state ? 1 : 0);
        }
    }
}