using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(PlayerMovement))]


    public class Player : MonoBehaviour
    {
        //private Rigidbody2D rb;
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

            movement.CheckCollsions();
            movement.Walk(inputs);
            //movement.Jump(inputs);

            movement.MoveCharacter();   
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
        }

        #endregion
    }
}