using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

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

        Vector2 position = new Vector2(17.75f, 6.25f);
        Vector2 size = new Vector2(34.5f, 12.5f);

        Collider2D[] colliders = Physics2D.OverlapBoxAll(position, size, 0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.tag == "Checkpoint")
            {
                Debug.Log("collider pos changed: ("+collider.transform.position+")" +
                    "to ("+ (int)collider.transform.position.x+","+ 
                    (int)collider.transform.position.y+")");

                tileSerializes.tiles.Add(new TileSerialize(
                    (int)collider.transform.position.x,
                    (int)collider.transform.position.y,
                    "checkpoint"));
                //checkpoints.Add(collider.GameObject());
            }
        }

        // TODO: get all checkpoints in the level.
        // Convert them to TileSerialize objects and put into list.

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
            // Sketchy casting conversion.
            // Originally TileSerialize had integer cords (since tiles
            // are always going to be ints), but changed to floats so
            // that we can use them with checkpoints
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
