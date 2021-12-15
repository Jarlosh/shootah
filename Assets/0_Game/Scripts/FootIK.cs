using System;
using UnityEngine;

namespace Scripts
{
    public class FootIK : MonoBehaviour
    {
        [SerializeField] private string key;
        [SerializeField] private AvatarIKGoal goal;
        [SerializeField] private float distToGround = 1f;
        [SerializeField] private LayerMask walkableLayer;
        
        [SerializeField] private float speed;
        [SerializeField] private float degreeSpeed;
        
        private Animator anim;
        
        private Vector3 targetPosition;
        private Vector3 footPosition;
        
        private Quaternion targetRot;
        private Quaternion footRotation;

        const float accuracy = 0.01f;
        private void Start()
        {
            anim = GetComponent<Animator>();
            CalcFootIK();
            footPosition = anim.GetIKPosition(goal);
        }

        private void Update()
        {
            if (!anim || !enabled) return;
            UpdatePosition();
            UpdateRotation();
        }

        private void UpdateRotation()
        {
            // if (Quaternion.Angle(targetRot, footRotation) < accuracy)
            //     return;
            footRotation = Quaternion.RotateTowards(footRotation, targetRot, degreeSpeed * Time.deltaTime);
            anim.SetIKRotation(goal, footRotation);
        }

        private void UpdatePosition()
        {
            if ((footPosition - targetPosition).magnitude < accuracy)
                return;
            footPosition = Vector3.MoveTowards(footPosition, targetPosition, speed * Time.deltaTime);
            anim.SetIKPosition(goal, footPosition);
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (!anim || !enabled) return;

            UpdateWeights();
            CalcFootIK();
            anim.SetIKPosition(goal, footPosition);
            anim.SetIKRotation(goal, footRotation);
        } 
        
        private void UpdateWeights()
        {
            anim.SetIKPositionWeight(goal, anim.GetFloat(key));
            anim.SetIKRotationWeight(goal, anim.GetFloat(key));
        }

        private void CalcFootIK()
        {
            var ray = new Ray(anim.GetIKPosition(goal) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out var hit, distToGround + 2f, walkableLayer))
            {
                targetPosition = hit.point + hit.normal * distToGround;
                var dir = Vector3.Cross(transform.right, hit.normal);
                targetRot = Quaternion.LookRotation(dir);
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(footPosition, footRotation * Vector3.up);
            Gizmos.DrawSphere(footPosition, 0.025f);
        }
    }
}