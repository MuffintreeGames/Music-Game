using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBar : MonoBehaviour
{
    public Color wasdColor;
    public Color arrowColor;
    public Color combinedColor;
    public Color defaultColor;

    PositionTracker positionTracker;
    float minScale = 0.1f;
    float maxScale = .8f;
    float combinedMaxScale = 1.4f;
    float detectionDistance = 2f;
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
        SetVisuals();
    }

    void SetVisuals()
    {
        float newScale = minScale;
        Color newColor = defaultColor;

        float distanceFromArrowSound = Mathf.Abs(transform.position.x - positionTracker.arrowSoundPosition);
        float distanceFromWasdSound = Mathf.Abs(transform.position.x - positionTracker.wasdSoundPosition);
        if (distanceFromArrowSound < detectionDistance && distanceFromWasdSound < detectionDistance)
        {
            newScale = minScale + ((detectionDistance - distanceFromArrowSound) / detectionDistance * (maxScale - minScale)) + ((detectionDistance - distanceFromWasdSound) / detectionDistance * (maxScale - minScale));
            newColor = new Color(defaultColor.r + ((arrowColor.r - defaultColor.r) * ((detectionDistance - distanceFromArrowSound) / detectionDistance)), defaultColor.g - (defaultColor.g * (detectionDistance * 2 - distanceFromArrowSound - distanceFromWasdSound)), defaultColor.b + ((wasdColor.b - defaultColor.b) * ((detectionDistance - distanceFromWasdSound) / detectionDistance)));
        }
        else
        {
            if (distanceFromArrowSound < detectionDistance)
            {
                newScale = minScale + ((detectionDistance - distanceFromArrowSound) / detectionDistance * (maxScale - minScale));
                newColor = new Color(defaultColor.r + ((arrowColor.r - defaultColor.r) * ((detectionDistance - distanceFromArrowSound) / detectionDistance)), defaultColor.g + ((arrowColor.g - defaultColor.g) * ((detectionDistance - distanceFromArrowSound) / detectionDistance)), defaultColor.b + ((arrowColor.b - defaultColor.b) * ((detectionDistance - distanceFromArrowSound) / detectionDistance)));
            }
            else if (distanceFromWasdSound < detectionDistance)
            {
                newScale = minScale + ((detectionDistance - distanceFromWasdSound) / detectionDistance * (maxScale - minScale));
                newColor = new Color(defaultColor.r - ((defaultColor.r - wasdColor.r) * ((detectionDistance - distanceFromWasdSound) / detectionDistance)), defaultColor.g + ((wasdColor.g - defaultColor.g) * ((detectionDistance - distanceFromWasdSound) / detectionDistance)), defaultColor.b + ((wasdColor.b - defaultColor.b) * ((detectionDistance - distanceFromWasdSound) / detectionDistance)));
            }
        }


        transform.localScale = new Vector2(transform.localScale.x, newScale);
        GetComponentInChildren<SpriteRenderer>().color = newColor;
    }
}
