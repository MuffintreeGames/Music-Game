using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dan.Main;

public class ParamVisual : MonoBehaviour
{
    public int band;
    public bool enableX, enableY = false;
    public float startXScale, scaleXMultiplier, startYScale, scaleYMultiplier;
    public bool enableColor = false;
    public bool enableLeaderboard = false;
    public SpriteRenderer sprite;
    private float red, green, blue;
    public bool useBuffer;
    // Start is called before the first frame update
    void Start()
    {
        red = sprite.color.r;
        green = sprite.color.g;
        blue = sprite.color.b;
        enableLeaderboard = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (useBuffer)
        {
            if (band == 12)
            {
                
                if (enableLeaderboard)
                {
                    // can we remove this?
                    //transform.localScale = new Vector3(1.75f, 1.75f, transform.localScale.z);
                    return;
                } else
                {
                    float average = 0;
                    for (int g = 0; g < 12; ++g)
                    {
                        average += Visualization.bandBuffer[g];
                    }
                    average /= 12;

                    Vector3 previousScale2 = transform.localScale;
                    previousScale2.x = enableX ? Mathf.Lerp(previousScale2.x, (average * scaleXMultiplier) + startXScale, 60f * Time.deltaTime) : transform.localScale.x;
                    previousScale2.y = enableY ? Mathf.Lerp(previousScale2.y, (average * scaleYMultiplier) + startYScale, 60f * Time.deltaTime) : transform.localScale.y;
                    //Add delta time please
                    transform.localScale = previousScale2;

                    sprite.color = new Color(red, green, blue, 0.1f);
                    return;
                }
            }

            /*transform.localScale = new Vector3(
                enableX ? (Visualization.audioBandBuffer[band] * scaleXMultiplier) + startXScale : transform.localScale.x,
                enableY ? (Visualization.audioBandBuffer[band] * scaleYMultiplier) + startYScale : transform.localScale.y,
                transform.localScale.z);*/

            //USE ME
            /*transform.localScale = new Vector3(
                enableX ? (Visualization.bandBuffer[band] * scaleXMultiplier) + startXScale : transform.localScale.x,
                enableY ? (Visualization.bandBuffer[band] * scaleYMultiplier) + startYScale : transform.localScale.y,
                transform.localScale.z);*/

            Vector3 previousScale = transform.localScale;
             previousScale.y = enableY ? Mathf.Lerp(previousScale.y, (Visualization.bandBuffer[band] * scaleYMultiplier) + startYScale, 60f * Time.deltaTime) : transform.localScale.y;
             previousScale.x = enableX ? Mathf.Lerp(previousScale.x, (Visualization.bandBuffer[band] * scaleXMultiplier) + startXScale, 60f * Time.deltaTime) : transform.localScale.x;
            //Add delta time please
            transform.localScale = previousScale;

            //if (enableColor) sprite.color = new Color(1f, 0.5f - 0.5f * Visualization.audioBandBuffer[band], 0f, Math.Max(Math.Min(Visualization.audioBandBuffer[band], 0.3f), 0.05f));
            //else sprite.color = new Color(red, green, blue, Math.Max(Math.Min(Visualization.audioBandBuffer[band], 0.3f), 0.05f));
            if (enableColor) sprite.color = new Color(1f, 0.7f - 0.7f * Visualization.audioBandBuffer[band], 0f, 0.2f);
            else sprite.color = new Color(red, green, blue, 0.2f);
        }
        if (!useBuffer)
        {
            transform.localScale = new Vector3(
                enableX ? (Visualization.audioBandBuffer[band] * scaleXMultiplier) + startXScale : transform.localScale.x,
                enableY ? (Visualization.audioBandBuffer[band] * scaleYMultiplier) + startYScale : transform.localScale.y,
                transform.localScale.z);
            /*
            float max = 0;
            for (int g = 0; g < 12; ++g)
            {
                max = Mathf.Max(Visualization.bandBuffer[g], max);
            }

            Vector3 previousScale2 = transform.localScale;
            previousScale2.x = enableX ? Mathf.Lerp(previousScale2.x, (max * scaleXMultiplier) + startXScale, 60f * Time.deltaTime) : transform.localScale.x;
            previousScale2.y = enableY ? Mathf.Lerp(previousScale2.y, (max * scaleYMultiplier) + startYScale, 60f * Time.deltaTime) : transform.localScale.y;
            //Add delta time please
            transform.localScale = previousScale2;*/

            if (enableColor) sprite.color = new Color(1f, 0.7f - 0.7f * Visualization.bandBuffer[band], 0f, 0.2f);
            else sprite.color = new Color(red, green, blue, 0.2f);
        }
    }
}
