using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEditorTestingBehavior : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    private Boolean escPressed;

    // Start is called before the first frame update
    void Start()
    {
        // Clears area (excluding editor tiles before level for consistency)
        // and loads level. Don't use setCurrentLevel() because it erases
        // the entire tilemap.
        gameManager.clearLevelArea();
        gameManager.loadLevel(gameManager.getCurrentLevel());
        escPressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        escPressed = Input.GetKey(KeyCode.Escape);

        if (escPressed)
        {
            SceneManager.LoadScene("LevelEditor");
        }
    }
}
