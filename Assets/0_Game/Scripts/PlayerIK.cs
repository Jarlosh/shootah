using System;
using UnityEngine;

namespace Scripts
{
    public class PlayerIK : MonoBehaviour
    {
        [SerializeField] private bool enabled;
        [SerializeField] private Animator anim;
        [SerializeField] private float distToGround = 1f;
        [SerializeField] private LayerMask walkableLayer;
        
        [SerializeField] private FootInfo leftFoot = new FootInfo(AvatarIKGoal.LeftFoot);
        [SerializeField] private FootInfo rightFoot = new FootInfo(AvatarIKGoal.RightFoot);

        private void OnAnimatorIK(int layerIndex)
        {
            if (!anim || !enabled) return;
            UpdateFootIK(leftFoot);
            UpdateFootIK(rightFoot);
        }

        private void UpdateFootIK(FootInfo footInfo)
        {
            UpdateWeight(footInfo);
            CalculateFootPosition(footInfo);
        }

        private void UpdateWeight(FootInfo footInfo)
        {
            var weight = anim.GetFloat(footInfo.ParamNameHash);
            var goal = footInfo.ikGoal;
            anim.SetIKPositionWeight(goal, weight);
            anim.SetIKRotationWeight(goal, weight);
        }

        private void CalculateFootPosition(FootInfo footInfo)
        {
            var goal = footInfo.ikGoal;
            var ray = new Ray(anim.GetIKPosition(goal) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out var hit, distToGround + 2f, walkableLayer))
            {
                footInfo.Position = hit.point + hit.normal * distToGround;
                var dir = Vector3.Cross(transform.right, hit.normal);
                footInfo.Rotation = Quaternion.LookRotation(dir);
                anim.SetIKPosition(goal, footInfo.Position);
                anim.SetIKRotation(goal, footInfo.Rotation);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            leftFoot.DrawGizmos();
            rightFoot.DrawGizmos();
        }
        
        [Serializable]
        class FootInfo
        {
            [SerializeField] string footParamName;
            private int? _paramNameHash;
            public int ParamNameHash
            {
                get { return _paramNameHash ??= Animator.StringToHash(footParamName); }
            }

            public readonly AvatarIKGoal ikGoal;
            public Vector3 Position { get; set; }
            public Quaternion Rotation { get; set; }
            
            public FootInfo(AvatarIKGoal ikGoal)
            {
                this.ikGoal = ikGoal;
            }
            
            public void DrawGizmos()
            {
                Gizmos.DrawSphere(Position, 0.025f);
                Gizmos.DrawRay(Position, Rotation * Vector3.up);
            }
        }
    }
}