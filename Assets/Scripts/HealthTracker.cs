using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTracker : MonoBehaviour
{
    public static float publicHealth = 100f;
    static float internalHealth = 100f;

    static float damageTakenPerHit = 25f;
    static float timeToStartHealing = 6f;
    static float rateOfHealing = 18f;   //health regained per second once started healing

    static float timeLeftToHeal = 0f;
    static float rateOfPublicChange = 50f;
    // Start is called before the first frame update
    void Start()
    {
        publicHealth = 100f;
        internalHealth = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeftToHeal > 0f)
        {
            timeLeftToHeal -= Time.deltaTime;
        } else if (publicHealth < 100f)
        {
            internalHealth += rateOfHealing * Time.deltaTime;
            internalHealth = Mathf.Min(100f, internalHealth);
        }

        if (publicHealth != internalHealth)
        {
            publicHealth += Mathf.Clamp(internalHealth-publicHealth, -rateOfPublicChange * Time.deltaTime, rateOfPublicChange * Time.deltaTime);
        }
    }

    public static void TakeDamage()
    {
        Debug.Log("taking damage!");
        internalHealth -= damageTakenPerHit;
        if (internalHealth <= 0)
        {
            Debug.Log("deadge");
        }
        timeLeftToHeal = timeToStartHealing;
    }
}
