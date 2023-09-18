using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageEvent : UnityEvent
{

}

public class DeathEvent : UnityEvent
{

}

public class ResetEvent : UnityEvent
{

}

public class HealthTracker : MonoBehaviour
{
    public static float publicHealth = 100f;
    public static DamageEvent playerDamage;
    public static DeathEvent playerDeath;
    public static ResetEvent songReset;
    public GameObject Hexagon;

    static float internalHealth = 100f;

    static float damageTakenPerHit = 15f;
    static float timeToStartHealing = 6f;
    static float rateOfHealing = 18f;   //health regained per second once started healing

    static float timeLeftToHeal = 0f;
    static float rateOfPublicChange = 50f;

    static float pauseTime = 1.5f;
    static float pauseTimeLeft = 0f;

    static float resetTime = 1.5f;
    static float resetTimeLeft = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        publicHealth = 100f;
        internalHealth = 100f;
        if (playerDeath == null)
        {
            playerDamage = new DamageEvent();
        }
        if (playerDeath == null)
        {
            playerDeath = new DeathEvent();
        }
        if (songReset == null)
        {
            songReset = new ResetEvent();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseTimeLeft > 0f)
        {
            pauseTimeLeft -= Time.deltaTime;
            if (pauseTimeLeft <= 0f)
            {
                songReset.Invoke();
                if (Hexagon != null)
                {
                    Hexagon.SetActive(false);
                }
                ResetHealth();
            }
        }
        else
        {
            if (timeLeftToHeal > 0f)
            {
                timeLeftToHeal -= Time.deltaTime;
            }
            else if (publicHealth < 100f)
            {
                internalHealth += rateOfHealing * Time.deltaTime;
                internalHealth = Mathf.Min(100f, internalHealth);
            }
        }

        if (publicHealth != internalHealth)
        {
            publicHealth += Mathf.Clamp(internalHealth - publicHealth, -rateOfPublicChange * Time.deltaTime, rateOfPublicChange * Time.deltaTime);
        }

    }

    void ResetHealth()
    {
        internalHealth = 100f;
        publicHealth = 100f;
    }

    public static void TakeDamage()
    {
        Debug.Log("taking damage!");
        internalHealth -= damageTakenPerHit;
        if (internalHealth <= 0)
        {
            Debug.Log("deadge");
            playerDeath.Invoke();
            pauseTimeLeft = pauseTime;
        } else
        {
            playerDamage.Invoke();
        }
        timeLeftToHeal = timeToStartHealing;
    }
}
