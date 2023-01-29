using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CheckpointBehavior : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;

    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private bool endOfLevel = false;

    //private Tilemap checkpointTilemap;

    // Start is called before the first frame update
    void Start()
    {
        //checkpointTilemap = GetComponent<Tilemap>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setEndOfLevel(bool val)
    {
        endOfLevel = val;
    }

    public void setJumpHeight(float val)
    {
        jumpHeight = val;
    }

    public void setMoveSpeed(float val)
    {
        moveSpeed = val;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.gameObject.tag == "Player")
        {
            if (endOfLevel)
            {
                // TODO: go back to main menu, show that they won somehow.
                Debug.Log("ENDLESS FINISHED");
            } 
            else
            {
                player.setCheckpoint(this.transform.position);
                player.setPlayerJumpForce(jumpHeight);
                player.setPlayerSpeed(moveSpeed);
            }
        }
    }
}
