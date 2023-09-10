using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Light))]
public class LightVisual : MonoBehaviour
{
    public int band;
    public float minIntensity, maxIntensity;
    Light lightEffect;
    // Start is called before the first frame update
    void Start()
    {
        lightEffect = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        lightEffect.intensity = (Visualization.audioBandBuffer[band] * (maxIntensity - minIntensity)) + minIntensity;
        //lightEffect.color = new Color(1f, 0.5f - 0.5f * Visualization.audioBandBuffer[band], 0f, Math.Max(Visualization.audioBandBuffer[band], 0.2f));
        lightEffect.color = new Color(1f, 0.5f - 0.5f * Visualization.audioBandBuffer[band], 0f);
    }
}
