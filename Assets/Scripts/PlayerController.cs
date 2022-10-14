using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidBody;

    [SerializeField]
    private float moveSpeed;

    private void Awake()
    {
        rigidBody = transform.GetComponent<Rigidbody2D>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        float currentInputHorizontal = Input.GetAxis("Horizontal");
        float currentInputVertical = Input.GetAxis("Vertical");
        Debug.Log("H:" + currentInputHorizontal + ", V:" + currentInputVertical);

        // move left or right depending on input.
        if(currentInputHorizontal > 0)
        {
            //rigidBody.velocity = new Vector2(currentInputHorizontal * moveSpeed, rigidBody.velocity.y);
            rigidBody.velocity = new Vector2(moveSpeed, rigidBody.velocity.y);
        } else if (currentInputHorizontal < 0)
        {
            //rigidBody.velocity = new Vector2(currentInputHorizontal * moveSpeed, rigidBody.velocity.y);
            rigidBody.velocity = new Vector2(-moveSpeed, rigidBody.velocity.y);

        } else
        {
            rigidBody.velocity = new Vector2(0f, rigidBody.velocity.y);
        }
    }
}
