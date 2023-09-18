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
        //Debug.Log("currently selected object: " + EventSystem.current.currentSelectedGameObject);
    }

    public static void LoadGame()
    {
        // load online music generically?
        SceneManager.LoadScene("Gameplay");
    }

    public static void LoadSong1()
    {
        MusicController.currentMusicChoice = 1;
        Leaderboard.selectLeaderboard(1);
        SceneManager.LoadScene("Gameplay");
    }

    public static void LoadSong2()
    {
        MusicController.currentMusicChoice = 2;
        Leaderboard.selectLeaderboard(2);
        SceneManager.LoadScene("Gameplay");
    }

    public static void LoadSong3()
    {
        MusicController.currentMusicChoice = 3;
        Leaderboard.selectLeaderboard(3);
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

    public static void LoadSongUpload()
    {
        SceneManager.LoadScene("SongUploadScene");
    }

    public static void LoadTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
