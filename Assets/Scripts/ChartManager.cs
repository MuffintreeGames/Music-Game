using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Note
{
    public float time;
    public bool blast;
    public int column;
}

public class ChartManager : MonoBehaviour
{
    public GameObject editorNote;
    public GameObject editorBlast;
    public GameObject targetCanvas;

    List<Note> chart;
    float currentTime = 0f;
    List<GameObject> visibleNotes;
    int firstVisibleIndex = 0;
    int numNotesVisible = 0;
    string selectedMode = "None";

    static float screenTimeRange = 3.5f;
    static float placementPosition = 0.5f;  //where in current range represents current time (percentage based; 0.5 means halfway through range)

    static float maxHeight = 3.5f;
    static float minHeight = -3.5f;

    static float columnWidth = 1.0438f; //change both of these if column width/positioning is changed
    static float leftmostColumnPlacement = -4.67f;
    // Start is called before the first frame update
    void Start()
    {
        chart = new List<Note>();
        visibleNotes = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        RemoveOutOfRangeNotes();
        UpdateNotePositions();
        //Debug.Log("current first index: " + firstVisibleIndex + "; current num of notes: " + numNotesVisible);
    }

    void RemoveOutOfRangeNotes()
    {
        for (int i = 0; i < numNotesVisible; i++)
        {
            if (chart[i + firstVisibleIndex].time >= currentTime - placementPosition * screenTimeRange)
            {
                return;
            } else
            {
                Debug.Log("need to remove a note!");
                visibleNotes[i].SetActive(false);
                visibleNotes.RemoveAt(i);
                firstVisibleIndex += 1;
                numNotesVisible -= 1;
            }
        }
    }

    void UpdateNotePositions()
    {
        for (int visibleIndex = 0; visibleIndex < numNotesVisible; visibleIndex++)
        {    //place new note before the first note that has a higher time than the current time, or at the end if no notes fit rule
            int chartIndex = visibleIndex + firstVisibleIndex;
            GameObject noteObject = visibleNotes[visibleIndex];
            Note note = chart[chartIndex];
            //float xPosition = leftmostColumnPlacement + (columnWidth * column);
            float timeDifference = note.time - currentTime;
            float percentageYPosition = ((placementPosition * screenTimeRange) + timeDifference) / screenTimeRange;
            float yPosition = minHeight + (percentageYPosition * maxHeight);// + Mathf.Max(timeDifference, 0) * maxHeight * (1-placementPosition) + Mathf.Min(timeDifference, 0) ;
            noteObject.transform.localPosition = new Vector2(noteObject.transform.localPosition.x, yPosition);
        }
    }

    public void PlaceNote(int column)
    {
        Debug.Log("calling place note!");
        if (selectedMode == "None") {
            Debug.Log("wrong mode");
            return;
        }

        Note newNote = new Note();
        newNote.time = currentTime;
        newNote.blast = selectedMode == "Blast";
        newNote.column = column;
        numNotesVisible++;

        for (int visibleIndex = 0; visibleIndex < numNotesVisible; visibleIndex++) {    //place new note before the first note that has a higher time than the current time, or at the end if no notes fit rule
            int chartIndex = visibleIndex + firstVisibleIndex;
            if (visibleIndex == numNotesVisible - 1 || chart[chartIndex].time >= currentTime)
            {
                chart.Insert(chartIndex, newNote);
                GameObject noteObject;
                float noteXPosition = leftmostColumnPlacement + (columnWidth * column);
                if (selectedMode == "Blast")
                {
                    noteObject = Instantiate(editorBlast);
                } else
                {
                    noteObject = Instantiate(editorNote);
                }
                noteObject.transform.SetParent(targetCanvas.transform);
                noteObject.transform.localPosition = new Vector2(noteXPosition, 0);
                visibleNotes.Insert(visibleIndex, noteObject);
                return;
            }
        }
    }

    public void ToggleMode(string targetMode)
    {
        if (targetMode == selectedMode)
        {
            selectedMode = "None";
        } else
        {
            selectedMode = targetMode;
        }
        Debug.Log("Mode has been toggled to " + selectedMode);
    }

    public void PrintChart()
    {
        Debug.Log("displaying notechart!");
        for (int i = 0; i < chart.Count; i++)
        {
            Debug.Log("note " + i + ": time = " + chart[i].time + ", column = " + chart[i].column + ", blast = " + chart[i].blast);
        }
        Debug.Log("finished notechart!");
    }
}
