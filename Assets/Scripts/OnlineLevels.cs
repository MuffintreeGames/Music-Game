using Dan.Main;
using Dan.Models;
using TMPro;
using UnityEngine;


public static class OnlineLeaderboards
{
    public static LeaderboardReference Online1 = new LeaderboardReference("fccf102ec5fa94a9adcc223c87738fa4f9fc8ec4692c8b8e0081953655ede9a7");
}
public class OnlineLevels : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] entryFields;
        [SerializeField] private TextMeshProUGUI playerField;

        private int levelSelected = 0;

        private void Start()
        {
            Load();
        }

        //needs to be generalized for all leaderboards
        public void Load() => OnlineLeaderboards.Online1.GetEntries(OnLeaderboardLoaded);

        private void OnLeaderboardLoaded(Entry[] entries)
        {
            int i, j;
            for (j = 0; j < entryFields.Length; j++)
            {
                entryFields[j].text = "";
            }

            for (i = 0; i < Mathf.Min(entryFields.Length, entries.Length); i++)
            {
                entryFields[i].text = $"{entries[i].RankSuffix()}. {entries[i].Username} : {entries[i].Score}";
            }
        }

    public void PlayLevel(Entry entry)
    {
        //log play if able
        OnlineLeaderboards.Online1.UploadNewEntry(entry.Username, entry.Score++, entry.Extra, Callback, ErrorCallback);
        // load level
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
