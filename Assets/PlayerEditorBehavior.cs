using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEditorBehavior : MonoBehaviour
{
    private float horizontalInput;

    private float verticalInput;

    private Boolean wDown;

    private Boolean aDown;

    private Boolean sDown;

    private Boolean dDown;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //horizontalInput = Input.GetAxisRaw("Horizontal");
        //verticalInput = Input.GetAxisRaw("Vertical");
        wDown = Input.GetKeyDown(KeyCode.W);
        aDown = Input.GetKeyDown(KeyCode.A);
        sDown = Input.GetKeyDown(KeyCode.S);
        dDown = Input.GetKeyDown(KeyCode.D);

        if (dDown)
        {
            transform.position = new Vector3(transform.position.x + .5f, transform.position.y, 0);
            //StartCoroutine(WaitCoroutine());
        } 
        else if (aDown)
        {
            transform.position = new Vector3(transform.position.x - .5f, transform.position.y, 0);
            //StartCoroutine(WaitCoroutine());
        }

        if(wDown)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + .5f, 0);
            //StartCoroutine(WaitCoroutine());
        }
        else if(sDown)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - .5f, 0);
            //StartCoroutine(WaitCoroutine());
        }

    }

    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(1f);
    }
}
