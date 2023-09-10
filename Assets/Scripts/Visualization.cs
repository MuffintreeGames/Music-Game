using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class Visualization : MonoBehaviour
{
    AudioSource audioSource;
    float[] samples = new float[512];
    public static float[] freqBand = new float[8];
    public static float[] bandBuffer = new float[8];
    float[] bufferDecrease = new float[8];

    float[] freqBandHighest = new float[8];
    public static float[] audioBand = new float[8];
    public static float[] audioBandBuffer = new float[8];

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
    }

    void CreateAudioBands()
    {
        for (int g = 0; g < 8; ++g)
        {
            if (freqBand[g] > freqBandHighest[g])
            {
                freqBandHighest[g] = freqBand[g];
            }
            if (freqBandHighest[g] != 0)
            {
                audioBand[g] = freqBand[g] / freqBandHighest[g];
                audioBandBuffer[g] = bandBuffer[g] / freqBandHighest[g];
            }
    }
}

    void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    void BandBuffer()
    {
        for (int g = 0; g < 8; ++g)
        {
            if (freqBand[g] > bandBuffer[g]) {
                bandBuffer[g] = freqBand[g];
                bufferDecrease[g] = 0.2f; // magic number fuck fuck fuck
            }

            if (freqBand[g] < bandBuffer[g])
            {
                bufferDecrease[g] = (bandBuffer[g] - freqBand[g]) / 8;
                bandBuffer[g] -= bufferDecrease[g];
                //bandBuffer[g] -= bufferDecrease[g];
                //bufferDecrease[g] *= 1.2f; more magic fuck fuck fuck
            }
        }
    }
    void MakeFrequencyBands()
    {
        int count = 0;

        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if (i == 7)
            {
                sampleCount += 2;
            }
            for (int j = 0; j < sampleCount; j++)
            {
                average += samples[count] * (count + 1);
                count++;
            }
            average /= count;
            freqBand[i] = average * 10;
        }
    }

}
