using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBar : MonoBehaviour
{
    PositionTracker positionTracker;
    float minScale = 0.1f;
    float maxScale = .8f;
    float combinedMaxScale = 1.4f;
    float detectionDistance = 1f;
    // Start is called before the first frame update
    void Start()
    {
        positionTracker = GetComponentInParent<PositionTracker>();
        transform.localScale = new Vector2(transform.localScale.x, minScale);
    }

    // Update is called once per frame
    void Update()
    {
        if (positionTracker == null)
        {
            Debug.LogError("can't find position tracker! Abort!");
            return;
        }

        float newScale = minScale;

        float distanceFromArrowSound = Mathf.Abs(transform.position.x - positionTracker.arrowSoundPosition);
        float distanceFromWasdSound = Mathf.Abs(transform.position.x - positionTracker.wasdSoundPosition);
        if (distanceFromArrowSound < detectionDistance && distanceFromWasdSound < detectionDistance)
        {
            newScale = minScale + ((detectionDistance - distanceFromArrowSound) / detectionDistance * (maxScale - minScale)) + ((detectionDistance - distanceFromWasdSound) / detectionDistance * (maxScale - minScale));
        }
        else
        {
            if (distanceFromArrowSound < detectionDistance)
            {
                newScale = minScale + ((detectionDistance - distanceFromArrowSound) / detectionDistance * (maxScale - minScale));
            } else if (distanceFromWasdSound < detectionDistance)
            {
                newScale = minScale + ((detectionDistance - distanceFromWasdSound) / detectionDistance * (maxScale - minScale));
            }
        }



        transform.localScale = new Vector2(transform.localScale.x, newScale);
    }
}
