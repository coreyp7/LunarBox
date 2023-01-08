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

    // Start is called before the first frame update
    void Start()
    {
        // "Turn off" mouse activity
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

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
        }

        // Always select first button (and load its TileList into the tilemaps)
        buttons.First().Select();
        gameManager.loadLevel(levels.First());
    }

    // Update is called once per frame
    void Update()
    {
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
        if (buttonsSelectedIndex == 0)
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
