using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;


namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]

    public class Player : MonoBehaviour
    {
        [SerializeField] private Movement movement;
        [SerializeField] private BulletTime bt;
        [SerializeField] private SpriteRenderer sprite;

        public MovementInputs moveInputs;
        public BulletTimeInputs btInputs;

        [HideInInspector] public bool isInvincible = false;
        [HideInInspector] public bool isBulletTimeActive = false;
        [HideInInspector] public bool isSwapped = false;
        [HideInInspector] public bool swaping = false;
        [HideInInspector] public bool isDead = false;
        [HideInInspector] public bool isAlive = true;

        public bool alternative;

        [HideInInspector] public Vector3 targetPosition;

        [HideInInspector] public Color originalColor;
        private Color targetColor;

        [SerializeField] PauseMenu pauseMenu;
        [SerializeField] StaminaController staminaController;

        [SerializeField] private GameObject echo;
        [SerializeField] private GameObject echo_L;
        [SerializeField] private float startTimeBetweenSpawns;
        private float timeBetweenSpawns;

        [HideInInspector] public CheckpointMaster cm;
        private ScreenWipe screenWipe;

        [Header("Audio")]
        public AudioClip deathSound;

        private void Start()
        {
            cm = GameObject.FindGameObjectWithTag("CM").GetComponent<CheckpointMaster>();
            screenWipe = FindObjectOfType<ScreenWipe>();
            movement = GetComponentInChildren<Movement>();
            bt = GetComponentInChildren<BulletTime>();
            sprite = GetComponentInChildren<SpriteRenderer>();
            originalColor = sprite.color;
            targetColor = new Color(1f, 1f, 1f, 0);
            Vector2 lastCheckPointPos = cm.lastCheckPointPos;
            transform.position = lastCheckPointPos;          
            
        }

        void Update()
        {
            if (!pauseMenu.isPaused)
                TakeInputs();
            movement.UpdateMovement(moveInputs);
            
            bt.UpdateBulletTime(btInputs, CanBT());
            UpdateSwapped();
            if (bt.trailOn)
                UpdateTrail();            
           
        }
        private void FixedUpdate()
        {
            movement.MoveCharacterInPlayer(isAlive,screenWipe.isDone);
        }
        #region Inputs
        private void TakeInputs()
        {
            moveInputs = new MovementInputs
            {
                walk = Input.GetAxisRaw("Horizontal"), //Raw makes it more snappy
                JumpDown = Input.GetButtonDown("Jump") ||
                    Input.GetKeyDown(KeyCode.W) ||
                    Input.GetKeyDown(KeyCode.UpArrow),
                JumpUp = Input.GetButtonUp("Jump") ||
                    Input.GetKeyUp(KeyCode.W) ||
                    Input.GetKeyUp(KeyCode.UpArrow)
            };
            if (moveInputs.JumpDown)
                movement.lastJumpInput = Time.time;


            btInputs = new BulletTimeInputs
            {
                BulletTimeDown = Input.GetMouseButtonDown(0),
                BulletTimeUp = Input.GetMouseButtonUp(0),
                SwapUp = Input.GetMouseButtonUp(1)
            };

            if (Input.GetButtonDown("Restart"))
            {
                RestartScene();
            }
        }

        private bool CanBT()
        {
            return (!(movement.isGrounded) && staminaController.stamina > 0.0f);
        }

        #endregion

        #region Collisions
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Bullet") && !isInvincible)
            {
                isDead = true;
                isAlive = false;
                AudioManager.PlayAudio2D(this.transform, deathSound);
                sprite.DOColor(targetColor, 0.2f);
                this.gameObject.tag = "aPlayer";               
            }else if (collision.gameObject.CompareTag("Trap") )
            {
                isDead = true;
                isAlive = false;
                AudioManager.PlayAudio2D(this.transform, deathSound);
                //Destroy(gameObject);
                sprite.DOColor(targetColor, 0.2f);
                this.gameObject.tag = "aPlayer";
            }
        }        
        IEnumerator WaitThenDie()
        {
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
            int scene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
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
        }

        void StopInvincibility()
        {
            StopCoroutine("ReturnToNormalState");
            isInvincible = false;
        }

        IEnumerator ReturnToNormalState()
        {
            yield return new WaitForSeconds(invincibilityTime);
            isInvincible = false;
            sprite.DOColor(originalColor, 0.5f);
        }

        #endregion

        #region Swap
        void UpdateSwapped()
        {
            Tween t;
            if (isSwapped)
            {
                t = DOTween.To(() => gameObject.transform.position, x => gameObject.transform.position = x, targetPosition, 0.15f).SetEase(Ease.InOutQuad);

                isSwapped = false;                
            }

        }

        
        #endregion

        #region trail
        void UpdateTrail()
        {
            if (timeBetweenSpawns <= 0)
            {
                if (movement.isFacingRight)
                {
                    GameObject instance = Instantiate(echo, transform.position, Quaternion.identity);
                    Destroy(instance, 2f);
                }
                else if(movement.isFacingRight == false)
                {
                    GameObject instance = Instantiate(echo_L, transform.position, Quaternion.identity);
                    Destroy(instance, 2f);                   
                }
                timeBetweenSpawns = startTimeBetweenSpawns;
                
            }
            else
            {                
                timeBetweenSpawns -= Time.deltaTime;                
            }
        }
        
        #endregion
        private void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}