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
        private void Start()
        {

            //bounds = playerCollider.bounds;
            isGrounded = false;

            colDown = false;
            colLeft = false;
            colRight = false;
            colUp = false;

            maxSpeed = 120f;
            acceleration = 40f;
            deceleration = 10f;

            jumpForce = 4.0f;
            coyoteTime = 2.0f;
        }

        #region Collisions

        //[SerializeField] private Bounds bounds;
        [SerializeField] private bool isGrounded;
        [SerializeField] private LayerMask _groundLayer;

        private bool colUp, colRight, colDown, colLeft;

        public void CheckCollsions()
        {

            isGrounded = colDown;


        }

        #endregion

        #region Walk
        Vector3 currentVelocity;

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
        [SerializeField] private float coyoteTime;
        private bool coyoteUsable = true;
        public void Jump(MovementInputs input)
        {
            if (input.JumpDown)
            {
                if (isGrounded || coyoteUsable) //add coyote time
                {
                    currentVelocity.y += jumpForce;
                    //coyoteUsable = false;
                }

            }
            else
            {

            }

        }
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