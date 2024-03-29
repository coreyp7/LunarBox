using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEditorEscMenu : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private EventSystem eventSystem;

    [SerializeField]
    private Button saveAndQuit;

    [SerializeField]
    private Button testLevelBtn;

    [SerializeField]
    private PlayerEditorBehavior playerEditor;

    [SerializeField]
    private LevelOptionsMenuBehavior levelOptions;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveAndQuit()
    {
        //gameManager.serializeCurrentLevel();
        gameManager.serializeCurrentLevelToFile(GameManager.currentlyLoadedLevel.name);
        

        SceneManager.LoadScene("MainMenu");
    }

    public void show()
    {
        this.gameObject.SetActive(true);
        eventSystem.SetSelectedGameObject(testLevelBtn.gameObject);
        playerEditor.setPlayerControl(false);
    }

    public void hide()
    {
        this.gameObject.SetActive(false);
        playerEditor.setPlayerControl(true);
    }

    public void TestLevel()
    {
        // current changes aren't made yet.
        TileList updatedTiles = gameManager.convertTilemapsToTileList();
        updatedTiles.playerJumpForce = GameManager.currentlyLoadedLevel.playerJumpForce;
        updatedTiles.playerSpeed = GameManager.currentlyLoadedLevel.playerSpeed;
        updatedTiles.name = GameManager.currentlyLoadedLevel.name;

        gameManager.setCurrentLevel(updatedTiles);

        SceneManager.LoadScene("LevelEditorTesting");
    }

    public void LevelOptionsMenu()
    {
        levelOptions.show();
    }
}
