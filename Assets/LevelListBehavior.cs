using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelListBehavior : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private Button levelBtnPrefab;

    // Start is called before the first frame update
    void Start()
    {
        /*
         * 1. Call method deserializeLevelsDirectory(dirPath) in gameManager, which
         * will return a List<TileList> objects of the files in that dir.
         * 2. Loop through that list and create a List of UI buttons. These will
         * contain information of that level (date created, etc) but for now
         * will have no info, just load level (maybe add name or something like that).
         *          - These buttons should load the tilemap with that levels tiles
         *              on selection, there should be a method for that???
         * 3. Place all of these buttons manually inside the level list empty object.
         * 4. When a user presses up/down, the next/prev btn should be selected, and
         * the LevelList object should be shifted by a hardcoded amount 
         * (height of the button?)
         * 
         */

        List<TileList> levels = gameManager.deserializeLevelsDirectory("Saved_Levels/");
        Debug.Log(levels);

        foreach(TileList levelInfo in levels)
        {
            //Create ui button with prefab, give it level info


            //set its transform to hardcoded location
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
