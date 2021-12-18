using System;
using UnityEngine;

namespace Scripts
{
    public class WeaponPivot : MonoBehaviour
    {
        [SerializeField] private Transform armedPivot;
        [SerializeField] private Transform disarmedPivot;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float accuracy = 0.01f;
        
        private Transform currentPivot;

        public void SetArmed(bool state)
        {
            currentPivot = state ? armedPivot : disarmedPivot;
        }

        private void Start()
        {
            SetArmed(false);
        }

        private void Update()
        {
            // UpdatePosition();
            // UpdateRotation();
        }

        private void UpdatePosition()
        {
            var pos = transform.position;
            var target = currentPivot.position;
            if ((pos - target).magnitude < accuracy)
                return;
            transform.position = Vector3.MoveTowards(pos, target, moveSpeed * Time.deltaTime);
        }
        
        private void UpdateRotation()
        {
            var rot = transform.rotation;
            var target = currentPivot.rotation;
            if (Quaternion.Angle(rot, target) < accuracy)
                return;
            transform.rotation = Quaternion.RotateTowards(rot, target, rotationSpeed * Time.deltaTime);
        }
    }
}