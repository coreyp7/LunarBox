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
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        
    }

    void FixedUpdate()
    {
        // Horizontal movement first:

        // move left or right depending on input.
        horizontalMovement(this.horizontalInput);
    }

    private void horizontalMovement(float currentInput)
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        //Doing this because without it there's some weirdness with neutral jumping
        //   and this sets the direction, fixing that.
            

        if (currentInput != 0) //on ground handling starts here (alot easier)
        {
            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
        

    }
}
