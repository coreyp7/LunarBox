using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

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
    private float horizontalInput;

    private float verticalInput;

    private Boolean wDown;

    private Boolean aDown;

    private Boolean sDown;

    private Boolean dDown;

    private Boolean beingHandled;

    private float initialMoveSpeed = .20f;

    private float holdingMoveSpeed = .05f;

    private KeyCode lastKeyHeld;

    private Boolean placeBtnDown;

    private Boolean deleteBtnDown;

    [SerializeField]
    private TileBase groundBlock;

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

    // Start is called before the first frame update
    void Start()
    {
        beingHandled = false;
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

        if (placeBtnDown)
        {
            DeleteCurrentTile();
            PlaceCurrentTile();
        }

        if(deleteBtnDown)
            DeleteCurrentTile();


        if ((wDown || aDown || sDown || dDown) && (!beingHandled))
        {
            StartCoroutine(WaitCoroutine());
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

        groundTilemap.SetTile(cursorPosition, groundBlock);

        Debug.Log("Tile placed @ " + transform.position);
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
        }

        cursorPosition = forceDownTilemap.WorldToCell(transform.position);
        Boolean forceDown = forceDownTilemap.HasTile(cursorPosition);
        if (forceDown)
        {
            forceDownTilemap.SetTile(cursorPosition, null);
        }

        cursorPosition = forceLeftTilemap.WorldToCell(transform.position);
        Boolean forceLeft = forceLeftTilemap.HasTile(cursorPosition);
        if (forceLeft)
        {
            forceLeftTilemap.SetTile(cursorPosition, null);
        }

        cursorPosition = forceRightTilemap.WorldToCell(transform.position);
        Boolean forceRight = forceRightTilemap.HasTile(cursorPosition);
        if (forceRight)
        {
            forceRightTilemap.SetTile(cursorPosition, null);
        }
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

