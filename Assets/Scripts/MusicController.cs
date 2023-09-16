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
        }
        DontDestroyOnLoad(gameObject);
        SceneManager.activeSceneChanged += UpdateMusic;
        UpdateMusic(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());  //call once immediately to set music for current scene
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateMusic(Scene current, Scene next)
    {
        if (currentAudioSource != null)
        {
            if (next.name == "Gameplay" || next.name == "LevelEditor")
            {
                currentAudioSource.Stop();
                return;
            }
            if (currentMusicChoice == 0)
            {
                return;
            } else
            {
                currentAudioSource.Stop();
            }
        }

        currentAudioSource = menuMusic;
        if (currentAudioSource != null) currentAudioSource.Play();
    }

    void UpdateMusicManual(int newMusicChoice)
    {
        if (currentAudioSource != null)
        {
            if (currentMusicChoice == newMusicChoice)
            {
                return;
            }
            else
            {
                currentAudioSource.Stop();
            }
        }

        switch (newMusicChoice)
        {
            case 0: currentAudioSource = menuMusic; break;
            case 1: currentAudioSource = song1; break;
            case 2: currentAudioSource = song2; break;
            case 3: currentAudioSource = song3; break;
        }
        currentMusicChoice = newMusicChoice;
        if (currentAudioSource != null) currentAudioSource.Play();
    }
}
