using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelOptionsMenuBehavior : MonoBehaviour
{
    [SerializeField]
    private Slider speedSlider;

    [SerializeField]
    private Slider jumpSlider;

    [SerializeField]
    private EventSystem eventSystem;

    [SerializeField]
    private TextMeshProUGUI speedText;

    [SerializeField]
    private TextMeshProUGUI jumpHeightText;

    [SerializeField]
    private LevelEditorEscMenu LevelEditorEscMenu;

    private bool escDown;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
        escDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        escDown = Input.GetKeyDown(KeyCode.Escape);
        if (escDown)
        {
            hide();
            LevelEditorEscMenu.show();
        }
    }

    public void show()
    {
        this.gameObject.SetActive(true);
        eventSystem.SetSelectedGameObject(speedSlider.gameObject);
        // set numbers to what they are right now from GameManager.currentlevel
        speedText.SetText(GameManager.currentlyLoadedLevel.playerSpeed.ToString());
        jumpHeightText.SetText(GameManager.currentlyLoadedLevel.playerJumpForce.ToString());
        speedSlider.SetValueWithoutNotify(GameManager.currentlyLoadedLevel.playerSpeed);
        jumpSlider.SetValueWithoutNotify(GameManager.currentlyLoadedLevel.playerJumpForce);
    }

    public void hide()
    {

        this.gameObject.SetActive(false);
    }
    
    public void updateSpeedText()
    {
        speedText.SetText(speedSlider.value.ToString());
        GameManager.currentlyLoadedLevel.playerSpeed = speedSlider.value;
        //TODO: update in gamemanager current level
        // maybe even save to file idk
    }

    public void updateJumpHeightText()
    {
        jumpHeightText.SetText(jumpSlider.value.ToString());
        GameManager.currentlyLoadedLevel.playerJumpForce = jumpSlider.value;
        //TODO: update in gamemanager current level
        // maybe even save to file idk
    }
}
