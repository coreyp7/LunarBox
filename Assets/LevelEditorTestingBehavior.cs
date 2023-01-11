using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorTestingBehavior : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // Clears area (excluding editor tiles before level for consistency)
        // and loads level. Don't use setCurrentLevel() because it erases
        // the entire tilemap.
        gameManager.clearCurrentLevelArea();
        gameManager.loadLevel(gameManager.getCurrentLevel());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
