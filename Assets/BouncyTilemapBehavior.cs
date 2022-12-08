using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BouncyTilemapBehavior : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;

    [SerializeField]
    private float bounciness;

    [SerializeField]
    private float holdingJumpIncrease;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Hit bounce.");
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            //Vector2 thisPosition = new Vector2(this.transform.position.x, this.transform.position.y);
            //Vector2 relativePosition = playerRb.position - thisPosition;
            //Debug.Log(relativePosition);

            //playerRb.velocity = new Vector2(playerRb.velocity.x, Mathf.Abs(playerRb.velocity.y) + bounciness);
            //playerRb.velocity = new Vector2(playerRb.velocity.x, bounciness);

            if (player.isHoldingJump())
            {
                Debug.Log("player is holding p");
                playerRb.velocity = new Vector2(playerRb.velocity.x, bounciness + holdingJumpIncrease);

            } else
            {
                playerRb.velocity = new Vector2(playerRb.velocity.x, bounciness);
            }

            /*
            float newVelocityX = 0f;
            float newVelocityY = 0f;
            if(playerRb.velocity.x > 0)
            {
                newVelocityX += bounciness;
            } else
            {
                newVelocityX -= bounciness;
            }

            if (playerRb.velocity.y > 0)
            {
                newVelocityY += bounciness;
            }
            else
            {
                newVelocityY -= bounciness;
            }
            */

            //playerRb.velocity = new Vector2(-playerRb.velocity.x + bounciness, -playerRb.velocity.y + bounciness);
            //playerRb.velocity = new Vector2(newVelocityX, newVelocityY);
        }
        else
        {
            Debug.Log("Something hit bounce, but not player...");
        }
    }
    
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Hit bounce.");
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            playerRb.velocity = new Vector2(playerRb.velocity.x, bounciness);
            //playerRb.velocity = new Vector2(playerRb.velocity.x, Mathf.Abs(playerRb.velocity.y) + bounciness);
        }
        else
        {
            //Debug.Log("Something hit bounce, but not player...");
        }
    }
    

}
