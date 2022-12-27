using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CheckpointBehavior : MonoBehaviour
{

    [SerializeField]
    private PlayerController player;

    //private Tilemap checkpointTilemap;

    // Start is called before the first frame update
    void Start()
    {
        //checkpointTilemap = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        //if(collision.gameObject == player)
        {
            player.setCheckpoint(this.transform.position);
        }
    }
}
