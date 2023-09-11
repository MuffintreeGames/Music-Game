using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamVisual : MonoBehaviour
{
    public int band;
    public bool enableX, enableY = false;
    public float startXScale, scaleXMultiplier, startYScale, scaleYMultiplier;
    public bool enableColor = false;
    public SpriteRenderer sprite;
    private float red, green, blue;
    public bool useBuffer;
    // Start is called before the first frame update
    void Start()
    {
        red = sprite.color.r;
        green = sprite.color.g;
        blue = sprite.color.b;

    }

    // Update is called once per frame
    void Update()
    {
        if (useBuffer)
        {
            transform.localScale = new Vector3(
                        enableX ? (Visualization.bandBuffer[band] * scaleXMultiplier) + startXScale : transform.localScale.x,
                        enableY ? (Visualization.bandBuffer[band] * scaleYMultiplier) + startYScale : transform.localScale.y,
                        transform.localScale.z);
            if (enableColor) sprite.color = new Color(1f, 0.5f - 0.5f * Visualization.audioBandBuffer[band], 0f, Math.Max(Visualization.audioBandBuffer[band], 0.2f));
        }
        if (!useBuffer)
        {
            transform.localScale = new Vector3(
                        enableX ? (Visualization.freqBand[band] * scaleXMultiplier) + startXScale : transform.localScale.x,
                        enableY ? (Visualization.freqBand[band] * scaleYMultiplier) + startYScale : transform.localScale.y,
                        transform.localScale.z);
            if (enableColor) sprite.color = new Color(red, green, blue, Math.Max(Visualization.freqBand[band], 0.2f));

        }
    }
}
