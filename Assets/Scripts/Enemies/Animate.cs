using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class Animate : MonoBehaviour
    {
        [SerializeField] private Controller enemy;
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer sprite;

        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Active = Animator.StringToHash("Active");
        private static readonly int Reload = Animator.StringToHash("Reload");

        private int currentState;
        //private float pauseTime;

        private void Start()
        {
            enemy = GetComponent<Controller>();
            animator = GetComponent<Animator>();
            sprite = GetComponent<SpriteRenderer>();

            animator.CrossFade(Idle, 0, 0);
        }

        void Update()
        {
            //sprite
            sprite.flipX = enemy.target.transform.position.x > transform.position.x ? true : false;

            //frame
            var state = currentState;
            state = GetState();

            if (state == currentState) return;
            animator.CrossFade(state, 0, 0);
            currentState = state;
        }

        private int GetState()
        {
            //if (Time.time < pauseTime) return currentState;

            // Priorities
            if (enemy.isReloaded) return Reload;

            return enemy.isDetected ? Active : Idle;

            //int LockState(int s, float t)
            //{
            //    pauseTime = Time.time + t;
            //    return s;
            //}
        }
    }
}