using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LevelButtonBehavior : MonoBehaviour, ISelectHandler //, IPointerClickHandler
{
    private TileList tileList;

    private LevelListBehavior list;


    public void setTileList(TileList tileList)
    {
        this.tileList = tileList;
    }

    // Start is called before the first frame update
    void Start()
    {
        // don't need game manager anymore, but keeping for convenience.
        //GameObject obj = GameObject.FindGameObjectWithTag("GameManager");
        //gameManager = obj.GetComponent<GameManager>();
        //Debug.Log("GameManager:" + gameManager);
        list = this.transform.GetComponentInParent<LevelListBehavior>();
    }

    /**
     * Called when button is SELECTED.
     * That is, when the user is "hovering" over the button, but hasn't
     * "clicked" it yet. 
     * 
     * Loads the tileList corresponding to this button into level preview.
     */
    public void OnSelect(BaseEventData eventData)
    {
        try
        {
            list.loadLevelPreview(this.tileList);
            Debug.Log("speed:"+this.tileList.playerSpeed+", jump:"+this.tileList.playerJumpForce);
        }
        catch (NullReferenceException nre)
        {
            Debug.Log("(LevelButtonBehavior.cs): NullReferenceException thrown, but don't worry ab it.");
        }
    }
    

    /**
     * !!!!!!!!!!Prefab for LevelBtns has this function bound to onClick().!!!!!!!!!!!!!!!!!!!!!
     * (Will say 0 references, but just cause its being called from inspector, stupid)
     */
    public void LoadInEditor()
    {
        // The GameManager already has this button's TileList as the
        // currentlyLoadedLevel if this button is selected.
        // So, this works.
        GameManager.loadLevelEditor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
