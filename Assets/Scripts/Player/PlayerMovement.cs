using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        [SerializeField] private LayerMask groundLayer;


        private void Start()
        {
            boxCol = GetComponent<BoxCollider2D>();
            startGravity = rb.gravityScale;
            startDrag = rb.drag;
        }
        public void UpdateMovement(MovementInputs inputs)
        {
            Walk(inputs);
            Jump(inputs);
        }
        public void FixedUpdate()
        {
            CheckCollisions();
            MoveCharacterPhysics();
        }

        #region Collisions

        //colDown = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        [SerializeField] private bool colUp;
        [SerializeField] private bool colRight;
        [SerializeField] private bool colDown;
        [SerializeField] private bool colLeft;
        private bool isGrounded =>
            Physics2D.Raycast(transform.position, -Vector3.up, boxCol.bounds.extents.y + 0.5f, groundLayer);

        public void CheckCollisions()
        {
            if (colDown && !isGrounded)
                timeLeftGrounded = Time.time;
            if (!colDown && isGrounded)
                coyoteUsable = true;

        }

        private void CheckRays()
        {
            colUp = Physics2D.Raycast(transform.position, Vector3.up, boxCol.bounds.extents.y + 0.5f, groundLayer);
            colRight = Physics2D.Raycast(transform.position, Vector3.right, boxCol.bounds.extents.y + 0.5f, groundLayer);
            colLeft = Physics2D.Raycast(transform.position, -Vector3.right, boxCol.bounds.extents.y + 0.5f, groundLayer);
        }

        #endregion

        #region Walk
        [Header("Walk")]
        private float movementScale;
        [SerializeField] private float maxSpeed = 120f;
        [SerializeField] private float airControl = 0.5f;
        [SerializeField] private float acceleration = 40f;
        [SerializeField] private float deceleration = 10f;

        public void Walk(MovementInputs input)
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

        [Header("Buffer and Coyote Time")]
        [SerializeField] private float jumpBuffer = 0.1f;
        public float lastJumpInput;
        public float coyoteTimeThreshold = 0.5f;
        public float timeLeftGrounded;
        private bool coyoteUsable;

        private bool HasJumpBuffered => colDown && lastJumpInput + jumpBuffer > Time.time;
        private bool CanUseCoyote => coyoteUsable && !colDown && timeLeftGrounded + coyoteTimeThreshold > Time.time;

        [Header("Jump Apex")]
        [SerializeField] private float apexThreshold = 0.1f;
        private bool isInApex = false;
        private float startGravity;
        [SerializeField] private float apexGravity = 3f;

        [Header("Bullet Time")]
        private bool bulletTimeActive;
        [SerializeField] private float bulletTimeDrag = 10f;

        public void Jump(MovementInputs inputs)
        {
            //TODO: implement Bullet time into this script

            if (inputs.JumpDown && CanUseCoyote || HasJumpBuffered)
            {
                shouldJump = true;
                timeLeftGrounded = float.MinValue;
            }

            if (!colDown)
            {

            }

        }

        #endregion

        #region Move

        public void MoveCharacterPhysics()
        {
            //JUMP
            if (shouldJump)
            {
                rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
                shouldJump = false;
            }

            isInApex = Mathf.Abs(rb.velocity.y) < apexThreshold && !colDown;
            rb.gravityScale = isInApex ? apexGravity : startGravity;

            //AIR DRAG
            /*if(bulletTimeActive && !colDown)
            {
                rb.drag = bulletTimeDrag;
            }
            else*/
            if (!colDown)
            {
                rb.drag = isAirDrag;
            }
            else
                rb.drag = startDrag;

            //GROUND MOVEMENT
            Vector2 movementForce = Vector2.right * movementScale * maxSpeed * Time.fixedDeltaTime;
            if (!colDown)
            {
                movementForce *= airControl;
            }
            rb.AddForce(movementForce, ForceMode2D.Force);
        }

        #endregion
    }
}