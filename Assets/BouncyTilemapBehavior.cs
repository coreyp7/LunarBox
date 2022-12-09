using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BouncyTilemapBehavior : MonoBehaviour
{
    public enum ForceDirection
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    };

    [SerializeField]
    private ForceDirection forceDirection;

    [SerializeField]
    private PlayerController player;

    [SerializeField]
    private Rigidbody2D playerRb;

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
            applyBounceForceOnPlayerOnTriggerEnter();

        }
        else
        {
            //Debug.Log("Something hit bounce, but not player...");
        }
    }
    
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Debug.Log("Hit bounce.");
            applyBounceForceOnPlayerOnStay();
            //playerRb.velocity = new Vector2(playerRb.velocity.x, Mathf.Abs(playerRb.velocity.y) + bounciness);
        }
        else
        {
            //Debug.Log("Something hit bounce, but not player...");
        }
    }

    private void applyBounceForceOnPlayerOnTriggerEnter()
    {
        switch (forceDirection)
        {
            case ForceDirection.UP:
                /*
                 * If the user is holding jump then increase the height of the bounce/force.
                 */
                if (player.isHoldingJump())
                {
                    playerRb.velocity = new Vector2(playerRb.velocity.x, bounciness + holdingJumpIncrease);
                }
                else
                {
                    playerRb.velocity = new Vector2(playerRb.velocity.x, bounciness);
                }
                break;
            case ForceDirection.DOWN:
                /*
                 * If the user is already falling, then increasing negative y velocity makes it go
                 * uncontrollably fast. So, if they're already falling, then ignore since the player
                 * is already being forced down.
                 */
                if(playerRb.velocity.y < 0) {
                    playerRb.velocity = new Vector2(playerRb.velocity.x, playerRb.velocity.y);
                } else
                {
                    playerRb.velocity = new Vector2(playerRb.velocity.x, -bounciness + playerRb.velocity.y);
                }
                break;
            case ForceDirection.LEFT:
                //Debug.Log("ForceDirection.LEFT");
                //playerRb.velocity = new Vector2(-100f + playerRb.velocity.x, playerRb.velocity.y);
                playerRb.AddForce(new Vector2(-2500f, 0f), ForceMode2D.Force);
                break;
            case ForceDirection.RIGHT:
                playerRb.velocity = new Vector2(bounciness + playerRb.velocity.x, playerRb.velocity.y);
                break;
            default:
                Debug.Log("DEFAULT HIT IN BouncyTilemapBehavior: " + forceDirection);
                break;
        }
    }

    private void applyBounceForceOnPlayerOnStay()
    {
        switch (forceDirection)
        {
            case ForceDirection.UP:
                /*
                 * If the user is holding jump then increase the height of the bounce/force.
                 */
                if (player.isHoldingJump())
                {
                    playerRb.velocity = new Vector2(playerRb.velocity.x, bounciness + holdingJumpIncrease);
                }
                else
                {
                    playerRb.velocity = new Vector2(playerRb.velocity.x, bounciness);
                }
                break;
            case ForceDirection.DOWN:
                /*
                 * If the user is already falling, then increasing negative y velocity makes it go
                 * uncontrollably fast. So, if they're already falling, then ignore since the player
                 * is already being forced down.
                 */
                if (playerRb.velocity.y < 0)
                {
                    playerRb.velocity = new Vector2(playerRb.velocity.x, playerRb.velocity.y);
                }
                else
                {
                    playerRb.velocity = new Vector2(playerRb.velocity.x, -bounciness + playerRb.velocity.y);
                }
                break;
            case ForceDirection.LEFT:
                //Debug.Log("ForceDirection.LEFT");
                //playerRb.velocity = new Vector2(-100f + playerRb.velocity.x, playerRb.velocity.y);
                playerRb.AddForce(new Vector2(-750f, 0f), ForceMode2D.Force);
                break;
            case ForceDirection.RIGHT:
                playerRb.velocity = new Vector2(bounciness + playerRb.velocity.x, playerRb.velocity.y);
                break;
            default:
                Debug.Log("DEFAULT HIT IN BouncyTilemapBehavior: " + forceDirection);
                break;
        }
    }


}
