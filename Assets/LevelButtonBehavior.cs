using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelButtonBehavior : MonoBehaviour, ISelectHandler
{
    private TileList tileList;

    [SerializeField]
    private GameManager gameManager;

    public void setTileList(TileList tileList)
    {
        this.tileList = tileList;
    }

    //TODO: Have this call a method in levellistbehavior instead
    public void OnSelect(BaseEventData eventData)
    {
        // Load this tileList; tileList corresponding to this button.
        this.transform.GetComponentInParent<LevelListBehavior>().loadLevel(this.tileList);
    }

    // Start is called before the first frame update
    void Start()
    {
        // don't need game manager anymore, but keeping for convenience.

        //GameObject obj = GameObject.FindGameObjectWithTag("GameManager");
        //gameManager = obj.GetComponent<GameManager>();
        //Debug.Log("GameManager:" + gameManager);

    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
