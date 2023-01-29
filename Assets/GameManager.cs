using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private string FILENAME;

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

    [SerializeField]
    private GameObject checkpointContainer;

    [SerializeField]
    private GameObject checkpointPrefab;

    [SerializeField]
    public static TileList currentlyLoadedLevel;

    [SerializeField]
    private PlayerController playerController;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        
    }
    
    public void serializeCurrentLevel()
    {
        if(currentlyLoadedLevel != null)
            serializeCurrentLevelToFile(currentlyLoadedLevel.name);
        else
            serializeCurrentLevelToFile(FILENAME); // development only
    }

    public void loadMainMenuScene()
    {
        //SceneManager.LoadScene("LevelEditor");
    }

    /// <summary>
    /// Serializes currently loaded level into a file (filename specified by levelName in param).
    /// </summary>
    /// <param name="levelName"> The name of the level. </param>
    public void serializeCurrentLevelToFile(string levelName)
    {
        TileList levelTileList = convertTilemapsToTileList();
        levelTileList.name = levelName;
        levelTileList.playerSpeed = GameManager.currentlyLoadedLevel.playerSpeed;
        levelTileList.playerJumpForce = GameManager.currentlyLoadedLevel.playerJumpForce;

        string serializedTileJson = JsonUtility.ToJson(levelTileList);

        Debug.Log("Tiles in serializedtileJson:" + levelTileList.tiles.Count);
        Debug.Log(serializedTileJson);

        try
        {
            //System.IO.File.WriteAllText("Saved_Levels/level_test.txt", serializedTileJson);
            System.IO.File.WriteAllText("Saved_Levels/" + levelName + ".txt", serializedTileJson);
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.StackTrace);
        }
    }

    /// <summary>
    /// Will create a TileList object of whatever is in the level when it is called.
    /// NOTE: Does not give TileList a name. Uses default values for player props.
    /// </summary>
    /// <returns>TileList representation of current tilemaps' contents.</returns>
    public TileList convertTilemapsToTileList()
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

        for (int x = 0; x < 71; x++)
        {
            for (int y = 0; y < 25; y++)
            {
                int index = x + y * 71; // magic ??

                TileBase tile = groundTiles[index];
                if (tile != null)
                {
                    tileSerializes.tiles.Add(new TileSerialize(x, y, "ground"));
                    continue;
                }

                tile = hazardTiles[index];
                if (tile != null)
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
        return tileSerializes;
    }

    /// <summary>
    /// Will load the level file (identified by the levelName)
    /// as the currently loaded level.
    /// </summary>
    /// <param name="levelName"></param>
    public void deserializeLevelFile(string levelName)
    {
        string json = System.IO.File.ReadAllText("Saved_Levels/"+ levelName);
        TileList deserializedTileList = JsonUtility.FromJson<TileList>(json);

        loadLevel(deserializedTileList);
    }

    /// <summary>
    /// Will load a level from the given tileList param.
    /// </summary>
    /// <param name="tileList"></param>
    public void loadLevel(TileList tileList)
    {
        currentlyLoadedLevel = tileList;

        foreach (TileSerialize tile in tileList.tiles)
        {
            // Sketchy casting from float to int.
            // Originally TileSerialize had integer cords (since tiles
            // are always going to be ints), but changed to floats so
            // that we can use them with checkpoints (which do not use
            // the position of the tilemaps, uses regular global pos.
            Vector3Int tilePosition = new((int)tile.x, (int)tile.y);

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
                    /* Checkpoints are dead.
                case "checkpoint":
                    GameObject newCheckpoint = Instantiate(checkpointPrefab,
                        new Vector2(tile.x, tile.y),
                        Quaternion.identity);
                    newCheckpoint.transform.parent = checkpointContainer.transform;
                    break;
                    */
            }
        }
    }

    /**
     * Will put the given tileList into the given position 
     * (just the order of the level, 1,2,3,4,....)
     */
    public void putLevel(TileList tileList, int pos)
    {
        foreach (TileSerialize tile in tileList.tiles)
        {
            // Sketchy casting from float to int.
            // Originally TileSerialize had integer cords (since tiles
            // are always going to be ints), but changed to floats so
            // that we can use them with checkpoints (which do not use
            // the position of the tilemaps, uses regular global pos.
            BoundsInt box = new BoundsInt(0, 0, 0, 71, 25, 1);
            Vector3Int tilePosition;

            if (pos > 0)
            {
                int scale = pos * 71;
                Debug.Log(scale);
                tilePosition = new(scale + (int)tile.x, (int)tile.y);
            } else
            {
                tilePosition = new((int)tile.x, (int)tile.y);
            }

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

    /// <summary>
    /// Will return a list of TileList objects, each of which corresponds to
    /// each level file.
    /// </summary>
    /// <param name="dirPath"> The path to the directory (local unity project).</param>
    /// <returns></returns>
    public List<TileList> deserializeLevelsDirectory(string dirPath)
    {
        string[] levelFiles = Directory.GetFiles(dirPath);
        List<TileList> levels = new List<TileList>();

        foreach(string levelFile in levelFiles)
        {
            string currJson = System.IO.File.ReadAllText(levelFile);
            TileList deserializedTileList = JsonUtility.FromJson<TileList>(currJson);
            levels.Add(deserializedTileList);
        }
        
        return levels;
    }

    /// <summary>
    /// Will return a TileList of the file (specified by filepath).
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public TileList deserializeLevelFileReturn(string filepath)
    {
        string json = System.IO.File.ReadAllText(filepath);
        TileList tileList = JsonUtility.FromJson<TileList>(json);
        return tileList;
    }

    /// <summary>
    /// Will set this TileList as the currently loaded level, 
    /// clear the tilemaps of the previous level, and populate
    /// the tilemaps with the level.
    /// </summary>
    /// <param name="tileList"></param>
    public void setCurrentLevel(TileList tileList)
    {
        currentlyLoadedLevel = tileList;
        clearAllTilemaps();
        loadLevel(tileList);
    }

    public TileList getCurrentLevel()
    {
        return currentlyLoadedLevel;
    }

    public static void loadLevelEditor()
    {
        SceneManager.LoadScene("LevelEditor");
    }



    // Private functions. //

    private void clearAllTilemaps()
    {
        groundTilemap.ClearAllTiles();
        hazardTilemap.ClearAllTiles();
        forceUpTilemap.ClearAllTiles();
        forceDownTilemap.ClearAllTiles();
        forceLeftTilemap.ClearAllTiles();
        forceRightTilemap.ClearAllTiles();
    }

    /* 
     * For the LevelEditorTestingBehavior to use to clear the level's tilemap
     * without erasing the ENTIRE tilemap. 
     * (If you want to keep blocks outside the editable area)
     */
    public void clearLevelArea()
    {
        BoundsInt box = new BoundsInt(0, 0, 0, 71, 25, 1);
        TileBase[] nulls = new TileBase[71 * 25];
        groundTilemap.SetTilesBlock(box,nulls);
        hazardTilemap.SetTilesBlock(box,nulls);
        forceUpTilemap.SetTilesBlock(box,nulls);
        forceDownTilemap.SetTilesBlock(box, nulls);
        forceLeftTilemap.SetTilesBlock(box, nulls);
        forceRightTilemap.SetTilesBlock(box, nulls);
    }

    public void loadImageToSprite(string filepath)
    {
        
        Texture2D tex = null;
        byte[] data = null;


        if (File.Exists(filepath))
        {
            data = File.ReadAllBytes(filepath);
            tex = new Texture2D(200, 200);
            tex.LoadImage(data);

            // Turn into a tile (Texture2D -> Sprite -> into Tile).
            Tile newTile = new Tile();
            Sprite newSprite = Sprite.Create(tex, new Rect(), new Vector2(0, 0), 200);
            newTile.sprite = newSprite;

            // TODO:
            /*
             * Return the newTile from this function.
             * 
             * 1. Create a function with a parameter that specifies
             * the tile type to load to (in addition to its filepath).
             *      - call this function
             *      - set the tile to its respective variable (groundTile,
             *      hazardTile, etc.).
             * 
             * 2. Change loading/saving functions to include the sprites
             * of the level into the level JSON.
             *      - save a unique identifier for that tile
             *      - load tile from a player_tilemap directory and determine
             *      the correct one to load by the unique specifier.
             * 
             */
        }
        
    }
}

/*
 * Keeping original version to keep checkpoint logic around if I need to use it later.
 * 
 * // Serialize Box 0,0 - 71,25 into json or something like that.
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

        if(this.currentlyLoadedLevel != null) // DEVELOPMENT ONLY
        {
            if (this.currentlyLoadedLevel.name != "")
            {
                tileSerializes.name = this.currentlyLoadedLevel.name;
            }
            else
            {
                tileSerializes.name = "example name";
            }
        } else
        {
            tileSerializes.name = this.FILENAME;
        }

        for (int x = 0; x < 71; x++)
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

        // Get all checkpoints in the level area.
        // Convert them to TileSerialize objects and put into list.
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
                    collider.transform.position.x,
                    collider.transform.position.y,
                    "checkpoint"));
                //checkpoints.Add(collider.GameObject());
            }
        }
        

        string serializedTileJson = JsonUtility.ToJson(tileSerializes);

        Debug.Log("Tiles in serializedtileJson:" + tileSerializes.tiles.Count);
        Debug.Log(serializedTileJson);

        try
        {
            //System.IO.File.WriteAllText("Saved_Levels/level_test.txt", serializedTileJson);
            System.IO.File.WriteAllText("Saved_Levels/" + tileSerializes.name + ".txt", serializedTileJson);
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.StackTrace);
        }
            }
 * 
 */
