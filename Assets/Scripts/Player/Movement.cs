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

    public class Movement : MonoBehaviour
    {
        [Header("Physics")]
        private BoxCollider2D boxCol;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private LayerMask groundLayer;

        private void Start()
        {
            boxCol = GetComponent<BoxCollider2D>();
            normalGravity = rb.gravityScale;
            startDrag = rb.drag;
        }
        public void UpdateMovement(MovementInputs inputs)
        {
            Landing();

            movementScale = inputs.walk;
            jumpDown |= inputs.JumpDown;
            jumpReleased |= inputs.JumpUp;
        }

        public void FixedUpdate()
        {
            CheckCollisions();

            CalculateJumpApex();
            CalculateWalk();
            CalculateJump();

            MoveCharacterPhysics();
        }

        #region Collisions
        [Header("Collisions")]
        [SerializeField] private float rayLength = 0.3f;
        [SerializeField] private bool colFront;
        [SerializeField] private bool colDown;
        [SerializeField] private bool colBack;
        private bool landingThisFrame;

        public bool isGrounded =>
           Physics2D.Raycast(transform.position,
               -Vector3.up, rayLength * 5, groundLayer) ||
           Physics2D.Raycast(new Vector3
               (transform.position.x + 0.01f, transform.position.y + boxCol.bounds.extents.y, transform.position.z),
               -Vector3.up, rayLength * 5, groundLayer) ||
           Physics2D.Raycast(new Vector3
               (transform.position.x + 0.01f, transform.position.y - boxCol.bounds.extents.y, transform.position.z),
               -Vector3.up, rayLength * 5, groundLayer);

        private void CheckCollisions()
        {
            landingThisFrame = false;

            if (colDown && !isGrounded)
                timeLeftGrounded = Time.time;
            if (!colDown && isGrounded)
            {
                coyoteUsable = true;
                landingThisFrame = true;
            }
            if ((colBack || colFront) && !isHanging)
                timeLeftWall = Time.time;

            colDown = isGrounded;

            CheckRays();
        }

        private void CheckRays()
        {
            var pos = transform.position;
            var extent = boxCol.bounds.extents;

            if (isFacingRight)
            {
                colFront = Physics2D.Raycast(pos, Vector3.right, rayLength, groundLayer) ||
                    Physics2D.Raycast(new Vector3(pos.x + extent.x, pos.y, pos.z),
                    Vector3.right, rayLength, groundLayer) ||
                    Physics2D.Raycast(new Vector3(pos.x - extent.x, pos.y, pos.z),
                    Vector3.right, rayLength, groundLayer);

                colBack = Physics2D.Raycast(pos, -Vector3.right, rayLength, groundLayer) ||
                    Physics2D.Raycast(new Vector3(pos.x + extent.x, pos.y, pos.z),
                    -Vector3.right, rayLength, groundLayer) ||
                    Physics2D.Raycast(new Vector3(pos.x - extent.x, pos.y, pos.z),
                    -Vector3.right, rayLength, groundLayer);
            }
            else
            {
                colFront = Physics2D.Raycast(pos, -Vector3.right, rayLength, groundLayer) ||
                    Physics2D.Raycast(new Vector3(pos.x + extent.x, pos.y, pos.z),
                    -Vector3.right, rayLength, groundLayer) ||
                    Physics2D.Raycast(new Vector3(pos.x - extent.x, pos.y, pos.z),
                    -Vector3.right, rayLength, groundLayer);

                colBack = Physics2D.Raycast(pos, Vector3.right, rayLength, groundLayer) ||
                    Physics2D.Raycast(new Vector3(pos.x + extent.x, pos.y, pos.z),
                    Vector3.right, rayLength, groundLayer) ||
                    Physics2D.Raycast(new Vector3(pos.x - extent.x, pos.y, pos.z),
                    Vector3.right, rayLength, groundLayer);
            }

        }

        #endregion

        #region Walk
        [Header("Walk")]
        [SerializeField] private float maxSpeed = 120f;
        [SerializeField] private float acceleration = 10.0f;
        [SerializeField] private float deceleration = 5.0f;
        [SerializeField] private float velPower = 10.5f;

        private float movementScale;
        private float targetSpeed;
        private float speedDif;
        private float accelRate;
        private float movement;

        private void CalculateWalk()
        {
            targetSpeed = movementScale * maxSpeed;
            speedDif = targetSpeed - rb.velocity.x;
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
            movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        }

        #endregion

        #region Jump 
        [Header("Jump")]
        [SerializeField] private float jumpHeight = 10.0f;
        [SerializeField] private float apexThreshold = 0.1f;
        [SerializeField] private float apexGravity = 3f;
        [SerializeField] private float airDrag = 5f;
        [SerializeField] private float wallDrag = 20f;
        [SerializeField] private float airControl = 0.5f;
        [SerializeField] private float earlyJumpModifier = 2.5f;
        private bool shouldJump;
        private float startDrag;
        private bool jumpDown;
        private bool jumpReleased;
        private float normalGravity;
        private bool isInApex;
        private bool endedJumpEarly;

        [Header("Wall Jump")]
        [SerializeField] private float forceOfSideJumpSide = 0.5f;
        [SerializeField] private float forceOfSideJumpUp = 2.0f;
        [SerializeField] private bool shouldWallJump;
        private bool isHanging =>
            !colDown && colFront && movementScale != 0; // this line might have to change

        [Header("Buffer and Coyote Time")]
        [SerializeField] private float jumpBuffer = 0.1f;
        [SerializeField] private float coyoteTimeThreshold = 0.5f;
        public float lastJumpInput;
        private float timeLeftGrounded;
        private bool coyoteUsable;
        private float timeLeftWall;
        private bool HasJumpBuffered =>
            colDown && lastJumpInput + jumpBuffer > Time.time;
        private bool CanUseCoyote =>
            coyoteUsable && !colDown && timeLeftGrounded + coyoteTimeThreshold > Time.time;
        private bool HasWallJumpBuffered =>
            isHanging && lastJumpInput + jumpBuffer > Time.time;
        private bool CanUseWallCoyote =>
            coyoteUsable && !isHanging && timeLeftWall + coyoteTimeThreshold > Time.time;

        [Header("Bullet Time")]
        [SerializeField] private float bulletTimeControl = 1.5f;

        private void CalculateJumpApex()
        {
            if (!colDown)
                isInApex = Mathf.Abs(rb.velocity.y) < apexThreshold && !colDown;
            else
                isInApex = false;
        }
        private void CalculateJump()
        {
            if (jumpDown && (CanUseCoyote || HasJumpBuffered))
            {
                shouldJump = true;
                endedJumpEarly = false;
                coyoteUsable = false;
                timeLeftGrounded = float.MinValue;
            }

            if (jumpDown && (CanUseWallCoyote || HasWallJumpBuffered))
            {
                shouldWallJump = true;
                coyoteUsable = false;
                timeLeftWall = float.MinValue;
            }

            if (!colDown && jumpReleased && !endedJumpEarly && rb.velocity.y > 0)
            {
                endedJumpEarly = true;
            }

            jumpDown = false;
            jumpReleased = false;
        }

        #endregion

        #region Dash
        [Header("Dash")]
        [SerializeField] private float dashDistance;


        #endregion

        #region Move

        public bool isFacingRight { get; private set; } = true;

        private void MoveCharacterPhysics()
        {
            //JUMP
            if (shouldJump)
            {
                if (!colDown)
                    rb.velocity = Vector3.zero;
                rb.AddForce(jumpHeight * Vector2.up, ForceMode2D.Impulse);

                shouldJump = false;
            }

            if (shouldWallJump)
            {
                rb.AddForce(jumpHeight * Vector2.up * forceOfSideJumpUp, ForceMode2D.Impulse);
                if (!isFacingRight)
                    rb.AddForce(jumpHeight * Vector2.right * forceOfSideJumpSide, ForceMode2D.Impulse);
                else
                    rb.AddForce(jumpHeight * -Vector2.right * forceOfSideJumpSide, ForceMode2D.Impulse);

                shouldWallJump = false;
            }

            if (endedJumpEarly)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / earlyJumpModifier);
                endedJumpEarly = false;
            }

            rb.gravityScale = isInApex ? apexGravity : normalGravity;

            // DRAG
            if (isHanging)
                rb.drag = wallDrag;
            else if (!colDown)
                rb.drag = airDrag;
            else
                rb.drag = startDrag;

            //GROUND MOVEMENT
            if (BulletTime.instance.isActive)
                movement *= bulletTimeControl;
            else if (!colDown)
                movement *= airControl;

            rb.AddForce(movement * Vector2.right);

            if (isFacingRight && movementScale < 0)
                Flip();
            else if (!isFacingRight && movementScale > 0)
                Flip();
        }

        private void Flip()
        {
            var scale = transform.localScale;

            transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
            isFacingRight = !isFacingRight;
        }

        #endregion

        #region SFX

        private void Landing()
        {
            if (landingThisFrame)
                AudioManager.instance.LandingSFX();
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