using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    public struct FrameInput
    {
        public float X;
        public bool JumpDown;
        public bool JumpUp;
    }

    public class Player : MonoBehaviour
    {
        //Variables
        //Inputs
        public FrameInput input;

        //Stats
        Vector3 currentSpeed;
        Vector3 lastPosition;

        public float jumpForce = 4f;
        public float maxSpeed = 15f;
        public float minSpeed = 5f;
        public float walkAcceleration = 4f;

        //
        bool isGrounded;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            currentSpeed = (transform.position - _lastPosition) / Time.deltaTime;
            _lastPosition = transform.position;

            isGrounded = true;
        }

        void Walk()
        {
            if (isGrounded)
            {
                if (currentSpeed.x < maxSpeed)
                {
                    currentSpeed.x += walkAcceleration;
                }
                if (currentSpeed.x > maxSpeed)
                {
                    currentSpeed.x = maxSpeed;
                }
                else if (currentSpeed.x < minSpeed)
                {
                    currentSpeed.x = minSpeed;
                }

            }
        }
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