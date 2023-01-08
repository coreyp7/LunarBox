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

    [SerializeField]
    private int buttonsSelectedIndex;

    [SerializeField]
    private Button createNewLevelBtn;

    [SerializeField]
    private Boolean inTopMenu;

    [SerializeField]
    private NewLevelMenu newLevelMenu;

    private bool inCreateLevelMenu;

    [SerializeField]
    private TMP_InputField inputField;

    private TileList emptyLevel; // for level editor

    // Start is called before the first frame update
    void Start()
    {
        // "Turn off" mouse activity
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inTopMenu = false;
        inCreateLevelMenu = false;

        emptyLevel = gameManager.deserializeLevelFileReturn("Saved_Levels/Default/level_menu_new_level.txt");


        buttonsSelectedIndex = 0;
        buttons = new List<Button>();

        List<TileList> levels = gameManager.deserializeLevelsDirectory("Saved_Levels/");
        foreach(TileList levelInfo in levels)
        {
            //Create ui button with prefab, give it level info (TileList)
            Button newLevelBtn = Instantiate(levelBtnPrefab, this.transform);
            newLevelBtn.GetComponent<LevelButtonBehavior>().setTileList(levelInfo);

            TextMeshProUGUI text = newLevelBtn.GetComponentInChildren<TextMeshProUGUI>();
            text.text = levelInfo.name;
            text.fontSize = 60;

            buttons.Add(newLevelBtn);
            Debug.Log(levelInfo);
        }

        // Always select first button (and load its TileList into the tilemaps)
        // NOTE: THIS ALWAYS THROWS NullReferenceException AND I DON'T KNOW WHY.
        // Functions properly so is alright.
        gameManager.clearCurrentLevel();

        gameManager.loadLevel(levels.First());
        buttons.First().Select();
    }

    // Update is called once per frame
    void Update()
    {
        wDown = Input.GetKey(KeyCode.W);
        aDown = Input.GetKey(KeyCode.A);
        sDown = Input.GetKey(KeyCode.S);
        dDown = Input.GetKey(KeyCode.D);

        // handle inputs navigating levels
        if((wDown || aDown || sDown || dDown) && (!beingHandled) && (!inCreateLevelMenu))
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
        if (buttonsSelectedIndex == 0)
            // TODO: move to top button menu (create new level, exit, etc.)
            return;

        buttonsSelectedIndex -= 1;
        buttons.ElementAt(buttonsSelectedIndex).Select();

        Vector2 size = levelBtnPrefab.GetComponent<RectTransform>().sizeDelta;
        this.transform.position = new Vector2(this.transform.position.x,
            this.transform.position.y - size.y);
    }

    public void moveDown()
    {
        if (buttonsSelectedIndex == buttons.Count-1)
            return;

        buttonsSelectedIndex += 1;
        buttons.ElementAt(buttonsSelectedIndex).Select();

        Vector2 size = levelBtnPrefab.GetComponent<RectTransform>().sizeDelta;
        this.transform.position = new Vector2(this.transform.position.x,
            this.transform.position.y + size.y);
    }

    public void selectTopMenu()
    {
        inTopMenu = true;
        createNewLevelBtn.Select();
        loadLevel(emptyLevel);
    }

    IEnumerator WaitCoroutine()
    {
        beingHandled = true;
        if (wDown && !inTopMenu)
        {
            moveUp();
        }
        else if (sDown)
        {
            if (inTopMenu)
            {
                buttons.ElementAt(buttonsSelectedIndex).Select();
                inTopMenu = false;
            }
            else
            {
                moveDown();
            }
        } else if (dDown)
        {
            selectTopMenu();
        }
        yield return new WaitForSeconds(.1f);
        beingHandled = false;
    }

    public void loadCreateNewLevelMenu()
    {
        newLevelMenu.show();
        inCreateLevelMenu = true;
    }

    /**
     * Called by the InputField in the 'CreateNewLevelMenu'  when user
     * presses enter.
     */
    public void CreateNewLevelSubmit()
    {
        Debug.Log("CreateNewLevelSubmit() called.");
        //Create ui button with prefab, give it level info (TileList)

        /*
         * 1. Deserialize default level file as TileList.
         * 2. Copy to new TileList with the name specified by user.
         * 3. Serialize this new TileList copy with the new name.
         * 4. Repopulate list of levels in this file (look at Start()).
         * 5. Go back to normal LevelList operations.
         */


        if (inputField.text == "")
        {
            Debug.Log("return early");
            return;
        }

        //1
        gameManager.deserializeLevelFile("Default/level_menu_new_level.txt");

        //2: make a copy of the default empty level
        TileList newLevelTileList = GameManager.currentlyLoadedLevel;
        newLevelTileList.name = inputField.text;

        //3: save this TileList copy as its own file, with the user's supplied name
        gameManager.serializeCurrentLevelToFile(newLevelTileList.name);

        //4: This was when I wanted to stay on the level menu after creating.
        // Decided to just send the user to the level editor instead.
        /*
        Button newLevelBtn = Instantiate(levelBtnPrefab, this.transform);
        newLevelBtn.GetComponent<LevelButtonBehavior>().setTileList(newLevelTileList);

        TextMeshProUGUI text = newLevelBtn.GetComponentInChildren<TextMeshProUGUI>();
        text.text = newLevelTileList.name;
        text.fontSize = 60;
        */
        //buttons.Add(newLevelBtn);

        // Hide new level menu
        newLevelMenu.hide();
        inCreateLevelMenu = false;

        // Load new scene
        // (prolly a better idea than just going back to level menu)
        GameManager.openLevelInEditor(newLevelTileList);

    }

    public void test()
    {
        

    }

}
