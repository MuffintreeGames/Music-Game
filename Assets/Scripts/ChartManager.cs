using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

struct Note
{
    public GameObject noteObject;
    public float time;
    public bool blast;
    public int column;
}

public class ChartManager : MonoBehaviour
{
    public GameObject editorNote;
    public GameObject editorBlast;
    public GameObject targetCanvas;
    public AudioSource songSource;
    public Slider timeline;

    List<Note> chart;
    float currentTime = 0f;
    float songLength = 0f;
    List<GameObject> visibleNotes;
    int firstVisibleIndex = 0;  //earliest note in song that is currently on screen
    int numNotesVisible = 0;
    string selectedMode = "None";
    bool shortPause = false;

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
        songLength = songSource.clip.length;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPaused())
        {
            songSource.Stop();
            return;
        } else if (!songSource.isPlaying)
        {
            songSource.Play();
        }

        currentTime += Time.deltaTime;
        RemoveNotesFromBottom();
        RemoveNotesFromTop();
        AddNotesToTop();
        AddNotesToBottom();
        UpdateNotePositions();
        UpdateTimeline();
        //Debug.Log("current first index: " + firstVisibleIndex + "; current num of notes: " + numNotesVisible);
    }

    bool IsPaused()
    {
        return shortPause;
    }

    public void BriefPlaybackPause()
    {
        shortPause = true;
    }

    public void EndBriefPause()
    {
        shortPause = false;
    }

    void UpdateTimeline()
    {
        timeline.SetValueWithoutNotify(currentTime / songLength);
    }

    void AddNotesToTop()
    {
        //Debug.Log("first index: " + firstVisibleIndex + "; num of notes: " + numNotesVisible);
        for (int i = firstVisibleIndex + numNotesVisible; i < chart.Count; i++)
        {
            //Debug.Log("comparing " + currentTime + " to " + chart[i].time);
            if (chart[i].time > currentTime + placementPosition * screenTimeRange)
            {
                return;
            } else
            {
                Debug.Log("found a new note to add to top: " + i + "!");
                numNotesVisible++;
                
                chart[i].noteObject.SetActive(true);
                visibleNotes.Add(chart[i].noteObject);
            }
        }
    }

    void AddNotesToBottom()
    {
        //Debug.Log("first index: " + firstVisibleIndex + "; num of notes: " + numNotesVisible);
        for (int i = firstVisibleIndex - 1; i >= 0; i--)
        {
            Debug.Log("comparing " + currentTime + " to " + chart[i].time);
            if (chart[i].time < currentTime - placementPosition * screenTimeRange)
            {
                return;
            }
            else
            {
                Debug.Log("found a new note to add to bottom: " + i + "!");
                numNotesVisible++;
                firstVisibleIndex--;
                if (chart[i].noteObject == null)
                {
                    Debug.LogError("error: " + i + "has a null note object!");
                    return;
                }
                chart[i].noteObject.SetActive(true);
                visibleNotes.Insert(0, chart[i].noteObject);
            }
        }
    }

    void RemoveNotesFromBottom()    //remove notes that are too old to fit (too small of a time value)
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
                firstVisibleIndex += 1; //removing the earliest note on screen, so need to update this value as there's a new earliest note
                numNotesVisible -= 1;
            }
        }
    }

    void RemoveNotesFromTop()   //remove notes that are too far ahead (too large of a time value)
    {
        for (int i = numNotesVisible-1; i >= 0; i--)
        {
            if (chart[i + firstVisibleIndex].time <= currentTime + placementPosition * screenTimeRange)
            {
                return;
            }
            else
            {
                Debug.Log("need to remove a note!");
                visibleNotes[i].SetActive(false);
                visibleNotes.RemoveAt(i);
                numNotesVisible -= 1;
            }
        }
    }

    void UpdateNotePositions()
    {
        for (int visibleIndex = 0; visibleIndex < numNotesVisible; visibleIndex++)
        { 
            int chartIndex = visibleIndex + firstVisibleIndex;
            GameObject noteObject = visibleNotes[visibleIndex];
            Note note = chart[chartIndex];
            //float xPosition = leftmostColumnPlacement + (columnWidth * column);
            float timeDifference = note.time - currentTime;
            float percentageYPosition = ((placementPosition * screenTimeRange) + timeDifference) / screenTimeRange;
            float yPosition = minHeight + (percentageYPosition * (maxHeight-minHeight));// + Mathf.Max(timeDifference, 0) * maxHeight * (1-placementPosition) + Mathf.Min(timeDifference, 0) ;
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
                GameObject noteObject;
                float noteXPosition = leftmostColumnPlacement + (columnWidth * column);
                if (selectedMode == "Blast")
                {
                    noteObject = Instantiate(editorBlast);
                } else
                {
                    noteObject = Instantiate(editorNote);
                }
                newNote.noteObject = noteObject;
                chart.Insert(chartIndex, newNote);
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

    public void UpdateSongPosition()
    {
        Debug.Log("updating song position!");
        currentTime = Mathf.Min(timeline.value * songLength, songLength);
        //songSource.Stop();
        songSource.time = currentTime;
        /*if (currentTime < songLength)
        {
            songSource.Play();
        }*/
        RemoveNotesFromBottom();
        RemoveNotesFromTop();
        AddNotesToTop();
        AddNotesToBottom();
        UpdateNotePositions();
    }

    public void ExportNotechart()
    {
        string output = "";
        for (int i = 0; i < chart.Count; i++)
        {
            output += JsonUtility.ToJson(chart[i]);
            //output += "time " + chart[i].time + " column " + chart[i].column + " blast " + chart[i].blast + " ";
        }
        GUIUtility.systemCopyBuffer = output;
    }
}
