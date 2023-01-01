using System;
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

        TileList tileSerializes = new TileList();
        
        for(int x = 0; x < 71; x++)
        {
            for(int y = 0; y < 25; y++)
            {
                TileBase tile = tiles[x + y * 71]; // magic ??
                if (tile != null)
                {
                    //Debug.Log("Tile (" + x + "," + y + ")");
                    // Serialize Here
                    //Debug.Log(JsonUtility.ToJson(new TileSerialize(x,y,"ground")));
                    tileSerializes.tiles.Add(new TileSerialize(x, y, "ground"));
                }
            }
        }

        foreach(TileSerialize tile in tileSerializes.tiles)
        {
            Debug.Log(JsonUtility.ToJson(tile));
        }

        //TileSerialize[] arr = tileSerializes.tiles.ToArray();
        //string listToJson = JsonHelper.ToJson(arr);
        Debug.Log(tileSerializes.tiles.Count);
        //Debug.Log(arr.Length);
        //Debug.Log(JsonUtility.ToJson(arr));
        Debug.Log(JsonUtility.ToJson(tileSerializes, true));
        //Debug.Log(JsonUtility.ToJson(tileSerializes.ToArray()));
    }

    [Serializable]
    public class TileSerialize
    {
        public TileSerialize(int x, int y, string type)
        {
            this.x = x;
            this.y = y;
            this.type = type;
        }

        public int x;
        public int y;
        public string type;

    }

    [Serializable]
    public class TileList
    {
        public List<TileSerialize> tiles;

        public TileList()
        {
            this.tiles = new List<TileSerialize>();
        }
    }
}
