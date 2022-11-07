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
        [SerializeField] private BulletTime bt;

        public MovementInputs inputs;
        public bool bulletTimeInput;

        [SerializeField] bool isBulletTimeActive;


        private void Start()
        {
            movement = GetComponent<PlayerMovement>();
            bt = GetComponent<BulletTime>();

            isBulletTimeActive = false;
        }

        void Update()
        {
            TakeInputs();
            movement.UpdateMovement(inputs, isBulletTimeActive);
            bt.UpdateBulletTime(bulletTimeInput, isBulletTimeActive);

            isBulletTimeActive = bulletTimeInput;
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
            if (inputs.JumpDown == true)
            {
                movement.lastJumpInput = Time.time;
            }

            bulletTimeInput = Input.GetButton("Fire1");
        }

        #endregion
    }
}