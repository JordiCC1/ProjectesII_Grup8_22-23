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

        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Fall = Animator.StringToHash("Fall");
        private static readonly int Land = Animator.StringToHash("Land");
        private static readonly int Hang = Animator.StringToHash("Hang");

        private int currentState;
        private float landTime;

        //blink
        private float minimum = 0.3f;
        private float maximum = 1.0f;
        private float cyclesPerSecond = 2.0f;
        private float alpha;
        private bool increasing = true;
        private Color srColor;

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
            
        }
        private void Update()
        {
            if (player.isDead)
            {
                DeathAnimation();
            }
        }

        private int GetState()
        {
            if (Time.time < landTime) return currentState;

            // Priorities
            if (movement.landingThisFrame) return LockState(Land, 0.1f);
            if (movement.isHanging)
            {
                //TODO: lock flip for a while
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
            //player.GetComponent<Collider2D>().enabled = false;
            player.isDead = false;
        }

        IEnumerator WaitAndMove()
        {
            Debug.Log("DIE");
            GameObject ParticleIns = Instantiate(deathParticles, transform.position, Quaternion.identity);
            ParticleIns.GetComponent<ParticleSystem>().Play();            
            //Seconds to wait after player death
            yield return new WaitForSeconds(1.5f);
            Tween t;
            //player.GetComponent<Collider2D>().enabled = false;
            t = DOTween.To(() => player.transform.position, x => player.transform.position = x, (Vector3)player.cm.lastCheckPointPos, 0.3f).SetEase(Ease.InOutQuad);
            // player.GetComponent<Collider2D>().enabled = true;
            StartCoroutine("WaitAndRestart");
        }
        IEnumerator WaitAndRestart()
        {
            //sprite.DOColor(player.originalColor, 0.2f);
            yield return new WaitForSeconds(1);
            int scene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
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