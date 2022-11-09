using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    //AudioManager.instance.PlayerDeathSFX();
    //AudioManager.instance.LandingSFX();

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(BulletTime))]

    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private BulletTime bt;

        public MovementInputs inputs;


        public bool isBulletTimeActive;


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
            bt.UpdateBulletTime(Input.GetMouseButtonDown(0), isBulletTimeActive,Input.GetMouseButtonUp(0), !movement.isGrounded);
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

        }

        #endregion
    }
}