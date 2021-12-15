using System;
using UnityEngine;

namespace Scripts
{
    public class PlayerIK : MonoBehaviour
    {
        [SerializeField] private Animator anim;
        [SerializeField] private float distToGround = 1f;
        [SerializeField] private LayerMask walkableLayer;

        private Vector3 leftFootPos;
        private Quaternion leftrot;
        private Vector3 rightFootPos;
        [SerializeField] private bool enabled;
        private void OnAnimatorIK(int layerIndex)
        {
            if (!anim || !enabled) return;

            UpdateFootIK(AvatarIKGoal.LeftFoot, "LeftFootIK", ref leftFootPos);
            UpdateFootIK(AvatarIKGoal.RightFoot, "RightFootIK", ref rightFootPos);
        }

        private void UpdateFootIK(AvatarIKGoal goal, string key, ref Vector3 pos)
        {
            UpdateWeights(goal, key);
            CalcFootIK(goal, ref pos);
        }

        private void UpdateWeights(AvatarIKGoal goal, string key)
        {
            anim.SetIKPositionWeight(goal, anim.GetFloat(key));
            anim.SetIKRotationWeight(goal, anim.GetFloat(key));
        }

        private void CalcFootIK(AvatarIKGoal goal, ref Vector3 pos)
        {
            var ray = new Ray(anim.GetIKPosition(goal) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out var hit, distToGround + 2f, walkableLayer))
            {
                var footPosition = pos = hit.point + Vector3.up * distToGround;
                var dir = Vector3.Cross(transform.right, hit.normal);
                var footRotation = leftrot = Quaternion.LookRotation(dir);
                anim.SetIKPosition(goal, footPosition);
                anim.SetIKRotation(goal, footRotation);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(leftFootPos, 0.025f);
            Gizmos.DrawRay(leftFootPos, leftrot * Vector3.up);
            Gizmos.DrawSphere(rightFootPos, 0.025f);
        }
    }
}