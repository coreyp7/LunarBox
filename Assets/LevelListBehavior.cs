using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelListBehavior : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private Button levelBtnPrefab;

    [SerializeField]
    private EventSystem eventSystem;

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
        List<Button> buttons = new List<Button>();

        foreach(TileList levelInfo in levels)
        {
            //Create ui button with prefab, give it level info
            Button newLevelBtn = Instantiate(levelBtnPrefab, this.transform);
            newLevelBtn.GetComponent<LevelButtonBehavior>().setTileList(levelInfo);

            TextMeshProUGUI text = newLevelBtn.GetComponentInChildren<TextMeshProUGUI>();
            text.text = levelInfo.name;
            text.fontSize = 60;

            buttons.Add(newLevelBtn);
        }
        try
        {
            eventSystem.SetSelectedGameObject(buttons.First().GameObject()); // ????????????
        } catch (NullReferenceException nre)
        {
            Debug.LogException(nre);
        }
        gameManager.loadLevel(levels.First());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**
     * Clear the current level tilemaps and load new level
     * from TileList object.
     */
    public void loadLevel(TileList tileList)
    {
        gameManager.clearCurrentLevel();
        gameManager.loadLevel(tileList);
    }

    public void openInEditor(TileList tileList)
    {
        gameManager.setCurrentLevel(tileList);
        SceneManager.LoadScene("LevelEditor");
    }

}