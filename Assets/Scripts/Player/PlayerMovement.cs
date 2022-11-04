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

            /*
            coyoteTime = 0.5f;
            coyoteUsable = false;
            coyoteTimer = 0.0f;

            minFallSpeed = 10.0f;
            maxFallSpeed = 60.0f;
            */
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

        [SerializeField]
        private bool isGrounded =>
            Physics2D.Raycast(transform.position, -Vector3.up, boxCol.bounds.extents.y + 0.5f, groundLayer);

        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        [SerializeField] private bool colUp, colRight, colDown, colLeft;

        public void CheckCollisions()
        {
            colDown = isGrounded;
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
            shouldJump |= inputs.JumpDown && colDown;

            //bulletTimeActive = bt.isActive;
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

            isInApex = Mathf.Abs(rb.velocity.y) < apexThreshold && !isGrounded;
            rb.gravityScale = isInApex ? apexGravity : startGravity;

            //AIR DRAG
            /*if(bulletTimeActive && !isGrounded)
            {
                rb.drag = bulletTimeDrag;
            }
            else*/ if (!isGrounded)
            {
                rb.drag = isAirDrag;
            }
            else
                rb.drag = startDrag;

            //GROUND MOVEMENT
            Vector2 movementForce = Vector2.right * movementScale * maxSpeed * Time.fixedDeltaTime;
            if (!isGrounded)
            {
                movementForce *= airControl;
            }
            rb.AddForce(movementForce, ForceMode2D.Force);
        }

        #endregion
    }
}