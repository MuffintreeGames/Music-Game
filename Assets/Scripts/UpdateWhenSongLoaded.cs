using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateWhenSongLoaded : MonoBehaviour
{
    public SongHolder songHolder;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (songHolder == null) { return; }
        if (songHolder.GetSongLoaded() )
        {
            GetComponent<TextMeshProUGUI>().text = "Loaded!";
        } else
        {
            GetComponent<TextMeshProUGUI>().text = "Not Loaded";
        }
    }
}
