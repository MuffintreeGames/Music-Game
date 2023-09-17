using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static Unity.VisualScripting.Member;
using NLayer;

public class SongHolder : MonoBehaviour
{

    public static AudioClip song = null;
    public static float correctSongLength = 0;

    string storedPath = "";
    float storedSongLength = 0;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ConfirmSong()
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
            StartCoroutine(UploadSong4(storedPath, storedSongLength));
        });
    }

    /*IEnumerator UploadSong2(string path)
    {
        AudioClip uploadedClip = null;
        using (UnityWebRequest soundWeb = new UnityWebRequest(path, UnityWebRequest.kHttpVerbGET))
        {
            // We create a "downloader" for textures and pass it to the request
            soundWeb.downloadHandler = new DownloadHandlerAudioClip(path, AudioType.MPEG);

            // We send a request, execution will continue after the entire file have been downloaded
            yield return soundWeb.SendWebRequest();

            // Getting the texture from the "downloader"
            uploadedClip = ((DownloadHandlerAudioClip)soundWeb.downloadHandler).audioClip;
            Debug.Log("version 2: at path " + path + ", found song with length " + uploadedClip.length);
        }
    }*/

    // Coroutine for image upload
    IEnumerator UploadSong(string path)
    {
        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG);
        yield return www.SendWebRequest();


        Debug.Log(www.result);
        song = DownloadHandlerAudioClip.GetContent(www);
        if (song == null)
        {
            Debug.LogError("returned song was null");
        }
        Debug.Log("version 1: at path " + path + ", found song with length " + song.length);
        //SceneManager.LoadScene("LevelEditor");
    }

    IEnumerator UploadSong2(string path)
    {
        using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG);
        DownloadHandlerAudioClip dHA = new DownloadHandlerAudioClip(string.Empty, AudioType.MPEG);
        dHA.streamAudio = true;
        www.downloadHandler = dHA;
        www.SendWebRequest();
        while (www.downloadProgress < 1)
        {
            Debug.Log(www.downloadProgress);
            yield return new WaitForSeconds(.1f);
        }
        if (www.responseCode != 200 || www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("error");
        }
        else
        {
            song = DownloadHandlerAudioClip.GetContent(www);
            Debug.Log("version 2: found song with length " + song.length);
            //audioSource.Play();
        }

    }

    IEnumerator UploadSong3(string path)
    {
        UnityWebRequest www = UnityWebRequest.Get(path);
        yield return www.SendWebRequest();


        Debug.Log(www.result);
        //AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
        
        Debug.Log("version 3: at path " + path + ", found data containing " + www.downloadHandler.data);
        //song.SetData(www.downloadHandler.data, 0);
        /*song = DownloadHandlerAudioClip.GetContent(www);
        if (song == null)
        {
            Debug.LogError("returned song was null");
        }
        Debug.Log("version 3: at path " + path + ", found song with length " + song.length);*/
    }

    IEnumerator UploadSong4(string uri, float songLength)
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
