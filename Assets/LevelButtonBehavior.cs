using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelButtonBehavior : MonoBehaviour, ISelectHandler
{
    [SerializeField]
    private TileList tileList;

    [SerializeField]
    private GameManager gameManager;

    public void setTileList(TileList tileList)
    {
        this.tileList = tileList;
    }

    public void OnSelect(BaseEventData eventData)
    {
        // name is deceiving: loads tiles where they should be.
        gameManager.deserializeLevelFile(tileList);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("GameManager");
        gameManager = obj.GetComponent<GameManager>();
        Debug.Log("GameManager:" + gameManager);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
