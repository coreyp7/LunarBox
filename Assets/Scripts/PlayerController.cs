using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float jumpForce;

    private float horizontalInput;
    private float verticalInput;

    private bool jumpBtnPressed;

    public bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;

    public bool jumping;

    public float holdJumpBtnLength; // default rn is 0.3f
    
    private float jumpTime;
    
    
    private float jumpTimeLimit;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
    }


    // Start is called before the first frame update
    void Start()
    {


    }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        //verticalInput = Input.GetAxis("Vertical");

        //jumpBtnPressed = Input.GetKeyDown(KeyCode.P);
        //jumpHandling();

        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if(isGrounded && Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("ENTER 1");
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumping = true;
            // set jumpTimeLimit to timestamp (like 0.3 into the future or something)
            jumpTimeLimit = Time.time + holdJumpBtnLength;
            jumpTime = Time.time;
        }

        // If they are holding jump btn & we haven't hit the jumpTimeLimit, then continue adding velocity.
        // Otherwise, turn off jumping mode.

        // idea: check before all of this if key is let go (KeyUp) && jumping, then turn off jumping immediately.
        if(jumping && Input.GetKeyUp(KeyCode.P))
        {
            jumping = false;
        }
        else if (jumping && (jumpTime > jumpTimeLimit) ){
            /** If either:
             * 1. the player lets go of the jump btn
             * or
             * 2. the jumpTimeLimit is reached
             * ...then turn off jumping.
             */
            /*
             * TODO: don't allow the user to jump forever. Set a limit to the height of the jump.
             * Then mess with the quickness of the player's gravity when falling.
             */

            jumping = false;
        }
        else if (jumping && Input.GetKey(KeyCode.P) && (jumpTime < jumpTimeLimit)) // last part is extra, could remove
        {
            Debug.Log("jumping && Input.GetKey(KeyCode.P)");
            jumpTime += Time.deltaTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        //if(!jumping && rb.velocity.y < 0f)
        //{
        //    rb.velocity = new Vector2(rb.velocity.x, -1f * jumpForce);
        //}

        // if p is being held down and player is not grounded
        // if still before jumptimer
        //      then continue increasing jump velocity
        // else:
        //      then ignore

        //if(!isGrounded && Input.GetKey(KeyCode.P) && jumping)
        //{
        //    // if jumpTimeLimit is above 0 then increase velocity still.
        //    if(jumpTime < jumpTimeLimit)
        //    {
        //        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        //        Debug.Log("ENTER 2");
        //    } else
        //    {
        //        // the time of the jump is gone, turn off jumping
        //        jumping = false;
        //    }
        //}


    }

    void FixedUpdate()
    {
        // Horizontal movement first:

        // move left or right depending on input.
        horizontalMovement();

        // Vertical movement next:
        //jumpHandling();
    }

    /**
     * Will apply forces onto rigidbody depending on the input from the user.
    */

    private void horizontalMovement()
    {
        if (this.horizontalInput != 0) //on ground handling starts here (alot easier)
        {
            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }

    private void jumpHandling()
    {
        if (this.jumpBtnPressed)
        {

            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        }
    }
}
