using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCustomLevelButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MusicController.customSongChart = null;
        if (MusicController.customSongSource != null)
        {
            MusicController.customSongSource.clip = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (MusicController.customSongChart == null || MusicController.customSongSource.clip == null || MusicController.customSongLength == 0f)
        {
            GetComponent<Button>().interactable = false;
        } else
        {
            GetComponent<Button>().interactable = true;
        }
    }
}
