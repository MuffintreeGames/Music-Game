using Dan.Main;
using Dan.Models;
using TMPro;
using UnityEngine;


public static class OnlineLeaderboards
{
    // these leaderboards need to be configured with Unique Usernames set to off!
    public static LeaderboardReference Online1 = new LeaderboardReference("fccf102ec5fa94a9adcc223c87738fa4f9fc8ec4692c8b8e0081953655ede9a7");
    public static LeaderboardReference Online2 = new LeaderboardReference("fccf102ec5fa94a9adcc223c87738fa4f9fc8ec4692c8b8e0081953655ede9a7");
}
public class OnlineLevels : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] entryFields;
    [SerializeField] private TextMeshProUGUI playerField;

    private static int levelSelected = 0;
    private int i, j = 0;
    private static int size1, size2 = 0;

    private void Start()
    {
        Load();
    }

    //needs to be generalized for all leaderboards
        public void Load() {
            for (j = 0; j < entryFields.Length; j++)
            {
                entryFields[j].text = "";
            }
            OnlineLeaderboards.Online1.GetEntries(OnLeaderboardLoaded);
        }

        private void OnLeaderboardLoaded(Entry[] entries)
        {
            size1 = entries.Length;
            while (i < Mathf.Min(entryFields.Length, entries.Length))
            {
                entries[i].Extra = "MyTitle;12312-71-295-12245_2452";
                entryFields[i].text = $"{entries[i].Extra.Split(";")[0]} by {entries[i].Username} : {entries[i].Extra.Split(";")[1]}";
                i++;
                // parse entries[i].Extra for SongName + ; + Gists Link
            }
            OnlineLeaderboards.Online2.GetEntries(OnLeaderboardLoaded2);
        }

    private void OnLeaderboardLoaded2(Entry[] entries)
    {
        size2 = entries.Length;
        while (i < Mathf.Min(entryFields.Length, entries.Length + size1))
        {
            entries[i].Username = "abcdefghijkl";
            entries[i].Extra = "MyTitle;12312-71-295-12245_2452";
            entryFields[i].text = $"{entries[i].Extra.Split(";")[0]} by {entries[i].Username} : {entries[i].Extra.Split(";")[1]}";
            i++;
        }
        
    }

    // this likely needs to be ported to LevelEditor or a new class
    public void UploadLevel(string title, string GistNoteMap)
    {
        // call a load before this code to ensure, 99 should also save us from a race condition
        // use score to encode length of song in seconds?
        int score = 180; // 3min
        string extra = string.Concat(title + ";" + GistNoteMap); // these could be static variable calls instead of parameters
        if (size1 < 99)
        {
            OnlineLeaderboards.Online1.UploadNewEntry(NameHolder.username, score, extra, Callback, ErrorCallback);
            OnlineLeaderboards.Online1.ResetPlayer();
        }
        else if (size2 < 99)
        {
            OnlineLeaderboards.Online2.UploadNewEntry(NameHolder.username, score, extra, Callback, ErrorCallback);
            OnlineLeaderboards.Online2.ResetPlayer();
        }
        return;
    }


    public void PlayLevel()
    {
        // load level w/ levelSelected variable (could be guid also)
        MainMenuController.LoadGame();
    }

    public void Refresh()
    {
        Load();
    }


        private void Callback(bool success)
        {
            //if (success)
               // Load();
        }
        
        private void ErrorCallback(string error)
        {
            Debug.LogError(error);
        }
    }
