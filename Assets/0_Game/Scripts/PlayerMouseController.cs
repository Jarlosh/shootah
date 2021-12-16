using UnityEngine;
using UnityEngine.UIElements;

namespace Scripts
{
    public class PlayerMouseController : MonoBehaviour
    {
        [SerializeField] private PlayerAnimator animator;
        private Camera camera;
        [SerializeField] private bool isShooting = false;
        private Vector3 shootTarget;

        public bool IsShooting
        {
            get => isShooting;
            set
            {
                if (value == isShooting)
                    return;
                isShooting = value;
                animator.SetFireState(isShooting);
            }
        }

        private void Start()
        {
            camera = Camera.main;
        }
        
        private void Update()
        {
            CheckInput();
            UpdateWeapon();
        }

        private void UpdateWeapon()
        {
            if(!IsShooting)
                return;
        }

        private void CheckInput()
        {
            if (Input.GetMouseButton(0))
            {
                var ray = camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit, 100))
                {
                    IsShooting = true;
                    shootTarget = hit.point;
                }
                else IsShooting = false;
            }
            else IsShooting = false;
        }
    }
}