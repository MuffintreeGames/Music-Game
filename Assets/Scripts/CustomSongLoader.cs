using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CustomSongLoader : MonoBehaviour
{
    public static AudioClip song = null;
    public static float correctSongLength = 0;

    string storedPath = "";
    float storedSongLength = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectSong()
    {
        // Requesting a file from the user
        FileUploaderHelper.RequestFile((path, songLength) =>
        {
            // If the path is empty, ignore it.
            if (string.IsNullOrWhiteSpace(path))
            {
                Debug.Log("empty path");
                return;
            }

            storedPath = path;
            storedSongLength = songLength;
            // Run a coroutine to load an image
            StartCoroutine(UploadSong(storedPath, storedSongLength));
        });
    }

    IEnumerator UploadSong(string uri, float songLength)
    {
        //statusText.text = "Loading...";
        UnityWebRequest www = UnityWebRequest.Get(uri);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            byte[] results = www.downloadHandler.data;
            var memStream = new System.IO.MemoryStream(results);
            var mpgFile = new NLayer.MpegFile(memStream);
            var samples = new float[mpgFile.Length];
            mpgFile.ReadSamples(samples, 0, (int)mpgFile.Length);


            var clip = AudioClip.Create("foo", samples.Length, mpgFile.Channels, mpgFile.SampleRate, false);
            clip.SetData(samples, 0);

            song = clip;
            Debug.Log("length of samples = " + songLength);

            correctSongLength = songLength;
            SceneManager.LoadScene("LevelEditor");
        }
    }
}
