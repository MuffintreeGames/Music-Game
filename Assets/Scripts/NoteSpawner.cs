using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteSpawner : MonoBehaviour
{
    public GameObject Target;
    public GameObject BlastNote;
    public GameObject YellowNote;
    public GameObject BlueNote;
    public string rawChart;
    public Slider timeline;
    public AudioSource songSource;

    static float spawnY = 6f;

    NoteList chart;
    float currentTime;
    float checkpointTime;
    bool checkpointReached = false;
    int chartIndex = 0;

    static float columnWidth = 1.778f; //change both of these if column width/positioning is changed
    static float leftmostColumnPlacement = -8f;
    static float countdownTime = 1f;    //use for effects when starting level like a countdown
    static float preludeTime = 2f;

    static float normalNoteDelay = 1.5f;
    static float blastNoteDelay = 1.9f;

    float countdownTimeLeft = 0f;
    float preludeTimeLeft = 0f;

    bool deathPause = false;

    int notesSpawned;
    // Start is called before the first frame update
    void Start()
    {
        chart = JsonUtility.FromJson<NoteList>(rawChart);
        chartIndex = 0;
        Debug.Log("chart loaded: " + chart.list[0].time);
        preludeTimeLeft = preludeTime;
        countdownTimeLeft = countdownTime;
        checkpointTime = chart.checkpointTime;// + preludeTime;
        deathPause = false;
        HealthTracker.playerDeath.AddListener(PauseOnDeath);
        HealthTracker.songReset.AddListener(ResetToCheckpoint);
    }

    // Update is called once per frame
    void Update()
    {
        if (deathPause) return;

        if (countdownTimeLeft > 0f) { 
            countdownTimeLeft -= Time.deltaTime;
            /*if (countdownTimeLeft < 0f )
            {
                songSource.Play();
            }*/
            return;
        }

        if (preludeTimeLeft > 0f)
        {
            preludeTimeLeft -= Time.deltaTime;
            if (preludeTimeLeft < 0f)
            {
                songSource.Play();
            }
        }

        currentTime += Time.deltaTime;
        notesSpawned = 0;
        SpawnNewNormalNotes();
        SpawnNewBlastNotes();
        chartIndex += notesSpawned;
        UpdateTimeline();
        if (currentTime >= checkpointTime && !checkpointReached)
        {
            Debug.Log("hit the checkpoint!");
            checkpointReached = true;
        }
    }

    void UpdateTimeline()
    {
        timeline.value = currentTime / songSource.clip.length;
    }

    void SpawnNewNormalNotes()
    {
        for (int i = chartIndex; i < chart.list.Count; i++)
        {
            Note nextNote = chart.list[i];
            if (nextNote.type != "Blast")
            {
                if (nextNote.time > currentTime + normalNoteDelay)
                {
                    return;
                }
                else
                {
                    float xPos = leftmostColumnPlacement + (nextNote.column * columnWidth);

                    switch (nextNote.type)
                    {
                        case "Yellow": Instantiate(YellowNote, new Vector2(xPos, spawnY), Quaternion.identity); break;
                        case "Blue": Instantiate(BlueNote, new Vector2(xPos, spawnY), Quaternion.identity); break;
                        case "Note": Instantiate(Target, new Vector2(xPos, spawnY), Quaternion.identity); break;
                    }
                    notesSpawned++;
                }
            }
        }
    }

    void SpawnNewBlastNotes()
    {
        for (int i = chartIndex; i < chart.list.Count; i++)
        {
            Note nextNote = chart.list[i];
            if (nextNote.type == "Blast")
            {
                if (nextNote.time > currentTime + blastNoteDelay)
                {
                    return;
                }
                else
                {
                    float xPos = leftmostColumnPlacement + (nextNote.column * columnWidth);

                    Instantiate(BlastNote, new Vector2(xPos, spawnY), Quaternion.Euler(0, 0, 45f));
                    notesSpawned++;
                }
            }
        }
    }

    void PauseOnDeath()
    {
        songSource.Stop();
        deathPause = true;
    }

    void ResetToCheckpoint()
    {
        if (checkpointReached)
        {
            currentTime = checkpointTime - preludeTime;
            preludeTimeLeft = preludeTime;
            songSource.time = currentTime;// - preludeTime;
            //songSource.Play();
            //chartIndex = 0;
            RecalculateIndex();
        } else
        {
            currentTime = 0;
            preludeTimeLeft = preludeTime;
            songSource.time = currentTime;
            chartIndex = 0;
        }
        deathPause = false;
    }

    void RecalculateIndex() //after going to checkpoint, need to find new chart index value
    {
        while (chart.list[chartIndex - 1].time > currentTime + preludeTime)
        {
            chartIndex--;
            if (chartIndex == 0)
            {
                return;
            }
        }
    }
}
