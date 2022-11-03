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
        private BoxCollider2D boxCol;
        [SerializeField] private LayerMask groundLayer;

        Vector3 currentVelocity;

        private void Start()
        {
            boxCol = GetComponent<BoxCollider2D>();

            colDown = false;
            colLeft = false;
            colRight = false;
            colUp = false;

            maxSpeed = 120f;
            acceleration = 40f;
            deceleration = 10f;

            jumpForce = 10.0f;
            //coyoteTime = 0.5f;
            //coyoteUsable = false;
            //coyoteTimer = 0.0f;

            //minFallSpeed = 10.0f;
            //maxFallSpeed = 60.0f;
        }

        #region Collisions

        [SerializeField] private bool isGrounded => 
            Physics2D.Raycast(transform.position, -Vector3.up, boxCol.bounds.extents.y + 0.5f, groundLayer);

        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        [SerializeField] private bool colUp, colRight, colDown, colLeft;

        public void CheckCollsions()
        {
            colDown = isGrounded;
        }

        #endregion

        #region Walk

        [SerializeField] private float maxSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] private float deceleration;

        public void Walk(MovementInputs input)
        {
            if (input.walk != 0)
            {
                currentVelocity.x += input.walk * acceleration * Time.deltaTime;

                currentVelocity.x = Mathf.Clamp(currentVelocity.x, -maxSpeed, maxSpeed);
            }
            //else if the player is in Bullet time
            else
            {
                currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, 0, deceleration * Time.deltaTime);
            }

            //if(currentVelocity.x > 0 && playerCollider)
            {

            }
        }
        #endregion

        #region Jump

        [SerializeField] private float jumpForce;

        #endregion

        #region Move

        public void MoveCharacter()
        {
            var move = currentVelocity * Time.deltaTime;

            transform.position += move;
        }

        #endregion
    }
}