using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NameHolder : MonoBehaviour
{
    public string username = "Anonymous";
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConfirmUsername()
    {
        GameObject inputField = GameObject.Find("UsernameInput");
        if (inputField == null)
        {
            Debug.LogError("no input field!");
            return;
        }
        string inputText = inputField.GetComponent<TMP_InputField>().text;
        if (inputText == "")
        {
            Debug.LogError("empty input");
            return;
        }
        username = inputText;
        SceneManager.LoadScene("MainMenu");
    }
}
