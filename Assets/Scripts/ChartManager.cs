using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Note
{
    public GameObject noteObject;
    public float time;
    public string type;
    public int column;
}

[Serializable]
public class NoteList
{
    public List<Note> list = new List<Note>();
}

public class ChartManager : MonoBehaviour
{
    public GameObject editorNote;
    public GameObject editorBlast;
    public GameObject editorYellow;
    public GameObject editorBlue;
    public GameObject targetCanvas;
    public AudioSource songSource;
    public Slider timeline;
    public TMP_InputField importField;

    public static string selectedMode = "None";

    NoteList chart;
    float currentTime = 0f;
    float songLength = 0f;
    List<GameObject> visibleNotes;
    int firstVisibleIndex = 0;  //earliest note in song that is currently on screen
    int numNotesVisible = 0;
    
    bool shortPause = false;
    bool longPause = false;

    static float screenTimeRange = 7f;
    static float placementPosition = 0.5f;  //where in current range represents current time (percentage based; 0.5 means halfway through range)

    static float maxHeight = 3.5f;
    static float minHeight = -3.5f;

    public static float columnWidth = 1.0438f; //change both of these if column width/positioning is changed
    public static float leftmostColumnPlacement = -4.67f;

    static float skipDuration = 5f;
    // Start is called before the first frame update
    void Start()
    {
        chart = new NoteList();
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
            songSource.time = currentTime;
            songSource.Play();
        }

        currentTime += Time.deltaTime;
        CheckForKeyPresses();
        PerformNoteUpkeep();
        UpdateTimeline();
        //Debug.Log("current first index: " + firstVisibleIndex + "; current num of notes: " + numNotesVisible);
    }

    void CheckForKeyPresses()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlaceNote(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlaceNote(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlaceNote(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlaceNote(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            PlaceNote(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            PlaceNote(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            PlaceNote(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            PlaceNote(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            PlaceNote(8);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            PlaceNote(9);
        }
    }
    

    void PerformNoteUpkeep()    //use whenever doing anything that changes the note positions
    {
        RemoveNotesFromBottom();
        RemoveNotesFromTop();
        AddNotesToTop();
        AddNotesToBottom();
        UpdateNotePositions();
    }

    bool IsPaused()
    {
        return shortPause || longPause;
    }

    public void BriefPlaybackPause()
    {
        shortPause = true;
    }

    public void EndBriefPause()
    {
        shortPause = false;
    }

    public void FullPause()
    {
        longPause = true;
    }

    public void EndFullPause()
    {
        longPause = false;
    }

    void UpdateTimeline()
    {
        timeline.SetValueWithoutNotify(currentTime / songLength);
    }

    void AddNotesToTop()
    {
        //Debug.Log("first index: " + firstVisibleIndex + "; num of notes: " + numNotesVisible);
        for (int i = firstVisibleIndex + numNotesVisible; i < chart.list.Count; i++)
        {
            //Debug.Log("comparing " + currentTime + " to " + chart[i].time);
            if (chart.list[i].time > currentTime + placementPosition * screenTimeRange)
            {
                return;
            } else
            {
                Debug.Log("found a new note to add to top: " + i + "!");
                numNotesVisible++;
                
                chart.list[i].noteObject.SetActive(true);
                visibleNotes.Add(chart.list[i].noteObject);
            }
        }
    }

    void AddNotesToBottom()
    {
        //Debug.Log("first index: " + firstVisibleIndex + "; num of notes: " + numNotesVisible);
        for (int i = firstVisibleIndex - 1; i >= 0; i--)
        {
            if (chart.list[i].time < currentTime - placementPosition * screenTimeRange)
            {
                return;
            }
            else
            {
                Debug.Log("found a new note to add to bottom: " + i + "!");
                numNotesVisible++;
                firstVisibleIndex--;
                if (chart.list[i].noteObject == null)
                {
                    Debug.LogError("error: " + i + "has a null note object!");
                    return;
                }
                chart.list[i].noteObject.SetActive(true);
                visibleNotes.Insert(0, chart.list[i].noteObject);
            }
        }
    }

    void RemoveNotesFromBottom()    //remove notes that are too old to fit (too small of a time value)
    {
        for (int i = 0; i < numNotesVisible; i++)
        {
            if (chart.list[i + firstVisibleIndex].time >= currentTime - placementPosition * screenTimeRange)
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
            if (chart.list[i + firstVisibleIndex].time <= currentTime + placementPosition * screenTimeRange)
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
            Note note = chart.list[chartIndex];
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
        if (selectedMode != "Note" && selectedMode != "Blast" && selectedMode != "Yellow" && selectedMode != "Blue") {
            Debug.Log("wrong mode");
            return;
        }

        Note newNote = new Note();
        newNote.time = currentTime;
        newNote.type = selectedMode;
        newNote.column = column;
        numNotesVisible++;

        for (int visibleIndex = 0; visibleIndex < numNotesVisible; visibleIndex++) {    //place new note before the first note that has a higher time than the current time, or at the end if no notes fit rule
            int chartIndex = visibleIndex + firstVisibleIndex;
            if (visibleIndex == numNotesVisible - 1 || chart.list[chartIndex].time >= currentTime)
            {
                GameObject noteObject;
                float noteXPosition = leftmostColumnPlacement + (columnWidth * column);
                if (selectedMode == "Blast")
                {
                    noteObject = Instantiate(editorBlast);
                } else if (selectedMode == "Yellow")
                {
                    noteObject = Instantiate(editorYellow);
                }
                else if (selectedMode == "Blue")
                {
                    noteObject = Instantiate(editorBlue);
                } else
                {
                    noteObject = Instantiate(editorNote);
                }
                newNote.noteObject = noteObject;
                chart.list.Insert(chartIndex, newNote);
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
            return;
            //selectedMode = "None";
        } else
        {
            selectedMode = targetMode;
        }
        Debug.Log("Mode has been toggled to " + selectedMode);
    }

    public void PrintChart()
    {
        Debug.Log("displaying notechart!");
        for (int i = 0; i < chart.list.Count; i++)
        {
            Debug.Log("note " + i + ": time = " + chart.list[i].time + ", column = " + chart.list[i].column + ", type = " + chart.list[i].type);
        }
        Debug.Log("finished notechart!");
    }

    public void UpdateSongPosition()
    {
        Debug.Log("updating song position!");
        currentTime = Mathf.Clamp(timeline.value * songLength, 0f, songLength);
        //songSource.Stop();
        songSource.time = currentTime;
        /*if (currentTime < songLength)
        {
            songSource.Play();
        }*/
        PerformNoteUpkeep();
    }

    public void SkipForward()
    {
        currentTime = Mathf.Min(currentTime + skipDuration, songLength);
        songSource.time = currentTime;
        PerformNoteUpkeep();
        UpdateTimeline();
    }

    public void SkipBackward()
    {
        currentTime = Mathf.Max(currentTime - skipDuration, 0f);
        songSource.time = currentTime;
        PerformNoteUpkeep();
        UpdateTimeline();
    }

    public void ExportNotechart()
    {
        string output = JsonUtility.ToJson(chart);
        GUIUtility.systemCopyBuffer = output;
    }

    public void ImportNotechart()
    {
        NoteList newChart = JsonUtility.FromJson<NoteList>(importField.text);
        if (newChart == null)
        {
            Debug.LogError("Failed to read import data!");
            return;
        }
        for (int i = 0; i < chart.list.Count; i++)
        {
            Destroy(chart.list[i].noteObject);
        }
        chart = newChart;
        for (int i = 0; i < chart.list.Count; i++)
        {
            GameObject noteObject;
            float noteXPosition = leftmostColumnPlacement + (columnWidth * chart.list[i].column);
            if (chart.list[i].type == "Blast")
            {
                noteObject = Instantiate(editorBlast);
            }
            else if (chart.list[i].type == "Yellow")
            {
                noteObject = Instantiate(editorYellow) ;
            }
            else if (chart.list[i].type == "Blue")
            {
                noteObject = Instantiate(editorBlue);
            } else
            {
                noteObject = Instantiate(editorNote);
            }
            noteObject.SetActive(false);
            chart.list[i].noteObject = noteObject;
            //chart.list.Insert(chartIn, newNote);
            noteObject.transform.SetParent(targetCanvas.transform);
            noteObject.transform.localPosition = new Vector2(noteXPosition, 0);
            //visibleNotes.Insert(visibleIndex, noteObject);
        }
        visibleNotes = new List<GameObject>();
        currentTime = 0f;
        firstVisibleIndex = 0;
        numNotesVisible = 0;
        longPause = true;
        
        PerformNoteUpkeep();
        UpdateTimeline();
    }

    public void DeleteNote(GameObject targetNote)
    {
        for (int i = 0; i < visibleNotes.Count; i++)
        {
            if (visibleNotes[i] == targetNote)
            {
                visibleNotes.RemoveAt(i);
                int chartIndex = firstVisibleIndex + i;
                chart.list.RemoveAt(chartIndex);
                numNotesVisible--;
                Destroy(targetNote);
                return;
            }
        }
        Debug.LogError("failed to find note to delete!");
    }

    public void MoveNote(GameObject targetNote, int targetColumn)
    {
        for (int i = 0; i < visibleNotes.Count; i++)
        {
            if (visibleNotes[i] == targetNote)
            {
                int chartIndex = firstVisibleIndex + i;
                Note noteInfo = chart.list[chartIndex];
                if (targetColumn != -1)
                {
                    noteInfo.column = targetColumn;
                }
                float percentageYPosition = (targetNote.transform.localPosition.y - minHeight) / (maxHeight - minHeight);
                float newTime = currentTime + (percentageYPosition * screenTimeRange - placementPosition * screenTimeRange);
                float oldTime = noteInfo.time;
                noteInfo.time = newTime;
                chart.list[chartIndex] = noteInfo;
                //Debug.Log("old time for note was " + noteInfo.time + "; calculated new time is " + newTime);
                bool changedPosition = true;
                if (newTime > oldTime)
                {
                    for (int j = i + 1; j <= visibleNotes.Count; j++)
                    {
                        int jChartIndex = firstVisibleIndex + j;
                        if (j == visibleNotes.Count)
                        {
                            visibleNotes.Add(targetNote);
                            visibleNotes.RemoveAt(i);
                            chart.list.Insert(jChartIndex, noteInfo);
                            chart.list.RemoveAt(chartIndex);
                            break;
                        }
                        
                        Debug.Log("index being checked: " + jChartIndex);
                        if (chart.list[jChartIndex].time >= newTime)
                        {
                            visibleNotes.Insert(j, targetNote);
                            visibleNotes.RemoveAt(i);
                            chart.list.Insert(jChartIndex, noteInfo);
                            chart.list.RemoveAt(chartIndex);
                            break;
                        }
                    }
                } else
                {
                    for (int j = i - 1; j >= -1; j--)
                    {
                        int jChartIndex = firstVisibleIndex + j;
                        if (j == -1 || chart.list[jChartIndex].time <= newTime)
                        {
                            visibleNotes.RemoveAt(i);
                            visibleNotes.Insert(j + 1, targetNote);
                            chart.list.RemoveAt(chartIndex);
                            chart.list.Insert(jChartIndex + 1, noteInfo);
                            break;
                        }
                    }
                }
                return;
            }
        }
        Debug.LogError("failed to find note to move!");
    }
}
