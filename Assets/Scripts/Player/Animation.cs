using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    public class Animation : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        void IdleAnim()
        {
            animator.SetBool("MidAir", false);
            animator.SetBool("Running", false);
            animator.SetBool("Hanging", false);
        }

        void WalkAnim()
        {
            IdleAnim();
            animator.SetBool("Running", true);
        }

        void JumpAnim()
        {
            IdleAnim();
            animator.SetBool("MidAir", true);
        }

        void WallJumpAnim()
        {
            IdleAnim();
            animator.SetBool("Hanging", true);
        }
    }
}