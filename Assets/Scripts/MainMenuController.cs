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

    public static void LoadGame()
    {
        MusicController.currentMusicChoice = 2;
        SceneManager.LoadScene("Gameplay");
    }

    public static void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public static void LoadLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public static void LoadOnlineLevelSelect()
    {
        SceneManager.LoadScene("OnlineLevelSelect");
    }

    public static void LoadLevelEditor()
    {
        SceneManager.LoadScene("LevelEditor");
    }

    public static void LoadCredits()
    {

        SceneManager.LoadScene("Credits");
    }
}
