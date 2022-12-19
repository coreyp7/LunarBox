using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{

    [SerializeField]
    private Image groundTileImage;
    [SerializeField]
    private Image hazardTileImage;
    [SerializeField]
    private Image forceUpTileImage;
    [SerializeField]
    private Image forceDownTileImage;
    [SerializeField]
    private Image forceLeftTileImage;
    [SerializeField]
    private Image forceRightTileImage;

    [SerializeField]
    private Image tilePreviewImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeTileType(string type)
    { 
        switch (type)
        {
            case "Ground":
                tilePreviewImage.sprite = groundTileImage.sprite;
                break;
            case "Hazard":
                tilePreviewImage.sprite = hazardTileImage.sprite;
                break;
            case "ForceUp":
                tilePreviewImage.sprite = forceUpTileImage.sprite;
                break;
            case "ForceDown":
                tilePreviewImage.sprite = (forceDownTileImage).sprite;
                break;
            case "ForceLeft":
                tilePreviewImage.sprite = (forceLeftTileImage).sprite;
                break;
            case "ForceRight":
                tilePreviewImage.sprite = forceRightTileImage.sprite;
                break;
        }

        if (type == "Hazard")
        {
            tilePreviewImage.sprite = hazardTileImage.sprite;
        }
    }
}
