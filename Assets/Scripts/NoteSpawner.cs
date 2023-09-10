using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public GameObject Target;
    public GameObject BlastNote;

    static float timeBetweenNotes = 0.25f;
    static float leftBound = -8f;
    static float rightBound = 8f;
    static float spawnY = 6f;

    float timeLeft;
    // Start is called before the first frame update
    void Start()
    {
        timeLeft = timeBetweenNotes;
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            int noteType = Random.Range(0, 2);
            float spawnX = Random.Range(leftBound, rightBound);
            if (noteType == 0)
            {
                Instantiate(Target, new Vector2(spawnX, spawnY), Quaternion.identity);
            } else
            {
                Instantiate(BlastNote, new Vector2(spawnX, spawnY), Quaternion.identity);
            }
            timeLeft = timeBetweenNotes;
        }
    }
}
