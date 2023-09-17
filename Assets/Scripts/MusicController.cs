using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    public static int currentMusicChoice = 0; 

    public AudioSource menuMusic;
    public AudioSource song1;
    public AudioSource song2;
    public AudioSource song3;

    public static AudioSource currentAudioSource;

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
        SceneManager.activeSceneChanged += UpdateMusic;
        UpdateMusic(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());  //call once immediately to set music for current scene
    }

    // Update is called once per frame
    void Update()
    {
        AudioSource check = menuMusic;
        switch (currentMusicChoice)
        {
            case 0: check = menuMusic; break;
            case 1: check = song1; break;
            case 2: check = song2; break;
            case 3: check = song3; break;
        }
        if (check == currentAudioSource) return;
        else {
            currentAudioSource.Stop();
            currentAudioSource = check;
            if (currentAudioSource != null) currentAudioSource.Play();
        }
    }

    void UpdateMusic(Scene current, Scene next)
    {
        if (currentAudioSource != null)
        {
            if (next.name == "Gameplay" || next.name == "LevelEditor")
            {
                currentAudioSource.Stop();
            }
            else if (currentMusicChoice == 0 && currentAudioSource == menuMusic)
            {
                return;
            } else
            {
                currentAudioSource.Stop();
            }
        }
        if (next.name == "Gameplay")
        {
            switch (currentMusicChoice)
            {
                case 0: currentAudioSource = menuMusic; break;
                case 1: currentAudioSource = song1; break;
                case 2: currentAudioSource = song2; break;
                case 3: currentAudioSource = song3; break;
            }
            // if (currentAudioSource != null) currentAudioSource.Play();
        }
            else if (current.name != "Gameplay" && current.name != "LevelEditor")
        {
            currentMusicChoice = 0;
            currentAudioSource = menuMusic;
            if (currentAudioSource != null) currentAudioSource.Play();
        }
    }

    public static void UpdateMusicManual(int newMusicChoice)
    {
        if (currentAudioSource != null)
        {
            if (currentMusicChoice == newMusicChoice)
            {
                return;
            }
            else
            {
                currentMusicChoice = newMusicChoice;
                currentAudioSource.Stop();
            }
        }
    }
}
