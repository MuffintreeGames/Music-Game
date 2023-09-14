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
                Vector3 targetScale;
                if (enableLeaderboard)
                {
                    targetScale = new Vector3(1.5f, 1.5f, transform.localScale.z);
                    // call leaderboard code and update child text object.
                    return;
                } else
                {
                    float average = 0;
                    for (int g = 0; g < 12; ++g)
                    {
                        average += Visualization.bandBuffer[g];
                    }
                    average /= 12;
                    targetScale = new Vector3(
                        (average * scaleXMultiplier) + startXScale,
                        (average * scaleYMultiplier) + startYScale,
                        transform.localScale.z);
                }

                transform.localScale = Vector3.MoveTowards(
                transform.localScale,
                targetScale,
                Time.deltaTime * 30f);
                sprite.color = new Color(red, green, blue, 0.1f);
                return;
            }

            /*transform.localScale = new Vector3(
                enableX ? (Visualization.audioBandBuffer[band] * scaleXMultiplier) + startXScale : transform.localScale.x,
                enableY ? (Visualization.audioBandBuffer[band] * scaleYMultiplier) + startYScale : transform.localScale.y,
                transform.localScale.z);*/

            transform.localScale = new Vector3(
                enableX ? (Visualization.bandBuffer[band] * scaleXMultiplier) + startXScale : transform.localScale.x,
                enableY ? (Visualization.bandBuffer[band] * scaleYMultiplier) + startYScale : transform.localScale.y,
                transform.localScale.z);

            //if (enableColor) sprite.color = new Color(1f, 0.5f - 0.5f * Visualization.audioBandBuffer[band], 0f, Math.Max(Math.Min(Visualization.audioBandBuffer[band], 0.3f), 0.05f));
            //else sprite.color = new Color(red, green, blue, Math.Max(Math.Min(Visualization.audioBandBuffer[band], 0.3f), 0.05f));
            if (enableColor) sprite.color = new Color(1f, 0.7f - 0.7f * Visualization.audioBandBuffer[band], 0f, 0.2f);
            else sprite.color = new Color(red, green, blue, 0.2f);
        }
        if (!useBuffer)
        {
            transform.localScale = new Vector3(
                        enableX ? (Visualization.freqBand[band] * scaleXMultiplier) + startXScale : transform.localScale.x,
                        enableY ? (Visualization.freqBand[band] * scaleYMultiplier) + startYScale : transform.localScale.y,
                        transform.localScale.z);
            sprite.color = new Color(red, green, blue, Math.Max(Math.Min(Visualization.audioBandBuffer[band], 0.3f), 0.05f));

        }
    }
}
