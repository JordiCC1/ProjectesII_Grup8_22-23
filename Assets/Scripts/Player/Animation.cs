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

        }

        void WalkAnim()
        {

        }

        void JumpAnim()
        {

        }

        void WallJumpAnim()
        {

        }


    }
}