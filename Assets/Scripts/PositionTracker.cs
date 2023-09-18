using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PositionTracker : MonoBehaviour
{
    public GameObject blast;

    public GameObject blueSwapParticle;
    public GameObject yellowSwapParticle;

    public float wasdSoundPosition = -5f;   //position of the soundwave controlled with a/d, starts on left
    public float arrowSoundPosition = 5f;   //position of the soundwave controlled with arrows, starts on right
    public bool colorsSwapped = false;
    public float colorTimer = 0.2f;

    static float soundMovementSpeed = 12f;
    static float leftBound = -8.5f;
    static float rightBound = 8.5f;
    static float blastAllowance = 0.2f; //range at which the soundwaves will trigger a blast
    static float blastCooldown = 0.1f; //delay until second blast can happen
    static float blastImpulse = 8f;
    static float blastDecel = 14f;
    static float blastStopSpeed = 2f;   //when blast velocity reaches this speed, allow control to resume
    static float blastHeight = 0f;

    float remainingBlastCooldown = 0f;
    float currentBlastVelocity = 0f;
    bool canControl = true;

    bool deathPause = false;

    
    // Start is called before the first frame update
    void Start()
    {
        wasdSoundPosition = -5f;
        arrowSoundPosition = 5f;
        HealthTracker.playerDeath.AddListener(PauseOnDeath);
        HealthTracker.songReset.AddListener(ResetPositions);
    }

    // Update is called once per frame
    void Update()
    {
        if (deathPause)
        {
            return;
        }

        if (remainingBlastCooldown > 0f)
        {
            remainingBlastCooldown -= Time.deltaTime;
        }
        
        UpdateWavePositions();
        ApplyBlastDeceleration();
        CheckForBlast();


        colorTimer -= Time.deltaTime;
        if (Input.GetButtonDown("ColorSwap"))
        {
            Debug.Log("swapping colors!");
            SwapColors();
        }
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
        Instantiate(blast, new Vector2(wasdSoundPosition + (wasdSoundPosition - arrowSoundPosition)/2, blastHeight), Quaternion.identity);
    }

    void SwapColors()
    {
        if (colorTimer <= 0)
        {
            //colorsSwapped = !colorsSwapped;
            colorTimer = 0.2f;
            if (!colorsSwapped) {
                GameObject newBlue = Instantiate(blueSwapParticle, new Vector2(wasdSoundPosition, -3.8f), Quaternion.identity);
                newBlue.GetComponent<SwapParticle>().xDirection = 1f;
                newBlue.GetComponent<SwapParticle>().goalX = arrowSoundPosition;
                GameObject newYellow = Instantiate(blueSwapParticle, new Vector2(arrowSoundPosition, -3.8f), Quaternion.identity);
                newYellow.GetComponent<SwapParticle>().xDirection = -1f;
                newYellow.GetComponent<SwapParticle>().goalX = wasdSoundPosition;
            } else
            {
                GameObject newBlue = Instantiate(blueSwapParticle, new Vector2(arrowSoundPosition, -3.8f), Quaternion.identity);
                newBlue.GetComponent<SwapParticle>().xDirection = -1f;
                newBlue.GetComponent<SwapParticle>().goalX = wasdSoundPosition;
                GameObject newYellow = Instantiate(blueSwapParticle, new Vector2(wasdSoundPosition, -3.8f), Quaternion.identity);
                newYellow.GetComponent<SwapParticle>().xDirection = 1f;
                newYellow.GetComponent<SwapParticle>().goalX = arrowSoundPosition;
            }
        }
    }

    void PauseOnDeath()
    {
        deathPause = true;
    }

    void ResetPositions()
    {
        wasdSoundPosition = -5f;
        arrowSoundPosition = 5f;
        colorsSwapped = false;
        deathPause = false;
    }
}
