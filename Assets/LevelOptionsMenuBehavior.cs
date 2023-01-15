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

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void show()
    {
        this.gameObject.SetActive(true);
        eventSystem.SetSelectedGameObject(speedSlider.gameObject);

    }

    public void hide()
    {
        this.gameObject.SetActive(false);
    }
    
    public void updateSpeedText()
    {
        speedText.SetText(speedSlider.value.ToString());
    }

    public void updateJumpHeightText()
    {
        jumpHeightText.SetText(jumpSlider.value.ToString());
    }
}
