using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap groundTilemap;

    [SerializeField]
    private TileBase groundTile;

    [SerializeField]
    private Tilemap hazardTilemap;

    [SerializeField]
    private TileBase hazardTile;

    [SerializeField]
    private Tilemap forceUpTilemap;

    [SerializeField]
    private TileBase forceUpTile;

    [SerializeField]
    private Tilemap forceDownTilemap;

    [SerializeField]
    private TileBase forceDownTile;

    [SerializeField]
    private Tilemap forceLeftTilemap;

    [SerializeField]
    private TileBase forceLeftTile;

    [SerializeField]
    private Tilemap forceRightTilemap;

    [SerializeField]
    private TileBase forceRightTile;

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

        // Get arrays for each tilemap, which contain each tile location in the box.
        TileBase[] groundTiles = groundTilemap.GetTilesBlock(box);
        TileBase[] hazardTiles = hazardTilemap.GetTilesBlock(box);
        TileBase[] forceUpTiles = forceUpTilemap.GetTilesBlock(box);
        TileBase[] forceDownTiles = forceDownTilemap.GetTilesBlock(box);
        TileBase[] forceLeftTiles = forceLeftTilemap.GetTilesBlock(box);
        TileBase[] forceRightTiles = forceRightTilemap.GetTilesBlock(box);

        //Debug.Log("tiles.length is: "+ groundTiles.Length);

        TileList tileSerializes = new TileList(); // list of TileSerialized objects
        
        for(int x = 0; x < 71; x++)
        {
            for(int y = 0; y < 25; y++)
            {
                int index = x + y * 71; // magic ??

                TileBase tile = groundTiles[index];
                if (tile != null)
                {
                    tileSerializes.tiles.Add(new TileSerialize(x, y, "ground"));
                    continue;
                }

                tile = hazardTiles[index];
                if(tile != null)
                {
                    tileSerializes.tiles.Add(new TileSerialize(x, y, "hazard"));
                    continue;
                }

                tile = forceUpTiles[index];
                if (tile != null)
                {
                    tileSerializes.tiles.Add(new TileSerialize(x, y, "forceup"));
                    continue;
                }

                tile = forceDownTiles[index];
                if (tile != null)
                {
                    tileSerializes.tiles.Add(new TileSerialize(x, y, "forcedown"));
                    continue;
                }

                tile = forceLeftTiles[index];
                if (tile != null)
                {
                    tileSerializes.tiles.Add(new TileSerialize(x, y, "forceleft"));
                    continue;
                }

                tile = forceRightTiles[index];
                if (tile != null)
                {
                    tileSerializes.tiles.Add(new TileSerialize(x, y, "forceright"));
                    continue;
                }
            }
        }

        /*
        foreach(TileSerialize tile in tileSerializes.tiles)
        {
            //Debug.Log(JsonUtility.ToJson(tile));
            Debug.Log("(" + tile.x + "," + tile.y + "): index=" + tile.x + tile.y * 71);
        }
        */
        string serializedTileJson = JsonUtility.ToJson(tileSerializes);

        Debug.Log(tileSerializes.tiles.Count);
        Debug.Log(serializedTileJson);

        try
        {
            System.IO.File.WriteAllText("Saved_Levels/level_test.txt", serializedTileJson);
        } catch (System.Exception ex)
        {
            Debug.LogError(ex.StackTrace);
        }
    }

    public void deserializeLevelFile(string filepath)
    {
        string json = System.IO.File.ReadAllText(filepath);

        TileList deserializedTileList = JsonUtility.FromJson<TileList>(json);

        foreach(TileSerialize tile in deserializedTileList.tiles)
        {
            Vector3Int tilePosition = new(tile.x, tile.y);

            switch (tile.type)
            {
                case "ground":
                    groundTilemap.SetTile(tilePosition, groundTile);
                    break;
                case "hazard":
                    hazardTilemap.SetTile(tilePosition, hazardTile);
                    break;
                case "forceup":
                    forceUpTilemap.SetTile(tilePosition, forceUpTile);
                    break;
                case "forcedown":
                    forceDownTilemap.SetTile(tilePosition, forceDownTile);
                    break;
                case "forceleft":
                    forceLeftTilemap.SetTile(tilePosition, forceLeftTile);
                    break;
                case "forceright":
                    forceRightTilemap.SetTile(tilePosition, forceRightTile);
                    break;
            }
        }
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
