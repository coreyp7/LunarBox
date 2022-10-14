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

    public float jumpTime;
    public float jumpTimeLimit;

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
            jumpTimeLimit = Time.time + 0.3f;
            jumpTime = Time.time;
        }

        if (jumping && Input.GetKey(KeyCode.P))
        {
            jumpTime += Time.deltaTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else if((jumping && Input.GetKeyUp(KeyCode.P)) || (jumpTime > jumpTimeLimit) ){ // when the jump button is released
            //TODO: also if the jump time limit has been passed, execute this.
            jumping = false;
        }
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
