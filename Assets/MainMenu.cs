using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UI.Button;
using Cursor = UnityEngine.Cursor;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Button defaultBtn;

    [SerializeField]
    private EventSystem eventSystem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (eventSystem.currentSelectedGameObject == null)
        {
            eventSystem.SetSelectedGameObject(defaultBtn.gameObject);
        }
    }

    public void levelMenu()
    {
        SceneManager.LoadScene("LevelMenu");
    }

    public void loadEndlessScene()
    {
        SceneManager.LoadScene("EndlessMode");
    }

    /*
    private void OnApplicationFocus(bool focus)
    {
        if(focus)
        {
            eventSystem.SetSelectedGameObject(defaultBtn.gameObject);
            Debug.Log("set selected game object");
        }
    }
    */
}
