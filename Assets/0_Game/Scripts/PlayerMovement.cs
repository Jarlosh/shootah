using System;
using UnityEngine;

namespace Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private PlayerAnimator animator;
        

        private void Update()
        {
            CheckInput();
        }

        private void CheckInput()
        {
            var vertical = Input.GetAxis("Vertical");
            var horizontal = Input.GetAxis("Horizontal");
            animator.SetMoveValue(horizontal, vertical);
        }
    }
}