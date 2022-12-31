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

    public class Player : MonoBehaviour
    {

        [SerializeField] private Movement movement;
        [SerializeField] private BulletTime bt;
        [SerializeField] private SpriteRenderer spriteRenderer;

        public MovementInputs moveInputs;
        public BulletTimeInputs btInputs;

        [HideInInspector] public bool isInvincible = false;
        [HideInInspector] public bool isBulletTimeActive = false;
        [HideInInspector] public bool isSwapped = false;

        public bool alternative;

        [HideInInspector] public Vector3 targetPosition;

        private Color originalColor;
        private Color targetColor;

        [SerializeField] PauseMenu pauseMenu;



        private void Start()
        {
            movement = GetComponentInChildren<Movement>();
            bt = GetComponentInChildren<BulletTime>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            originalColor = spriteRenderer.color;
            targetColor = new Color(1f, 1f, 1f, 1);
        }

        void Update()
        {
            if (!pauseMenu.isPaused)
                TakeInputs();
            movement.UpdateMovement(moveInputs);
            bt.UpdateBulletTime(btInputs, CanBT());
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

        public void Invincibility()
        {
            if (isInvincible)
            {
                StopInvincibility();
            }
            StartInvincibility();
        }

        void StartInvincibility()
        {
            isInvincible = true;
            StartCoroutine("ReturnToNormalState");
            //spriteRenderer.color = targetColor;
            spriteRenderer.DOColor(targetColor, 0.6f);
        }

        void StopInvincibility()
        {
            StopCoroutine("ReturnToNormalState");
            isInvincible = false;
            //spriteRenderer.color = originalColor;
            spriteRenderer.DOColor(originalColor, 0.5f);

        }

        IEnumerator ReturnToNormalState()
        {
            yield return new WaitForSeconds(invincibilityTime);
            isInvincible = false;
            //spriteRenderer.color = originalColor;
            spriteRenderer.DOColor(originalColor, 0.5f);
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