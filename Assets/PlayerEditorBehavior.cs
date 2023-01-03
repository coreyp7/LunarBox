using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

/**
 * Things left to do in here:
 * 
 * - Clean up this class and optimize it, the its really lazy and ugly right now.
 * 
 * - Implement it so that if player presses it initially it pauses longer than normal, so that its easy
 * for the user to just move by one space. If the player is still holding the direction after the small
 * pause, then move the editor cursor
 * 
 */
public class PlayerEditorBehavior : MonoBehaviour
{
    [SerializeField]
    private UIManager uiManager;
    enum TileType
    {
        Ground,
        Hazard,
        ForceUp,
        ForceDown,
        ForceLeft,
        ForceRight,
        Checkpoint // this one isn't a tile
    }

    private float horizontalInput;

    private float verticalInput;

    private Boolean wDown;

    private Boolean aDown;

    private Boolean sDown;

    private Boolean dDown;

    private Boolean Alpha0Down;
    private Boolean Alpha1Down;
    private Boolean Alpha2Down;
    private Boolean Alpha3Down;
    private Boolean Alpha4Down;
    private Boolean Alpha5Down;
    private Boolean Alpha6Down;

    private Boolean vDown;
    private Boolean cDown;


    private Boolean beingHandled;

    private float initialMoveSpeed = .20f;

    private float holdingMoveSpeed = .05f;

    private KeyCode lastKeyHeld;

    private Boolean placeBtnDown;

    private Boolean deleteBtnDown;

    // tiles

    [SerializeField]
    private TileBase groundBlock;

    [SerializeField]
    private TileBase hazardTile;

    [SerializeField]
    private TileBase forceUpTile;

    [SerializeField]
    private TileBase forceDownTile;

    [SerializeField]
    private TileBase forceLeftTile;

    [SerializeField]
    private TileBase forceRightTile;

    private List<TileType> tileSelectionList;
    private int tileSelectionListCurrentIndex;


    // tilemaps

    [SerializeField]
    private Tilemap groundTilemap;

    [SerializeField]
    private Tilemap hazardTilemap;

    [SerializeField]
    private Tilemap forceUpTilemap;

    [SerializeField]
    private Tilemap forceDownTilemap;

    [SerializeField]
    private Tilemap forceLeftTilemap;

    [SerializeField]
    private Tilemap forceRightTilemap;

    [SerializeField]
    private GameObject checkpointPrefab;

    [SerializeField]
    private GameObject checkpointContainer;

    [SerializeField]
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        beingHandled = false;
        tileSelectionList = new List<TileType>();
        tileSelectionList.Add(TileType.Ground);
        tileSelectionList.Add(TileType.Hazard);
        tileSelectionList.Add(TileType.ForceUp);
        tileSelectionList.Add (TileType.ForceDown);
        tileSelectionList.Add(TileType.ForceLeft);
        tileSelectionList.Add(TileType.ForceRight);
        tileSelectionList.Add(TileType.Checkpoint);
        tileSelectionListCurrentIndex = 0;


    }
    
    private void selectNextTileType()
    {
        tileSelectionListCurrentIndex += 1;
        if (tileSelectionListCurrentIndex >= tileSelectionList.Count)
            tileSelectionListCurrentIndex = 0;

        Debug.Log("New tile type is " + tileSelectionList[tileSelectionListCurrentIndex]);
        uiManager.changeTileType(Enum.GetName(typeof(TileType), tileSelectionList[tileSelectionListCurrentIndex]));
    }

    private void selectPreviousTileType()
    {
        tileSelectionListCurrentIndex -= 1;
        if (tileSelectionListCurrentIndex < 0)
            tileSelectionListCurrentIndex = tileSelectionList.Count-1;

        Debug.Log("New tile type is " + tileSelectionList[tileSelectionListCurrentIndex]);
    }

    void Update()
    {
        //horizontalInput = Input.GetAxisRaw("Horizontal");
        //verticalInput = Input.GetAxisRaw("Vertical");
        wDown = Input.GetKey(KeyCode.W);
        aDown = Input.GetKey(KeyCode.A);
        sDown = Input.GetKey(KeyCode.S);
        dDown = Input.GetKey(KeyCode.D);

        placeBtnDown = Input.GetKey(KeyCode.P);
        deleteBtnDown = Input.GetKey(KeyCode.O);

        Alpha0Down = Input.GetKeyDown(KeyCode.Alpha0);
        Alpha1Down = Input.GetKeyDown(KeyCode.Alpha1);
        Alpha2Down = Input.GetKeyDown(KeyCode.Alpha2);
        Alpha3Down = Input.GetKeyDown(KeyCode.Alpha3);
        Alpha4Down = Input.GetKeyDown(KeyCode.Alpha4);
        Alpha5Down = Input.GetKeyDown(KeyCode.Alpha5);
        Alpha6Down = Input.GetKeyDown(KeyCode.Alpha6);

        cDown = Input.GetKeyDown(KeyCode.C);
        vDown = Input.GetKeyDown(KeyCode.V);

        // Change tile type when user selects new one
        /*
        if (Input.GetKeyDown(KeyCode.N))
            selectNextTileType();
        else if (Input.GetKeyDown(KeyCode.B))
            selectPreviousTileType();
        */
        DetectAlphaNumericKeyDown();

        // Handle player editing inputs (place/delete blocks
        if (placeBtnDown)
        {
            DeleteCurrentTile();
            PlaceCurrentTile();
        } else if (deleteBtnDown)
        {
            DeleteCurrentTile();
        }

        if ((wDown || aDown || sDown || dDown) && (!beingHandled))
        {
            StartCoroutine(WaitCoroutine());
        }

        if (cDown)
        {
            SerializeCurrentLevel();
        }

        if (vDown)
        {
            DeserializeLevelFile("Saved_Levels/level_test.txt");
        }
    }

    private Vector2 position = new Vector2(17.75f, 6.25f);
    private Vector2 size = new Vector2(34.5f, 12.5f);

    // Serialize Box 0,0 - 71,25 into json or something like that.
    private void SerializeCurrentLevel()
    {
        gameManager.serializeCurrentLevel();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawWireCube(position, size);
    }

    private void DeserializeLevelFile(string filepath)
    {
        gameManager.deserializeLevelFile(filepath);
    }

    private void DetectAlphaNumericKeyDown()
    {
        if (Alpha0Down)
        {
            tileSelectionListCurrentIndex = 6;
            uiManager.changeTileType(Enum.GetName(typeof(TileType), tileSelectionList[tileSelectionListCurrentIndex]));
        }
        else if (Alpha1Down)
        {
            tileSelectionListCurrentIndex = 0;
            uiManager.changeTileType(Enum.GetName(typeof(TileType), tileSelectionList[tileSelectionListCurrentIndex]));
        }
        else if (Alpha2Down)
        {
            tileSelectionListCurrentIndex = 1;
            uiManager.changeTileType(Enum.GetName(typeof(TileType), tileSelectionList[tileSelectionListCurrentIndex]));
        }
        else if (Alpha3Down)
        {
            tileSelectionListCurrentIndex = 2;
            uiManager.changeTileType(Enum.GetName(typeof(TileType), tileSelectionList[tileSelectionListCurrentIndex]));
        }
        else if (Alpha4Down)
        {
            tileSelectionListCurrentIndex = 3;
            uiManager.changeTileType(Enum.GetName(typeof(TileType), tileSelectionList[tileSelectionListCurrentIndex]));
        }
        else if (Alpha5Down)
        {
            tileSelectionListCurrentIndex = 4;
            uiManager.changeTileType(Enum.GetName(typeof(TileType), tileSelectionList[tileSelectionListCurrentIndex]));
        }
        else if (Alpha6Down)
        {
            tileSelectionListCurrentIndex = 5;
            uiManager.changeTileType(Enum.GetName(typeof(TileType), tileSelectionList[tileSelectionListCurrentIndex]));
        }
    }

    IEnumerator WaitCoroutine()
    {
        beingHandled = true;
        if (dDown)
        {
            transform.position = new Vector3(transform.position.x + .5f, transform.position.y, 0);
        }
        else if (aDown)
        {
            transform.position = new Vector3(transform.position.x - .5f, transform.position.y, 0);
        }

        if (wDown)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + .5f, 0);
        }
        else if (sDown)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - .5f, 0);
        }
        yield return new WaitForSeconds(holdingMoveSpeed);
        beingHandled = false;
    }

    void PlaceCurrentTile()
    {
        Vector3Int cursorPosition = groundTilemap.WorldToCell(transform.position);

        TileType currentTileType = tileSelectionList[tileSelectionListCurrentIndex];
        //groundTilemap.SetTile(cursorPosition, groundBlock);
        if (currentTileType == TileType.Ground)
        {
            groundTilemap.SetTile(cursorPosition, groundBlock);
        } else if(currentTileType == TileType.Hazard)
        {
            hazardTilemap.SetTile(cursorPosition, hazardTile);
        } else if(currentTileType == TileType.ForceUp)
        {
            forceUpTilemap.SetTile(cursorPosition, forceUpTile);
        } else if(currentTileType== TileType.ForceDown)
        {
            forceDownTilemap.SetTile(cursorPosition, forceDownTile);
        }
        else if (currentTileType == TileType.ForceRight)
        {
            forceRightTilemap.SetTile(cursorPosition, forceRightTile);
        }
        else if (currentTileType == TileType.ForceLeft)
        {
            forceLeftTilemap.SetTile(cursorPosition, forceLeftTile);
        }
        else if (currentTileType == TileType.Checkpoint)
        {
            GameObject newCheckpoint = Instantiate(checkpointPrefab, this.transform.position, Quaternion.identity);
            newCheckpoint.transform.parent = checkpointContainer.transform;
        }

        //Debug.Log("Tile placed @ " + transform.position);
    }

    void DeleteCurrentTile()
    {
        // First check for ground/hazard. If not then go through all the force blocks manually.
        // Really stupid but the only way to do this with the current configuration.
        Vector3Int cursorPosition = groundTilemap.WorldToCell(transform.position);
        Boolean groundTile = groundTilemap.HasTile(cursorPosition);
        if (groundTile)
        {
            groundTilemap.SetTile(cursorPosition, null);
            return;
        }

        cursorPosition = hazardTilemap.WorldToCell(transform.position);
        Boolean hazardTile = hazardTilemap.HasTile(cursorPosition);
        if (hazardTile)
        {
            hazardTilemap.SetTile(cursorPosition, null);
            return;
        }

        cursorPosition = forceUpTilemap.WorldToCell(transform.position);
        Boolean forceUp = forceUpTilemap.HasTile(cursorPosition);
        if (forceUp)
        {
            forceUpTilemap.SetTile(cursorPosition, null);
            return;
        }

        cursorPosition = forceDownTilemap.WorldToCell(transform.position);
        Boolean forceDown = forceDownTilemap.HasTile(cursorPosition);
        if (forceDown)
        {
            forceDownTilemap.SetTile(cursorPosition, null);
            return;
        }

        cursorPosition = forceLeftTilemap.WorldToCell(transform.position);
        Boolean forceLeft = forceLeftTilemap.HasTile(cursorPosition);
        if (forceLeft)
        {
            forceLeftTilemap.SetTile(cursorPosition, null);
            return;
        }

        cursorPosition = forceRightTilemap.WorldToCell(transform.position);
        Boolean forceRight = forceRightTilemap.HasTile(cursorPosition);
        if (forceRight)
        {
            forceRightTilemap.SetTile(cursorPosition, null);
            return;
        }

        // Either it is a checkpoint or it is empty.
        Collider2D[] colliders;
        if((colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(.25f, .25f), 0f)).Length > 1)
        {
            foreach(var collider in colliders)
            {
                if(collider.gameObject.tag == "Checkpoint")
                {
                    Destroy(collider.gameObject);
                }
            }
        }

        /*
        if((colliders = Physics.OverlapBox(transform.position, new Vector3(.5f, .5f))).Length > 1)
        {
            Debug.Log("Entered");
            foreach (var collider in colliders)
            {
                if (collider.gameObject.tag == "Checkpoint")
                {
                    Destroy(collider.gameObject);
                }
            }
        */
    }

    /**
     * Keeping this here in case I want to revert to later.
     * Tried to refine cursor movement but ended up being shitty.
     * 
     * if (dDown)
        {
            if(lastKeyHeld == KeyCode.D)
            {
                transform.position = new Vector3(transform.position.x + .5f, transform.position.y, 0);
                yield return new WaitForSeconds(holdingMoveSpeed);
            } 
            else
            {
                transform.position = new Vector3(transform.position.x + .5f, transform.position.y, 0);
                lastKeyHeld = KeyCode.D;
                yield return new WaitForSeconds(initialMoveSpeed);
            }
        }
        else if (aDown)
        {
            if (lastKeyHeld == KeyCode.A)
            {
                transform.position = new Vector3(transform.position.x - .5f, transform.position.y, 0);
                yield return new WaitForSeconds(holdingMoveSpeed);
            }
            else
            {
                transform.position = new Vector3(transform.position.x - .5f, transform.position.y, 0);
                lastKeyHeld = KeyCode.A;
                yield return new WaitForSeconds(initialMoveSpeed);
            }
        }

        if (wDown)
        {
            if (lastKeyHeld == KeyCode.W)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + .5f, 0);
                yield return new WaitForSeconds(holdingMoveSpeed);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + .5f, 0);
                lastKeyHeld = KeyCode.W;
                yield return new WaitForSeconds(initialMoveSpeed);
            }
        }
        else if (sDown)
        {
            if (lastKeyHeld == KeyCode.S)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - .5f, 0);
                yield return new WaitForSeconds(holdingMoveSpeed);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - .5f, 0);
                lastKeyHeld = KeyCode.S;
                yield return new WaitForSeconds(initialMoveSpeed);
            }
        }
    */

}

