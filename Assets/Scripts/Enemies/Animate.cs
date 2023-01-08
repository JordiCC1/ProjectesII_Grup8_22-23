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

        //Stun Animation
        private float minimum = 0.3f;
        private float maximum = 1.0f;
        private float cyclesPerSecond = 2.0f;
        private float alpha;
        private bool increasing = true;
        private Color srColor;
        private Color maxAlpha;

        private int currentState;
        //private float pauseTime;

        private void Start()
        {
            enemy = GetComponent<Controller>();
            animator = GetComponent<Animator>();
            sprite = GetComponent<SpriteRenderer>();

            animator.CrossFade(Idle, 0, 0);


            maxAlpha = sprite.color;
            srColor = sprite.color;
            alpha = maximum;
        }

        void Update()
        {
            //enemy stunned
            if (enemy.isSwapped)
                Blink();

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
            // Priorities
            if (enemy.isSwapped) return Idle;
            if (enemy.isReloaded) return Reload;

            return enemy.isDetected ? Active : Idle;
        }


        void Blink()
        {
            float t = Time.deltaTime;
            if (alpha >= maximum) increasing = false;
            if (alpha <= minimum) increasing = true;
            alpha = increasing ? alpha += t * cyclesPerSecond * 2 : alpha -= t * cyclesPerSecond;
            srColor.a = alpha;
            sprite.color = srColor;
        }
    }
}