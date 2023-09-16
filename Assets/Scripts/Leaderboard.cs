using Dan.Main;
using Dan.Models;
using TMPro;
using UnityEngine;


public static class LocalLeaderboards
{
    public static LeaderboardReference TestLeaderboard = new LeaderboardReference("fccf102ec5fa94a9adcc223c87738fa4f9fc8ec4692c8b8e0081953655ede9a7");
    public static LeaderboardReference Song1Leaderboard = new LeaderboardReference("639c1328cc5e1f40ea030ede958de4bb1bdafb5cefab0287638f311b95d56e9b");
    public static LeaderboardReference Song2Leaderboard = new LeaderboardReference("a166dc3e669f768f5547556ae9fefd694a6dbd40766ecfe447da364bac03e1e0");
    public static LeaderboardReference Song3Leaderboard = new LeaderboardReference("e759b0ccf3f0124ae78ef9010eaf5065840e53b65be3debb93c0ef772f7d66e2");
    public static LeaderboardReference Song4Leaderboard = new LeaderboardReference("dd6df64fbd56838ebee113cbc64f095e03c9a4d64a876ae8941181307627f21b");
}
public class Leaderboard : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] entryFields;
        [SerializeField] private TextMeshProUGUI playerField;

        public string playerName;
        public int score = 9999999;
        public GameObject board;
        private static int numEntriesHack = 0;
        private LeaderboardReference selectedLeaderboard = LocalLeaderboards.Song1Leaderboard;

        private void Start()
        {
            MakeInvisible();
            //selectedLeaderboard.GetPersonalEntry(LogScore);
            Load();
        }

        public void MakeVisible()
        {
          board.SetActive(true);
        }

        public void MakeInvisible()
        {
            board.SetActive(false);
        }

        public void selectLeaderboard(int level)
        {
            if (level == 1 || level > 4) selectedLeaderboard = LocalLeaderboards.Song1Leaderboard;
            if (level == 2) selectedLeaderboard = LocalLeaderboards.Song2Leaderboard;
            if (level == 3) selectedLeaderboard = LocalLeaderboards.Song3Leaderboard;
            Load();
        }

        //needs to be generalized for all leaderboards
        public void Load() => selectedLeaderboard.GetEntries(OnLeaderboardLoaded);

        private void OnLeaderboardLoaded(Entry[] entries)
        {
            Debug.LogError("OnLeaderboardLoaded");
             foreach (var entryField in entryFields)
            {
                entryField.text = "";
            }

            int i;
            for (i = 0; i < Mathf.Min(entryFields.Length, entries.Length); i++)
            {
                entryFields[i].text = $"{entries[i].RankSuffix()}. {entries[i].Username} : {entries[i].Score}";
            }

            numEntriesHack = i;
            GetPersonalEntry();
        }

    public string RankSuffixLocal(int rank)
    {
        var lastDigit = rank % 10;
        var lastTwoDigits = rank % 100;

        var suffix = lastDigit == 1 && lastTwoDigits != 11 ? "st" :
            lastDigit == 2 && lastTwoDigits != 12 ? "nd" :
            lastDigit == 3 && lastTwoDigits != 13 ? "rd" : "th";

        return $"{rank}{suffix}";
    }

    public void LogScore(Entry entry)
        {
        Debug.LogError("LogScore: " + entry.Score + "<" + score);
        if (entry.Score == 0 || entry.Score < score) selectedLeaderboard.UploadNewEntry(playerName, score, Callback, ErrorCallback);
        }

      public void GetPersonalEntry()
      {
            selectedLeaderboard.GetPersonalEntry(UpdateLeaderboard);
      }

         private void UpdateLeaderboard(Entry entry)
         {
            Debug.LogError("UpdateLeaderboard");
            if (entry.Rank == 0)
            {
                if (numEntriesHack < entryFields.Length) entryFields[numEntriesHack].text = $"{RankSuffixLocal(numEntriesHack + 1)}. {playerName} : 0";
                else playerField.text = $"{RankSuffixLocal(9)}. {playerName} : 0";
            } else
            {
                if (entry.Rank > 8) playerField.text = $"{entry.Rank}. {entry.Username} : {entry.Score}";
                else playerField.text = "Congrats on making top 8!";
        }
            MakeVisible();
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
