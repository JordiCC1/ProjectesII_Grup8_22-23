using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace Player
{
    public class Animate : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private Movement movement;
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private GameObject deathParticles;
        [SerializeField] private GameObject landEffect;
        [SerializeField] private GameObject leftWallEffect;
        [SerializeField] private GameObject rightWallEffect;

        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Fall = Animator.StringToHash("Fall");
        private static readonly int Land = Animator.StringToHash("Land");
        private static readonly int Hang = Animator.StringToHash("Hang");

        private int currentState;
        private float landTime;

        private bool effectPlaying;

        //blink
        private float minimum = 0.3f;
        private float maximum = 1.0f;
        private float cyclesPerSecond = 2.0f;
        private float alpha;
        private bool increasing = true;
        private Color srColor;

        //Wipe
        private ScreenWipe screenWipe;
        private void Start()
        {
            player = GetComponentInParent<Player>();
            movement = GetComponent<Movement>();
            animator = GetComponent<Animator>();
            sprite = GetComponent<SpriteRenderer>();

            animator.CrossFade(Idle, 0, 0);

            //blink
            srColor = sprite.color;
            alpha = maximum;

            screenWipe = FindObjectOfType<ScreenWipe>();
        }

        private void FixedUpdate()
        { 
            //remove later
            if (player.isInvincible)
                Blink();

            var state = currentState;

            state = GetState();

            if (state == currentState) return;
            animator.CrossFade(state, 0, 0);
            currentState = state;
            
            //if (movement.shouldWallJump)
            //{
            //    if (movement.isFacingRight)
            //        LeftWallJumpEffect();
            //    else if (!movement.isFacingRight)
            //        RightWallJumpEffect();
            //}

        }
        private void Update()
        {
            if (player.isDead)
            {
                DeathAnimation();
            }


            if (movement.landingThisFrame)
                LandEffect();

        }

        private int GetState()
        {
            if (Time.time < landTime) return currentState;

            // Priorities
            if (movement.landingThisFrame) return LockState(Land, 0.1f);
            if (movement.isOnWall)
            {
                sprite.flipX = true;
                return Hang;
            }
            else
            {
                sprite.flipX = false;
            }
            if (movement.shouldJump || movement.shouldWallJump) return Jump;

            if (movement.colDown) return movement.movementScale == 0 ? Idle : Walk;
            return movement.rb.velocity.y > 0.5f ? Jump : Fall;

            int LockState(int s, float t)
            {
                landTime = Time.time + t;
                return s;
            }
        }


        public void DeathAnimation()
        {
            GameObject ParticleIns = Instantiate(deathParticles, transform.position, Quaternion.identity);
            ParticleIns.GetComponent<ParticleSystem>().Play();
            StartCoroutine("WaitAndMove");
            player.isDead = false;
            
        }

        IEnumerator WaitAndMove()
        {
            GameObject ParticleIns = Instantiate(deathParticles, transform.position, Quaternion.identity);
            ParticleIns.GetComponent<ParticleSystem>().Play();
            yield return new WaitForSeconds(0.3f);            
            StartCoroutine("WaitAndRestart");
        }
        IEnumerator WaitAndRestart()
        {
            screenWipe.ToggleWipe(true);
            yield return new WaitForSeconds(1.2f);
            int scene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }

        private void LandEffect()
        {
            if(!effectPlaying)
            {
                effectPlaying = true;
                GameObject instance = Instantiate(landEffect, transform.position, Quaternion.identity);
                Destroy(instance, 0.2f);
                effectPlaying = false;
            }
        }

        private void LeftWallJumpEffect()
        {
            if (!effectPlaying)
            {
                Debug.Log("L");
                effectPlaying = true;
                GameObject instance = Instantiate(leftWallEffect, transform.position, Quaternion.identity);
                Destroy(instance, 0.5f);
                effectPlaying = false;
            }
        }
        private void RightWallJumpEffect()
        {
            if (!effectPlaying)
            {
                Debug.Log("R");
                effectPlaying = true;
                GameObject instance = Instantiate(rightWallEffect, transform.position, Quaternion.identity);
                Destroy(instance, 0.5f);
                effectPlaying = false;
            }
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