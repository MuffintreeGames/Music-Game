using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class TutorialNoteSpawner : NoteSpawner
{
    static float introEndTime = 10.669f;
    static float timeBetweenNotes = 1f;
    static float endOfBeats = 24.67f;
    static float endOfLoop = 46.6f;
    static int[] columnOrder1 = { 3, 6, 1, 8 };

    int stageOfTutorial = 0;
    int columnIndex = 0;
    float timeLeftUntilNote = 0f;
    // Start is called before the first frame update
    void Start()
    {
        //songSource.Play();
        currentTime = 0f;
        preludeTimeLeft = preludeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (preludeTimeLeft > 0f)
        {
            preludeTimeLeft -= Time.deltaTime;
            if (preludeTimeLeft < 0f)
            {
                songSource.Play();
            }
        }

        currentTime += Time.deltaTime;
        //songSource.time = currentTime;
        float normalNoteTime = currentTime + normalNoteDelay;
        if (normalNoteTime > introEndTime)
        {
            if (currentTime < endOfBeats) {
                timeLeftUntilNote -= Time.deltaTime;
                if (timeLeftUntilNote <= 0f)
                {
                    SpawnTutorialNotes();
                    timeLeftUntilNote = timeBetweenNotes;
                }
            }
        }
    }

    void SpawnTutorialNotes()
    {
        switch (stageOfTutorial)
        {
            case 0:
                float xPos = leftmostColumnPlacement + (columnOrder1[columnIndex] * columnWidth);
                Debug.Log("spawning note at " + currentTime);
                Instantiate(Target, new Vector2(xPos, spawnY), Quaternion.identity);
                timeLeftUntilNote = timeBetweenNotes;
                columnIndex++;
                if (columnIndex >= columnOrder1.Length)
                {
                    columnIndex = 0;
                }
                break;
        }
    }

    void StageCompleted()
    {
        stageOfTutorial++;
        currentTime = introEndTime;
        columnIndex = 0;
    }
}
