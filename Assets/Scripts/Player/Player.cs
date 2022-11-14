using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    //AudioManager.instance.LandingSFX();

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Movement))]
    [RequireComponent(typeof(BulletTime))]

    public class Player : MonoBehaviour
    {
        [SerializeField] private Movement movement;
        [SerializeField] private BulletTime bt;

        public MovementInputs moveInputs;
        public BulletTimeInputs btInputs;

        public bool isInvincible = false;


        public bool isBulletTimeActive;


        private void Start()
        {
            movement = GetComponent<Movement>();
            bt = GetComponent<BulletTime>();

            isBulletTimeActive = false;
        }

        void Update()
        {
            TakeInputs();
            movement.UpdateMovement(moveInputs, isBulletTimeActive);
            bt.UpdateBulletTime(btInputs, isBulletTimeActive, !movement.isGrounded);
            UpdateInvincible();
        }

        #region Inputs

        private void TakeInputs()
        {
            moveInputs = new MovementInputs
            {
                walk = Input.GetAxisRaw("Horizontal"), //Raw makes it more snappy
                JumpDown = UnityEngine.Input.GetButtonDown("Jump"),
                JumpUp = UnityEngine.Input.GetButtonUp("Jump")
            };
            if (moveInputs.JumpDown == true)
                movement.lastJumpInput = Time.time;


            btInputs = new BulletTimeInputs
            {
                BulletTimeDown = Input.GetMouseButtonDown(0),
                BulletTimeUp = Input.GetMouseButtonUp(0)
            };
            if (btInputs.BulletTimeDown == true)
                isBulletTimeActive = true;
            if (btInputs.BulletTimeUp == true)
                isBulletTimeActive = false;
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