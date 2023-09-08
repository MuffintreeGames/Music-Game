using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTracker : MonoBehaviour
{
    public float wasdSoundPosition = -5f;   //position of the soundwave controlled with a/d, starts on left
    public float arrowSoundPosition = 5f;   //position of the soundwave controlled with arrows, starts on right

    static float soundMovementSpeed = 12f;
    static float leftBound = -8.5f;
    static float rightBound = 8.5f;
    // Start is called before the first frame update
    void Start()
    {
        wasdSoundPosition = -5f;
        arrowSoundPosition = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        float arrowAxis = Input.GetAxis("ArrowMovement");
        arrowSoundPosition += arrowAxis * soundMovementSpeed * Time.deltaTime;
        if (arrowSoundPosition > rightBound)
        {
            arrowSoundPosition = rightBound;
        } else if (arrowSoundPosition < leftBound)
        {
            arrowSoundPosition = leftBound;
        }

        float wasdAxis = Input.GetAxis("WASDMovement");
        wasdSoundPosition += wasdAxis * soundMovementSpeed * Time.deltaTime;
        if (wasdSoundPosition > rightBound)
        {
            wasdSoundPosition = rightBound;
        }
        else if (wasdSoundPosition < leftBound)
        {
            wasdSoundPosition = leftBound;
        }
    }
}
