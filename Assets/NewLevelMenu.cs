using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class NewLevelMenu : MonoBehaviour
{
    
    private TMP_InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
        this.inputField = GetComponentInChildren<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void show()
    {
        this.gameObject.SetActive(true);
        inputField.Select();
    }

    public void hide()
    {
        this.gameObject.SetActive(false);
    }

    private void OnApplicationFocus(bool focus)
    {
        inputField.ActivateInputField();
    }
}
