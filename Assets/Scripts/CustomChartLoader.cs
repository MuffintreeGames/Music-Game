using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CustomChartLoader : MonoBehaviour
{
    public TMP_InputField importField;

    //public NoteList chart = new NoteList();

    bool errorReceived = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ImportNotechart()
    {
        NoteList newChart = JsonUtility.FromJson<NoteList>(importField.text);
        //chart = newChart;
        if (newChart == null)
        {
            Debug.LogError("Failed to read import data!");
            errorReceived = true;
            return;
        }

        errorReceived = false;
        MusicController.customSongChart = importField.text;
    }

    public bool GetError()
    {
        return errorReceived;
    }
}
