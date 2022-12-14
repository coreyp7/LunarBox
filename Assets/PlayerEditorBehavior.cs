using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

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


    // Start is called before the first frame update
    void Start()
    {
        beingHandled = false;
        lastKeyHeld = KeyCode.None;
    }

    void Update()
    {
        //horizontalInput = Input.GetAxisRaw("Horizontal");
        //verticalInput = Input.GetAxisRaw("Vertical");
        wDown = Input.GetKey(KeyCode.W);
        aDown = Input.GetKey(KeyCode.A);
        sDown = Input.GetKey(KeyCode.S);
        dDown = Input.GetKey(KeyCode.D);
        


        if ((wDown || aDown || sDown || dDown) && (!beingHandled))
        {
            StartCoroutine(WaitCoroutine());
        } else if (!beingHandled)
        {
            lastKeyHeld = KeyCode.None;
        }

    }

    IEnumerator WaitCoroutine()
    {
        beingHandled = true;


        if (dDown)
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
        beingHandled = false;
    }
}
