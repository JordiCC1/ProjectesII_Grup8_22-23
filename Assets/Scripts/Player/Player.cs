using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;


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

        [HideInInspector] public bool isInvincible = false;
        [HideInInspector] public bool isBulletTimeActive = false;
        [HideInInspector] public bool isSwapped = false;

        [HideInInspector] public Vector3 targetPosition;


        private void Start()
        {
            movement = GetComponent<Movement>();
            bt = GetComponent<BulletTime>();
        }

        void Update()
        {
            TakeInputs();
            movement.UpdateMovement(moveInputs);
            bt.UpdateBulletTime(btInputs, CanBT());
            UpdateInvincible();
            UpdateSwapped();
        }

        #region Inputs

        private void TakeInputs()
        {
            moveInputs = new MovementInputs
            {
                walk = Input.GetAxisRaw("Horizontal"), //Raw makes it more snappy
                JumpDown = Input.GetButtonDown("Jump"),
                JumpUp = Input.GetButtonUp("Jump")
            };
            if (moveInputs.JumpDown)
                 movement.lastJumpInput = Time.time;


            btInputs = new BulletTimeInputs
            {
                BulletTimeDown = Input.GetMouseButtonDown(0),
                BulletTimeUp = Input.GetMouseButtonUp(0)
            };
        }
        
        private bool CanBT()
        {
            return (!(movement.isGrounded) && StaminaController.instance.stamina >= 0);
        }

        #endregion

        #region Collisions
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Bullet") && !isInvincible)
            {
                AudioManager.instance.PlayerDeathSFX();
                Destroy(gameObject);
                int scene = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(scene, LoadSceneMode.Single);
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
            else
            {

            }
        }

        public void RestartInvincibility()
        {
            StopCoroutine(ReturnToNormalState());
            isInvincible = false;
        }

        IEnumerator ReturnToNormalState()
        {
            yield return new WaitForSeconds(invincibilityTime);
            isInvincible = false;
            //Debug.Log("not Invicible");
        }
        #endregion

        #region Swap
        void UpdateSwapped()
        {
            Tween t;            
            if (isSwapped)
            {                                                 
                t = DOTween.To(() => gameObject.transform.position, x => gameObject.transform.position = x, targetPosition, 0.2f).SetEase(Ease.InOutQuad);               
                isSwapped = false;        
            }
                
        }        
        #endregion

    }
}