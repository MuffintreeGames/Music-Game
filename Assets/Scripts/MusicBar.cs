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
    float maxMaxScale = .8f;    //the name makes sense I promise
    float currentMaxScale = .8f;
    float combinedMaxScale = 1.4f;
    float detectionDistance = 2f;
    bool currentlyYellow = false;
    bool currentlyBlue = false;

    static float minHealthFactor = 0.5f;   //maximum scale subtracted by as much as this number as health approaches 0
    // Start is called before the first frame update
    void Start()
    {
        positionTracker = GetComponentInParent<PositionTracker>();
        transform.localScale = new Vector3(transform.localScale.x, minScale, transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (positionTracker == null)
        {
            Debug.LogError("can't find position tracker! Abort!");
            return;
        }
        AdjustMaxScale();
        SetVisuals();
    }

    void AdjustMaxScale()   //if hp is less than 100, reduce max scale
    {
        currentMaxScale = Mathf.Max(minScale, maxMaxScale - (minHealthFactor * 0.01f * (100f - HealthTracker.publicHealth)));
    }

    Color GetCurrentWasdColor()
    {
        if (positionTracker.colorsSwapped)
        {
            return arrowColor;
        } else
        {
            return wasdColor;
        }
    }

    Color GetCurrentArrowColor()
    {
        if (positionTracker.colorsSwapped)
        {
            return wasdColor;
        }
        else
        {
            return arrowColor;
        }
    }

    public bool IsCurrentlyYellow()
    {
        Debug.Log("currently yellow: " + currentlyYellow + ", current blue: " + currentlyBlue);
        return currentlyYellow;
    }

    public bool IsCurrentlyBlue()
    {
        return currentlyBlue;
    }

    void SetVisuals()
    {
        float newScale = minScale;
        Color newColor = defaultColor;
        Color currentWasdColor = GetCurrentWasdColor();
        Color currentArrowColor = GetCurrentArrowColor();

        float distanceFromArrowSound = Mathf.Abs(transform.position.x - positionTracker.arrowSoundPosition);
        float distanceFromWasdSound = Mathf.Abs(transform.position.x - positionTracker.wasdSoundPosition);
        if (distanceFromArrowSound < detectionDistance && distanceFromWasdSound < detectionDistance)
        {
            currentlyBlue = true;
            currentlyYellow = true;
            newScale = minScale + ((detectionDistance - distanceFromArrowSound) / detectionDistance * (currentMaxScale - minScale)) + ((detectionDistance - distanceFromWasdSound) / detectionDistance * (currentMaxScale - minScale));
            if (positionTracker.colorsSwapped)
            {
                newColor = new Color(defaultColor.r + ((arrowColor.r - defaultColor.r) * ((detectionDistance - distanceFromWasdSound) / detectionDistance)), defaultColor.g - (defaultColor.g * (detectionDistance * 2 - distanceFromArrowSound - distanceFromWasdSound)), defaultColor.b + ((wasdColor.b - defaultColor.b) * ((detectionDistance - distanceFromArrowSound) / detectionDistance)));
            } else
            {
                newColor = new Color(defaultColor.r + ((arrowColor.r - defaultColor.r) * ((detectionDistance - distanceFromArrowSound) / detectionDistance)), defaultColor.g - (defaultColor.g * (detectionDistance * 2 - distanceFromArrowSound - distanceFromWasdSound)), defaultColor.b + ((wasdColor.b - defaultColor.b) * ((detectionDistance - distanceFromWasdSound) / detectionDistance)));
            }
        }
        else
        {
            if (distanceFromArrowSound < detectionDistance)
            {
                currentlyBlue = positionTracker.colorsSwapped;
                currentlyYellow = !positionTracker.colorsSwapped;
                newScale = minScale + ((detectionDistance - distanceFromArrowSound) / detectionDistance * (currentMaxScale - minScale));
                newColor = new Color(defaultColor.r + ((currentArrowColor.r - defaultColor.r) * ((detectionDistance - distanceFromArrowSound) / detectionDistance)), defaultColor.g + ((currentArrowColor.g - defaultColor.g) * ((detectionDistance - distanceFromArrowSound) / detectionDistance)), defaultColor.b + ((currentArrowColor.b - defaultColor.b) * ((detectionDistance - distanceFromArrowSound) / detectionDistance)));
            }
            else if (distanceFromWasdSound < detectionDistance)
            {
                currentlyBlue = !positionTracker.colorsSwapped;
                currentlyYellow = positionTracker.colorsSwapped;
                newScale = minScale + ((detectionDistance - distanceFromWasdSound) / detectionDistance * ((currentMaxScale - minScale)));
                newColor = new Color(defaultColor.r - ((defaultColor.r - currentWasdColor.r) * ((detectionDistance - distanceFromWasdSound) / detectionDistance)), defaultColor.g + ((currentWasdColor.g - defaultColor.g) * ((detectionDistance - distanceFromWasdSound) / detectionDistance)), defaultColor.b + ((currentWasdColor.b - defaultColor.b) * ((detectionDistance - distanceFromWasdSound) / detectionDistance)));
            } else
            {
                currentlyBlue = false;
                currentlyYellow = false;
            }
        }


        transform.localScale = new Vector3(transform.localScale.x, newScale, transform.localScale.z);
        GetComponentInChildren<SpriteRenderer>().color = newColor;
    }
}
