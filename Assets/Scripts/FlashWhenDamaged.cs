using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashWhenDamaged : MonoBehaviour
{
    static float flashTime = .1f;  //time to go from 0 opacity to max, same time is then used to go back again
    static float maxOpacity = 0.5f;

    float rateOfChange;
    bool flashing = false;  //true when getting more opaque
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        HealthTracker.playerDamage.AddListener(StartFlashing);
        rateOfChange = maxOpacity / flashTime;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float newAlpha = spriteRenderer.color.a;
        if (flashing)
        {
            newAlpha = Mathf.Min(newAlpha + rateOfChange * Time.deltaTime, maxOpacity);
            if (newAlpha == maxOpacity)
            {
                flashing = false;
            }
        } else
        {
            newAlpha = Mathf.Max(newAlpha - rateOfChange * Time.deltaTime, 0f);
        }
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newAlpha);
    }

    void StartFlashing()
    {
        Debug.Log("trying to flash!");
        flashing = true;
    }
}
