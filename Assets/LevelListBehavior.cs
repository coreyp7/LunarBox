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
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Cursor = UnityEngine.Cursor;

public class LevelListBehavior : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private Button levelBtnPrefab;

    [SerializeField]
    private EventSystem eventSystem;

    private Boolean wDown;

    private Boolean aDown;

    private Boolean sDown;

    private Boolean dDown;

    private Boolean beingHandled;

    private Button firstTileListBtn;

    private List<Button> buttons;

    private int buttonsSelectedIndex;


    //[SerializeField]
    //public ScrollRect scrollView;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        buttonsSelectedIndex = 0;
        buttons = new List<Button>();

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

        //scrollView = this.transform.GetComponentInParent<ScrollRect>();
        //scrollView.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;

        List<TileList> levels = gameManager.deserializeLevelsDirectory("Saved_Levels/");
        //List<Button> buttons = new List<Button>();

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

        buttons.First().Select();

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
        //if(EventSystem.current.currentSelectedGameObject == null)
        //{
        //    EventSystem.current.SetSelectedGameObject(firstTileListBtn.gameObject);
        //    Debug.Log("cowabunga 1");
        //}

        wDown = Input.GetKey(KeyCode.W);
        aDown = Input.GetKey(KeyCode.A);
        sDown = Input.GetKey(KeyCode.S);
        dDown = Input.GetKey(KeyCode.D);

        // handle inputs navigating levels
        if((wDown || aDown || sDown || dDown) && (!beingHandled))
            StartCoroutine(WaitCoroutine());
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
        GameManager.openLevelInEditor(tileList);
    }

    public void scrollToButton(VisualElement btn)
    {
        //scrollView.ScrollTo(btn);
        this.transform.GetComponentInParent<ScrollView>().ScrollTo(btn);
    }

    public void moveUp()
    {
        Vector2 size = levelBtnPrefab.GetComponent<RectTransform>().sizeDelta;
        this.transform.position = new Vector2(this.transform.position.x,
            this.transform.position.y - size.y);
        Debug.Log("LevelList y increased by " + size.y);

        //buttons.ElementAt(buttonsSelectedIndex).OnDeselect();
        buttonsSelectedIndex -= 1;
        buttons.ElementAt(buttonsSelectedIndex).Select();
    }

    public void moveDown()
    {
        Vector2 size = levelBtnPrefab.GetComponent<RectTransform>().sizeDelta;
        this.transform.position = new Vector2(this.transform.position.x,
            this.transform.position.y + size.y);
        Debug.Log("LevelList y increased by " + size.y);

        //buttons.ElementAt(buttonsSelectedIndex).OnDeselect();
        buttonsSelectedIndex += 1;
        buttons.ElementAt(buttonsSelectedIndex).Select();
        
    }

    IEnumerator WaitCoroutine()
    {
        beingHandled = true;
        if (wDown)
        {
            moveUp();
        }
        else if (sDown)
        {
            moveDown();
        }
        yield return new WaitForSeconds(.1f);
        beingHandled = false;
    }



}
