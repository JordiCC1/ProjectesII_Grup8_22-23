using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public struct MovementInputs
    {
        public float walk;
        public bool JumpDown;
        public bool JumpUp;
    }

    public class PlayerMovement : MonoBehaviour
    {
        [Header("Physics")]
        private BoxCollider2D boxCol;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private BulletTime bt;
        [SerializeField] private ParticleSystem particles;

        [SerializeField] private LayerMask groundLayer;


        private void Start()
        {
            boxCol = GetComponent<BoxCollider2D>();
            startGravity = rb.gravityScale;
            startDrag = rb.drag;
            wasGrounded = true;
        }
        public void UpdateMovement(MovementInputs inputs, bool _isBulletTimeActive)
        {
            Walk(inputs);
            Jump(inputs);
            WallJump(inputs);

            Landing();

            bulletTimeActive = _isBulletTimeActive;
        }

        public void FixedUpdate()
        {
            CheckCollisions();
            MoveCharacterPhysics();
        }

        #region Collisions
        [Header("Collisions")]
        [SerializeField] float rayLength = 0.3f;

        [SerializeField] private bool colUp;
        private bool colRight;
        [SerializeField] private bool colDown;
        private bool colLeft;
        public bool isGrounded =>
           Physics2D.Raycast(transform.position,
               -Vector3.up, boxCol.bounds.extents.y + rayLength, groundLayer)
           ||
           Physics2D.Raycast(new Vector3
               (transform.position.x, transform.position.y + boxCol.bounds.extents.y, transform.position.z),
               -Vector3.up, boxCol.bounds.extents.y + rayLength, groundLayer)
           ||
           Physics2D.Raycast(new Vector3
               (transform.position.x, transform.position.y - boxCol.bounds.extents.y, transform.position.z),
               -Vector3.up, boxCol.bounds.extents.y + rayLength, groundLayer);

        private void CheckCollisions()
        {
            if (colDown && !isGrounded)
                timeLeftGrounded = Time.time;
            if (!colDown && isGrounded)
                coyoteUsable = true;

            colDown = isGrounded;

            CheckRays();
        }

        private void CheckRays()
        {
            colUp = Physics2D.Raycast(transform.position, Vector3.up, boxCol.bounds.extents.y + rayLength, groundLayer) ||
                Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y + boxCol.bounds.extents.y, transform.position.z),
                Vector3.up, boxCol.bounds.extents.y + rayLength, groundLayer) ||
                Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - boxCol.bounds.extents.y, transform.position.z),
                Vector3.up, boxCol.bounds.extents.y + rayLength, groundLayer);

            colRight = Physics2D.Raycast(transform.position, Vector3.right, boxCol.bounds.extents.x + rayLength, groundLayer) ||
                Physics2D.Raycast(new Vector3(transform.position.x + boxCol.bounds.extents.x, transform.position.y, transform.position.z),
                Vector3.right, boxCol.bounds.extents.x + rayLength, groundLayer) ||
                Physics2D.Raycast(new Vector3(transform.position.x - boxCol.bounds.extents.x, transform.position.y, transform.position.z),
                Vector3.right, boxCol.bounds.extents.x + rayLength, groundLayer);

            colLeft = Physics2D.Raycast(transform.position, -Vector3.right, boxCol.bounds.extents.x + rayLength, groundLayer) ||
                Physics2D.Raycast(new Vector3(transform.position.x + boxCol.bounds.extents.x, transform.position.y, transform.position.z),
                -Vector3.right, boxCol.bounds.extents.x + rayLength, groundLayer) ||
                Physics2D.Raycast(new Vector3(transform.position.x - boxCol.bounds.extents.x, transform.position.y, transform.position.z),
                -Vector3.right, boxCol.bounds.extents.x + rayLength, groundLayer);
        }

        #endregion

        #region Walk
        [Header("Walk")]
        private float movementScale;
        [SerializeField] private float maxSpeed = 120f;
        [SerializeField] private float airControl = 0.5f;

        private void Walk(MovementInputs input)
        {
            movementScale = input.walk;
        }

        #endregion

        #region Jump
        [Header("Jump")]
        [SerializeField] private float jumpForce = 10.0f;
        bool shouldJump = false;
        private float startDrag;
        [SerializeField] private float isAirDrag = 5f;
        [SerializeField] private float wallDrag = 20f;

        [Header("Bullet Time")]
        private bool bulletTimeActive;
        [SerializeField] private float bulletTimeControl = 1.5f;

        [Header("Buffer and Coyote Time")]
        [SerializeField] private float jumpBuffer = 0.1f;
        public float lastJumpInput;
        [SerializeField] float coyoteTimeThreshold = 0.5f;
        private float timeLeftGrounded;
        private bool coyoteUsable;

        private bool HasJumpBuffered => colDown && lastJumpInput + jumpBuffer > Time.time;
        private bool CanUseCoyote => coyoteUsable && !colDown && timeLeftGrounded + coyoteTimeThreshold > Time.time;

        [Header("Jump Apex")]
        [SerializeField] private float apexThreshold = 0.1f;
        private bool isInApex = false;
        private float startGravity;
        [SerializeField] private float apexGravity = 3f;

        [Header("Wall Jump")]
        [SerializeField] private float forceOfSideJumpSide = 0.5f;
        [SerializeField] private float forceOfSideJumpUp = 2.0f;
        [SerializeField] private bool canWallJump;
        private bool isHanging => (colLeft && !colDown) || (colRight && !colDown);

        private void Jump(MovementInputs inputs)
        {
            //TODO: implement Bullet time into this script

            if (inputs.JumpDown && CanUseCoyote || HasJumpBuffered)
            {
                shouldJump = true;
                timeLeftGrounded = float.MinValue;
            }
        }

        private void WallJump(MovementInputs inputs)
        {
            if (inputs.JumpDown && isHanging)
            {
                canWallJump = true;
            }
        }

        #endregion

        #region Move

        private void MoveCharacterPhysics()
        {
            //JUMP
            if (shouldJump)
            {
                if (!colDown)
                    rb.velocity = Vector3.zero;
                rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
                shouldJump = false;
            }

            if (canWallJump)
            {
                rb.AddForce(jumpForce * Vector2.up * forceOfSideJumpUp, ForceMode2D.Impulse);
                if (colLeft)
                    rb.AddForce(jumpForce * Vector2.right * forceOfSideJumpSide, ForceMode2D.Impulse);
                if (colRight)
                    rb.AddForce(jumpForce * -Vector2.right * forceOfSideJumpSide, ForceMode2D.Impulse);

                canWallJump = false;
            }

            isInApex = Mathf.Abs(rb.velocity.y) < apexThreshold && !colDown;
            rb.gravityScale = isInApex ? apexGravity : startGravity;

            //AIR DRAG
            //WALL DRAG

            if (isHanging)
                rb.drag = wallDrag;
            else if (!colDown)
                rb.drag = isAirDrag;
            else
                rb.drag = startDrag;

            //GROUND MOVEMENT
            Vector2 movementForce = Vector2.right * movementScale * maxSpeed * Time.fixedDeltaTime;
            if (bulletTimeActive)
                movementForce *= bulletTimeControl;
            else if (!colDown)
                movementForce *= airControl;



            rb.AddForce(movementForce, ForceMode2D.Force);
        }

        #endregion

        #region SFX

        private bool wasGrounded;

        private void Landing()
        {
            if (!isGrounded)
            {
                wasGrounded = false;
            }
            else if (isGrounded && !wasGrounded)
            {
                AudioManager.instance.LandingSFX();
                wasGrounded = true;
            }

        }

        #endregion

        #region Death
        IEnumerator WaitAndDie()
        {
            yield return new WaitForSeconds(2);
            SceneManager.LoadScene(0);
        }
        #endregion
    }
}