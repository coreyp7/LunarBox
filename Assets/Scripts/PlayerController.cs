using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /**
     * Gravity scale: 4.04
     * Angular drag: 0.05
     * Linear drag: 0
     * Mass: 1
     */
    private Rigidbody2D rb;


    // default: 7
    [SerializeField]
    private float moveSpeed;

    // default: 8
    [SerializeField]
    private float jumpForce;

    private float horizontalInput;
    private float verticalInput;

    private bool jumpBtnPressed;

    public bool isGrounded;
    public Transform feetPos;

    // should be 0.14
    public float checkRadius;

    public LayerMask whatIsGround;

    public bool jumping;

    // should be 0.12
    public float holdJumpBtnLength;
    
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

        //isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if(isGrounded && Input.GetKeyDown(KeyCode.P))
        {
            //Debug.Log("ENTER 1");
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumping = true;
            // set jumpTimeLimit to timestamp (like 0.3 into the future or something)
            jumpTimeLimit = Time.time + holdJumpBtnLength;
            jumpTime = Time.time;
        }

        // If they are holding jump btn & we haven't hit the jumpTimeLimit, then continue adding velocity.
        // Otherwise, turn off jumping mode.

        // idea: check before all of this if key is let go (KeyUp) && jumping, then turn off jumping immediately.

        if(jumping && Input.GetKeyUp(KeyCode.P)) // if jumping & user releases jump button.
        {
            jumping = false;
            jumpTime = jumpTimeLimit + 1f;
        }
        else if (jumping && (jumpTime > jumpTimeLimit)) // if player is jumping & has exceeded the jumpTimeLimit
        {
            jumping = false;
        }
        else if (jumping && Input.GetKey(KeyCode.P) && (jumpTime < jumpTimeLimit)) // last part is extra, could remove
        {
            //Debug.Log("jumping && Input.GetKey(KeyCode.P)");
            jumpTime += Time.deltaTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // Make jumping feel waaaayyyyyy better.
        //TODO: look at video this is from and look for comment about optimization nd implement.
        if(!jumping && rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * 1.5f * Time.deltaTime;
        } else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.P))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * 1f * Time.deltaTime;
        }

    }

    void FixedUpdate()
    {
        // Horizontal movement first:
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
        
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

    void onDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(feetPos.position, checkRadius);
        
    }
}
