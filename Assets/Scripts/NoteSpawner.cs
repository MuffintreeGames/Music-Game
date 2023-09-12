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
    int chartIndex = 0;

    static float columnWidth = 1.778f; //change both of these if column width/positioning is changed
    static float leftmostColumnPlacement = -8f;
    static float preludeTime = 2f;

    static float normalNoteDelay = 1.5f;
    static float blastNoteDelay = 1.8f;

    float preludeTimeLeft = 0f;

    int notesSpawned;
    // Start is called before the first frame update
    void Start()
    {
        chart = JsonUtility.FromJson<NoteList>(rawChart);
        chartIndex = 0;
        Debug.Log("chart loaded: " + chart.list[0].time);
        preludeTimeLeft = preludeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (preludeTimeLeft > 0f) { 
            preludeTimeLeft -= Time.deltaTime;
            if (preludeTimeLeft < 0f )
            {
                songSource.Play();
            }
            return;
        }

        currentTime += Time.deltaTime;
        notesSpawned = 0;
        SpawnNewNormalNotes();
        SpawnNewBlastNotes();
        chartIndex += notesSpawned;
        UpdateTimeline();
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
}
