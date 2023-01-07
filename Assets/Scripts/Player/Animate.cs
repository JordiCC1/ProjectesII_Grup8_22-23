using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Animate : MonoBehaviour
    {
        [SerializeField] private Movement player;
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer sprite;

        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Fall = Animator.StringToHash("Fall");
        private static readonly int Land = Animator.StringToHash("Land");
        private static readonly int Hang = Animator.StringToHash("Hang");

        private int currentState;
        private float landTime;

        private void Start()
        {
            player = GetComponent<Movement>();
            animator = GetComponent<Animator>();
            sprite = GetComponent<SpriteRenderer>();

            animator.CrossFade(Idle, 0, 0);
        }

        private void FixedUpdate()
        {
            var state = currentState;

            state = GetState();

            if (state == currentState) return;
            animator.CrossFade(state, 0, 0);
            currentState = state;
        }

        private int GetState()
        {
            if (Time.time < landTime) return currentState;

            // Priorities
            if (player.landingThisFrame) return LockState(Land, 0.1f);
            if (player.isHanging)
            {
                //TODO: lock flip for a while
                sprite.flipX = true;
                return Hang;
            }
            else
            {
                sprite.flipX = false;
            }
            if (player.shouldJump || player.shouldWallJump) return Jump;

            if (player.colDown) return player.movementScale == 0 ? Idle : Walk;
            return player.rb.velocity.y > 0.5f ? Jump : Fall;

            int LockState(int s, float t)
            {
                landTime = Time.time + t;
                return s;
            }
        }
    }
}