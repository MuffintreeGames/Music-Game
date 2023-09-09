using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blast : MonoBehaviour
{
    static float lifespan = 0.5f;
    static float initialScale = 4f;
    static float finalScale = 8f;

    float timeLeft = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        lifespan = timeLeft;
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            Destroy(gameObject);
        } else
        {
            float currentScale = initialScale + (finalScale - initialScale) * (lifespan - timeLeft) / lifespan;
            transform.localScale = new Vector2(currentScale, currentScale);
        }
    }
}
