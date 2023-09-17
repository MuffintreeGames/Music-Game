using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class Visualization : MonoBehaviour
{
    public static AudioSource audioSource;
    float[] samples = new float[512];
    public static float[] freqBand = new float[12];
    public static float[] bandBuffer = new float[12];
    float[] bufferDecrease = new float[12];

    float[] freqBandHighest = new float[12];
    public static float[] audioBand = new float[12];
    public static float[] audioBandBuffer = new float[12];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAudioSource();
        if (audioSource == null) return;
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
    }

    public static void UpdateAudioSource()
    {
        audioSource = MusicController.currentAudioSource;
    }

    void CreateAudioBands()
    {
        for (int g = 0; g < 12; ++g)
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
        //audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman); // USE ME
        audioSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
    }

    void BandBuffer()
    {
        for (int g = 0; g < 12; ++g)
        {
            if (freqBand[g] > bandBuffer[g]) {
                bandBuffer[g] = freqBand[g];
                bufferDecrease[g] = 0.02f; // magic number fuck fuck fuck
            }

            if (freqBand[g] < bandBuffer[g])
            {
                bufferDecrease[g] = (bandBuffer[g] - freqBand[g]) / 16;
                bandBuffer[g] -= bufferDecrease[g];
                //bandBuffer[g] -= bufferDecrease[g];
                //bufferDecrease[g] *= 1.2f; more magic fuck fuck fuck
            }
        }
    }


    void MakeFrequencyBands()
    {


        int count = 0;
        int[] samplesXBand = { 2, 4, 8, 8, 8, 16, 16, 32, 32, 64, 64, 258 };
        for (int i = 0; i < 12; ++i)
        {
            float average = 0;
            int sampleCount = samplesXBand[i];

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
