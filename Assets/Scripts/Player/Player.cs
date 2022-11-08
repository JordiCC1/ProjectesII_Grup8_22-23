using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(BulletTime))]

    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerMovement movement;

        public MovementInputs inputs;

        private void Start()
        {
            //rb = GetComponent<Rigidbody2D>();
            movement = GetComponent<PlayerMovement>();
        }

        void Update()
        {
            TakeInputs();
            movement.UpdateMovement(inputs);  
        }

        #region Inputs

        private void TakeInputs()
        {
            inputs = new MovementInputs
            {
                walk = Input.GetAxisRaw("Horizontal"), //Raw makes it more snappy
                JumpDown = UnityEngine.Input.GetButtonDown("Jump"),
                JumpUp = UnityEngine.Input.GetButtonUp("Jump")
            };
            if (inputs.JumpUp == true)
            {
                movement.lastJumpInput = Time.time;
            }
        }

        #endregion
    }
}