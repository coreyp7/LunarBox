using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelButtonBehavior : MonoBehaviour, ISelectHandler //, IPointerClickHandler
{
    private TileList tileList;

    private LevelListBehavior list;


    public void setTileList(TileList tileList)
    {
        this.tileList = tileList;
    }

    //TODO: Have this call a method in levellistbehavior instead
    public void OnSelect(BaseEventData eventData)
    {
        // Load this tileList; tileList corresponding to this button.
        list.loadLevel(this.tileList);
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

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    public void OnPointerClick(PointerEventData eventData)
    {
        list.openInEditor(this.tileList);
    }
    */

    public void LoadInEditor()
    {
        list.openInEditor(this.tileList);
        //GameManager.openLevelInEditor(this.tileList);
    }
}
