using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    //AudioManager.instance.LandingSFX();

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(BulletTime))]

    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private BulletTime bt;

        public MovementInputs inputs;

        [Header("Invincibility")]
        public bool isInvincible = false;

        private bool isBulletTimeActive;


        private void Start()
        {
            movement = GetComponent<PlayerMovement>();
            bt = GetComponent<BulletTime>();

            isBulletTimeActive = false;
        }

        void Update()
        {
            TakeInputs();
            movement.UpdateMovement(inputs, isBulletTimeActive);            
            bt.UpdateBulletTime(Input.GetMouseButtonDown(0), isBulletTimeActive, Input.GetMouseButtonUp(0), !movement.isGrounded);
            UpdateInvincible();
        }

        #region Inputs

        private void TakeInputs()
        {
            inputs = new MovementInputs
            {
                walk = Input.GetAxisRaw("Horizontal"), //Raw makes it more snappy
                JumpDown = UnityEngine.Input.GetButtonDown("Jump"),
                JumpUp = UnityEngine.Input.GetButtonUp("Jump")
            };
            if (inputs.JumpDown == true)
            {
                movement.lastJumpInput = Time.time;
            }

        }

        #endregion

        #region Collisions
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Bullet") && !isInvincible)
            {
                AudioManager.instance.PlayerDeathSFX();
                Destroy(gameObject);
                SceneManager.LoadScene(0);
                //StartCoroutine(WaitAndDie());
            }
        }

        #endregion

        #region Invincibility
        public float invincibilityTime = 0.0f;
        void UpdateInvincible()
        {
            if (isInvincible)
            {
                StartCoroutine("ReturnToNormalState");
            }
        }
        IEnumerator ReturnToNormalState() 
        {
            yield return new WaitForSeconds(invincibilityTime);
            isInvincible = false;
            Debug.Log("not Invicible");
        }
        #endregion
    }
}