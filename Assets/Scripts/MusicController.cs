using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    public static int currentMusicChoice = 0;

    public AudioSource menuMusic;
    public AudioSource song1;
    public string song1Chart;
    public AudioSource song2;
    public string song2Chart;
    public AudioSource song3;
    public string song3Chart;
    public static AudioSource customSongSource;
    public static string customSongChart;

    public static AudioSource currentAudioSource;
    public static string currentSongChart;
    public static float customSongLength = 0f;
    public static bool updatedChart = false;

    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        if (currentAudioSource == null)
        {
            currentAudioSource = menuMusic;
            currentAudioSource.Play();
        }
        if (customSongSource == null)
        {
            customSongSource = GetComponents<AudioSource>()[4]; //don't try this at home
        }
        SceneManager.activeSceneChanged += UpdateMusic;
        UpdateMusic(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());  //call once immediately to set music for current scene
    }

    // Update is called once per frame
    void Update()
    {
        AudioSource check = menuMusic;
        string check2 = null;
        switch (currentMusicChoice)
        {
            case 0: check = menuMusic; check2 = null; break;
            case 1: check = song1; check2 = song1Chart; break;
            case 2: check = song2; check2 = song2Chart; break;
            case 3: check = song3; check2 = song3Chart; break;
            case 4: check = null; check2 = null; break;
            case 5: check = customSongSource; check2 = customSongChart; break;
        }
        if (check != currentAudioSource)
        {
            currentAudioSource.Stop();
            currentAudioSource = check;
            if (currentAudioSource != null && currentAudioSource.clip != null)
            {
                currentAudioSource.time = 0f;
                currentAudioSource.Play();
            }
        }

        if (check2 != null && check2 != currentSongChart)
        {
            currentSongChart = check2;
        }
        updatedChart = true;
    }

    void UpdateMusic(Scene current, Scene next)
    {
        if (currentAudioSource != null)
        {
            if (next.name == "Gameplay" || next.name == "LevelEditor" || next.name == "UploadSongScene" || next.name == "Tutorial")
            {
                currentAudioSource.Stop();
            }
            else if (currentMusicChoice == 0 && currentAudioSource == menuMusic)
            {
                return;
            }
            else
            {
                currentAudioSource.Stop();
            }
        }
        if (next.name == "Gameplay")
        {
            switch (currentMusicChoice)
            {
                case 0: currentAudioSource = menuMusic; currentSongChart = null; break;
                case 1: currentAudioSource = song1; currentSongChart = song1Chart; break;
                case 2: currentAudioSource = song2; currentSongChart = song2Chart; break;
                case 3: currentAudioSource = song3; currentSongChart = song3Chart; break;
                case 5: currentAudioSource = customSongSource; currentSongChart = customSongChart; break;
            }
            // if (currentAudioSource != null) currentAudioSource.Play();
        }
        else if (current.name != "Gameplay" && next.name != "LevelEditor" && next.name != "Tutorial" && next.name != "UploadSongScene")
        {
            currentMusicChoice = 0;
            currentAudioSource = menuMusic;
            if (currentAudioSource != null) currentAudioSource.Play();
        }
        else
        {
            currentAudioSource = null;
            currentMusicChoice = 4;
        }
        updatedChart = false;
    }

    public static void UpdateMusicManual(int newMusicChoice)
    {
        if (currentMusicChoice == newMusicChoice)
        {
            return;
        }
        else
        {
            currentMusicChoice = newMusicChoice;
            updatedChart = false;
            if (currentAudioSource != null)
            {
                currentAudioSource.Stop();
            }
        }
    }

}
