using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap groundTilemap;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Serialize Box 0,0 - 71,25 into json or something like that.
    public void serializeCurrentLevel()
    {
        BoundsInt box = new BoundsInt(0, 0, 0, 71, 25, 1);

        TileBase[] tiles = groundTilemap.GetTilesBlock(box);
        Debug.Log("tiles.length is: "+tiles.Length);
        
        for(int x = 0; x < 71; x++)
        {
            for(int y = 0; y < 25; y++)
            {
                TileBase tile = tiles[x + y * 71]; // magic ??
                if (tile != null)
                {
                    Debug.Log("Tile (" + x + "," + y + ")");
                    // Serialize Here

                }
            }
        }
    }
}