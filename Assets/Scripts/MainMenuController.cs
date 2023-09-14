using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("currently selected object: " + EventSystem.current.currentSelectedGameObject);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void LoadLevelEditor()
    {
        SceneManager.LoadScene("LevelEditor");
    }

    public void LoadCredits()
    {

        SceneManager.LoadScene("Credits");
    }
}
