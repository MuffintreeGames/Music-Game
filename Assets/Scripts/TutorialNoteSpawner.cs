using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialNoteSpawner : NoteSpawner
{
    static float introEndTime = 10.47f;
    static float restartTime = 1.165f;
    static float triggerRestartTime = 7.17f;
    static float timeBetweenNotes = 1f;
    static float endOfBeats = 24.67f;
    static float endOfLoop = 42.5f;
    static float endOfStage3 = 35f;
    static int[] columnOrder1 = { 3, 6, 1, 8 };
    static int[] columnOrder2 = { 3, 6, 1, 8 };
    static int[] columnOrder3 = { 3, -1, 6, -1 };
    static string[] colorOrder2 = { "blue", "yellow", "yellow", "blue" };

    static float[] stage0MessageTimings = { 1f, 4f, 8f, 11f };
    static float[] stage1MessageTimings = { 3f, 8f, 12f };
    static float[] stage2MessageTimings = { 3f, 8f, 12f };
    static float[] stage3MessageTimings = { 3f, 6f };

    public TextFader[] stage0TutorialMessages;
    public TextFader[] stage1TutorialMessages;
    public TextFader[] stage2TutorialMessages;
    public TextFader[] stage3TutorialMessages;

    int tutorialIndex = 0;
    float tutorialTimer = 0f;

    int stageOfTutorial = 0;
    int columnIndex = 0;
    float timeLeftUntilNote = 0f;
    bool triggeredEndOfBeats = false;
    bool triggeredInitialLoop = false;
    // Start is called before the first frame update
    void Start()
    {
        //songSource.Play();
        currentTime = 0f;
        tutorialTimer = 0f;
        preludeTimeLeft = preludeTime;
        HealthTracker.playerDeath.AddListener(base.PauseOnDeath);
        HealthTracker.songReset.AddListener(ResetToCheckpoint);
        songSource = GetComponent<AudioSource>();
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

        HandleTutorialMessages();
        currentTime += Time.deltaTime;
        if (stageOfTutorial == 3 && currentTime > endOfStage3)
        {
            Debug.Log("ending tutorial");
            MusicController.UpdateMusicManual(1);
            SceneManager.LoadScene("Gameplay");
        }
        //songSource.time = currentTime;
        if (!triggeredInitialLoop)
        {
            if (currentTime > triggerRestartTime)
            {
                currentTime = restartTime + preludeTime;
                songSource.time = restartTime;
                triggeredInitialLoop = true;
            }
        } else {
            float normalNoteTime = currentTime + normalNoteDelay;
            float blastNoteTime = currentTime + blastNoteDelay;
            if (normalNoteTime > introEndTime || (stageOfTutorial == 2 && blastNoteTime > introEndTime))
            {
                if (currentTime < endOfBeats || normalNoteTime > endOfLoop || (stageOfTutorial == 2 && blastNoteDelay + currentTime > endOfLoop))
                {
                    timeLeftUntilNote -= Time.deltaTime;
                    if (timeLeftUntilNote <= 0f)
                    {
                        SpawnTutorialNotes();
                        timeLeftUntilNote = timeBetweenNotes;
                    }
                }

                if (currentTime > endOfBeats && !triggeredEndOfBeats)
                {
                    timeLeftUntilNote = 0f;
                    WipeTutorialMessages();
                    stageOfTutorial++;
                    columnIndex = 0;
                    triggeredEndOfBeats = true;
                }

                if (currentTime > endOfLoop)
                {
                    StageCompleted();
                }
            }
        }
    }

    void HandleTutorialMessages()
    {
        tutorialTimer += Time.deltaTime;
        switch (stageOfTutorial)
        {
            case 0:
                if (tutorialIndex >= stage0MessageTimings.Length)
                {
                    return;
                }
                if (tutorialTimer > stage0MessageTimings[tutorialIndex])
                {
                    stage0TutorialMessages[tutorialIndex].FadeIn();
                    tutorialIndex++;
                }
                break;

            case 1:
                if (tutorialIndex >= stage1MessageTimings.Length)
                {
                    return;
                }
                if (tutorialTimer > stage1MessageTimings[tutorialIndex])
                {
                    stage1TutorialMessages[tutorialIndex].FadeIn();
                    tutorialIndex++;
                }
                break;
            case 2:
                if (tutorialIndex >= stage2MessageTimings.Length)
                {
                    return;
                }
                if (tutorialTimer > stage2MessageTimings[tutorialIndex])
                {
                    stage2TutorialMessages[tutorialIndex].FadeIn();
                    tutorialIndex++;
                }
                break;
            case 3:
                if (tutorialIndex >= stage3MessageTimings.Length)
                {
                    return;
                }
                if (tutorialTimer > stage3MessageTimings[tutorialIndex])
                {
                    stage3TutorialMessages[tutorialIndex].FadeIn();
                    tutorialIndex++;
                }
                break;
        }
    }

    void WipeTutorialMessages()
    {
        switch (stageOfTutorial)
        {
            case 0:
                for (int i = 0; i < stage0TutorialMessages.Length; i++)
                {
                    stage0TutorialMessages[i].FadeOut();
                }
                break;
            case 1:
                for (int i = 0; i < stage1TutorialMessages.Length; i++)
                {
                    stage1TutorialMessages[i].FadeOut();
                }
                break;
            case 2:
                for (int i = 0; i < stage2TutorialMessages.Length; i++)
                {
                    stage2TutorialMessages[i].FadeOut();
                }
                break;
            case 3:
                for (int i = 0; i < stage3TutorialMessages.Length; i++)
                {
                    stage3TutorialMessages[i].FadeOut();
                }
                break;
        }
        tutorialTimer = 0;
        tutorialIndex = 0;
    }

    void SpawnTutorialNotes()
    {
        float xPos;
        switch (stageOfTutorial)
        {
            case 0:
                xPos = leftmostColumnPlacement + (columnOrder1[columnIndex] * columnWidth);
                Instantiate(Target, new Vector2(xPos, spawnY), Quaternion.identity);
                timeLeftUntilNote = timeBetweenNotes;
                columnIndex++;
                if (columnIndex >= columnOrder1.Length)
                {
                    columnIndex = 0;
                }
                break;
            case 1:
                xPos = leftmostColumnPlacement + (columnOrder2[columnIndex] * columnWidth);
                if (colorOrder2[columnIndex] == "blue")
                {
                    Instantiate(BlueNote, new Vector2(xPos, spawnY), Quaternion.identity);
                } else
                {
                    Instantiate(YellowNote, new Vector2(xPos, spawnY), Quaternion.identity);
                }
                
                timeLeftUntilNote = timeBetweenNotes;
                columnIndex++;
                if (columnIndex >= columnOrder2.Length)
                {
                    columnIndex = 0;
                }
                break;
            case 2:
                if (columnOrder3[columnIndex] != -1)
                {
                    xPos = leftmostColumnPlacement + (columnOrder3[columnIndex] * columnWidth);

                    Instantiate(BlastNote, new Vector2(xPos, spawnY), Quaternion.Euler(0, 0, 45f));
                }

                timeLeftUntilNote = timeBetweenNotes;
                columnIndex++;
                if (columnIndex >= columnOrder3.Length)
                {
                    columnIndex = 0;
                }
                break;
        }
    }

    new void ResetToCheckpoint()
    {
        /*if (checkpointReached)
        {
            currentTime = checkpointTime - preludeTime;
            preludeTimeLeft = preludeTime;
            songSource.time = currentTime;// - preludeTime;
            //songSource.Play();
            //chartIndex = 0;
            RecalculateIndex();
        }
        else
        {
            currentTime = 0;
            preludeTimeLeft = preludeTime;
            songSource.time = currentTime;
            chartIndex = 0;
        }
        deathPause = false;*/
        currentTime = introEndTime - preludeTime;
        preludeTimeLeft = preludeTime;
        songSource.time = currentTime;
        songSource.Stop();
        columnIndex = 0;
        timeLeftUntilNote = 0;
    }

    void StageCompleted()
    {
        //stageOfTutorial++;
        currentTime = introEndTime;
        //columnIndex = 0;
        songSource.time = currentTime - preludeTime;
        triggeredEndOfBeats = false;
    }
}
