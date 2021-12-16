using UnityEngine;

namespace Scripts
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private static readonly int X = Animator.StringToHash("X");
        private static readonly int Y = Animator.StringToHash("Y");
        
        public void SetMoveValue(float x, float y)
        {
            animator.SetFloat(X, x);
            animator.SetFloat(Y, y);
        }
        
        
    }
}