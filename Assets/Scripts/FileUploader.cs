using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

// Helper component to get a file
public class FileUploader : MonoBehaviour
{
    string storedPath = "";
    float storedSongLength = 0f;
    private void Start()
    {
        // We don't need to delete it on the new scene, because system is singleton
        DontDestroyOnLoad(gameObject);
    }

    // This method is called from JS via SendMessage
    void FileRequestCallback(string path)
    {
        Debug.Log("first callback called");
        storedPath = path;
        if (storedSongLength > 0f)
        {
            // Sending the received link back to the FileUploaderHelper
            FileUploaderHelper.SetResult(storedPath, storedSongLength);
            storedPath = "";
            storedSongLength = 0f;
        } else
        {
            Debug.Log("stored length not set");
        }
    }

    void FileRequestCallback2(float songLength)
    {
        Debug.Log("second callback called");
        storedSongLength = songLength;
        if (storedPath != "")
        {
            // Sending the received link back to the FileUploaderHelper
            FileUploaderHelper.SetResult(storedPath, storedSongLength);
            storedPath = "";
            storedSongLength = 0f;
        } else
        {
            Debug.Log("stored path not set");
        }
    }
}

public static class FileUploaderHelper
{
    static FileUploader fileUploaderObject;
    static Action<string, float> pathCallback;
    static Action<byte[]> dataCallback;

    static FileUploaderHelper()
    {
        string methodName = "FileRequestCallback"; // We will not use reflection, so as not to complicate things, hardcode :)
        string methodName2 = "FileRequestCallback2";
        string objectName = typeof(FileUploaderHelper).Name; // But not here

        // Create a helper object for the FileUploader system
        var wrapperGameObject = new GameObject(objectName, typeof(FileUploader));
        fileUploaderObject = wrapperGameObject.GetComponent<FileUploader>();

        // Initializing the JS part of the FileUploader system
        InitFileLoader(objectName, methodName, methodName2);
    }

    /// <summary>
    /// Requests a file from the user.
    /// Should be called when the user clicks!
    /// </summary>
    /// <param name="callback">Will be called after the user selects a file, the Http path to the file is passed as a parameter</param>
    /// <param name="extensions">File extensions that can be selected, example: ".jpg, .jpeg, .png"</param>
    public static void RequestFile(Action<string, float> callback, string extensions = ".mp3")
    {
        RequestUserFile(extensions);
        pathCallback = callback;
    }

    /// <summary>
    /// For internal use
    /// </summary>
    /// <param name="path">The path to the file</param>
    public static void SetResult(string path, float songLength)
    {
        pathCallback.Invoke(path, songLength);
        Dispose();
    }

    /// <summary>
    /// For internal use
    /// </summary>
    /// <param name="data">The data of the file</param>
    public static void SetResult2(byte[] data)
    {
        dataCallback.Invoke(data);
        Dispose();
    }

    private static void Dispose()
    {
        ResetFileLoader();
        pathCallback = null;
        
    }

    // Below we declare external functions from our .jslib file
    [DllImport("__Internal")]
    private static extern void InitFileLoader(string objectName, string methodName, string methodName2);

    [DllImport("__Internal")]
    private static extern void RequestUserFile(string extensions);

    [DllImport("__Internal")]
    private static extern void ResetFileLoader();
}
