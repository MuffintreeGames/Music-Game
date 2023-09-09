using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTracker : MonoBehaviour
{
    public GameObject blast;

    public float wasdSoundPosition = -5f;   //position of the soundwave controlled with a/d, starts on left
    public float arrowSoundPosition = 5f;   //position of the soundwave controlled with arrows, starts on right

    static float soundMovementSpeed = 12f;
    static float leftBound = -8.5f;
    static float rightBound = 8.5f;
    static float blastAllowance = 0.2f; //range at which the soundwaves will trigger a blast
    static float blastCooldown = 0.25f; //delay until second blast can happen
    static float blastImpulse = 8f;
    static float blastDecel = 10f;
    static float blastStopSpeed = 2f;   //when blast velocity reaches this speed, allow control to resume
    static float blastHeight = 0f;

    float remainingBlastCooldown = 0f;
    float currentBlastVelocity = 0f;
    bool canControl = true;
    // Start is called before the first frame update
    void Start()
    {
        wasdSoundPosition = -5f;
        arrowSoundPosition = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (remainingBlastCooldown > 0f)
        {
            remainingBlastCooldown -= Time.deltaTime;
        }
        
        UpdateWavePositions();
        ApplyBlastDeceleration();
        CheckForBlast();
    }

    void UpdateWavePositions()
    {
        if (canControl)
        {
            float arrowAxis = Input.GetAxis("ArrowMovement");
            arrowSoundPosition += arrowAxis * soundMovementSpeed * Time.deltaTime;

            float wasdAxis = Input.GetAxis("WASDMovement");
            wasdSoundPosition += wasdAxis * soundMovementSpeed * Time.deltaTime;

        } else
        {
            arrowSoundPosition += currentBlastVelocity * Time.deltaTime;
            wasdSoundPosition -= currentBlastVelocity * Time.deltaTime;
        }

        if (wasdSoundPosition > rightBound)
        {
            wasdSoundPosition = rightBound;
        }
        else if (wasdSoundPosition < leftBound)
        {
            wasdSoundPosition = leftBound;
        }

        if (arrowSoundPosition > rightBound)
        {
            arrowSoundPosition = rightBound;
        }
        else if (arrowSoundPosition < leftBound)
        {
            arrowSoundPosition = leftBound;
        }
    }

    void ApplyBlastDeceleration()
    {
        currentBlastVelocity -= blastDecel * Time.deltaTime;
        if (currentBlastVelocity <= blastStopSpeed)
        {
            canControl = true;
        }
    }

    void CheckForBlast()
    {
        if (Mathf.Abs(arrowSoundPosition - wasdSoundPosition) < blastAllowance && remainingBlastCooldown <= 0)
        {
            TriggerBlast();
        }
    }

    void TriggerBlast()
    {
        remainingBlastCooldown = blastCooldown;
        Debug.Log("BOOM!");
        currentBlastVelocity = blastImpulse;
        canControl = false;
        GameObject.Instantiate(blast, new Vector2(wasdSoundPosition + (wasdSoundPosition - arrowSoundPosition)/2, blastHeight), Quaternion.identity);
    }
}
