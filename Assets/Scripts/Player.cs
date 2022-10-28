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
        private Rigidbody2D rb;
        private BoxCollider2D playerCollider;
        [SerializeField] private PlayerMovement movement;

        public MovementInputs input;


        private void Start()
        {
            //Collisions
            rb = GetComponent<Rigidbody2D>();
            playerCollider = GetComponent<BoxCollider2D>();
            movement = GetComponent<PlayerMovement>();
        }

        void Update()
        {

            TakeInputs();

            movement.CheckCollsions();
            movement.Walk(input);
            movement.Jump(input);

            movement.MoveCharacter();
            
        }

        #region Inputs
        void TakeInputs()
        {
            input.walk = Input.GetAxisRaw("Horizontal"); //Raw makes it more snappy

            input.JumpDown = UnityEngine.Input.GetButtonDown("Jump");
            input.JumpUp = UnityEngine.Input.GetButtonUp("Jump");
        }

        #endregion


    }
}

/*public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Player player;

    [SerializeField] private float minJumpDuration;
    [SerializeField] private float maxJumpDuration;

    private float movement;
    private float jump;

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    private int extraJumps;
    private int maxJumps;

    private void Start()
    {
        //Make Player move right for a bit
        player = GetComponent<Player>();

        //Set values up
        extraJumps = maxJumps;
        maxJumps = 1;
    }

    private void Update()
    {
        if (isGrounded)
        {
            extraJumps = maxJumps;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && extraJumps > 0)
        {
            player.Jump();
            extraJumps--;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && extraJumps == 0 && isGrounded)
        {
            player.Jump();
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        movement = Input.GetAxisRaw("Horizontal"); //Raw makes it more snappy
        player.Walk(movement);

        jump = Input.GetAxisRaw("Vertical");
        //player.Jump(jump);
    }
}
*/

/*public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void IdleAnimation()
    {

        animator.SetBool("isRising", false);
        animator.SetBool("isFalling", false);
    }

    public void JumpAnimation()
    {
        IdleAnimation();
        Rising();
        //make it wait before going to Falling
        Falling();
    }

    private void Rising()
    {
        animator.SetBool("isRising", true);
    }

    private void Falling()
    {
        animator.SetBool("isRising", false);
    }

    public void WalkAnimation()
    {

    }
}*/

/*public class Player : MonoBehaviour
{
    //Components
    [SerializeField] private PlayerAnimation anim;
    public Rigidbody2D playerRB;

    //Variables
    private float moveSpeed;
    private float jumpForce;

    private bool direction;

    //Functions
    private void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<PlayerAnimation>();

        moveSpeed = 10;
        jumpForce = 7;

        direction = true;
    }

    private void Update()
    {

    }

    public void Walk(float input)
    {
        playerRB.velocity = new Vector2(input * moveSpeed, playerRB.velocity.y);

        if (direction == false && input > 0)
        {
            Flip();
        }
        else if (direction == true && input < 0)
        {
            Flip();
        }
    }

    public void Jump()
    {
        playerRB.velocity = Vector2.up * jumpForce;
    }

    void Flip()
    {
        direction = !direction;

        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}
*/