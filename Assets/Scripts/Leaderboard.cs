using Dan.Main;
using Dan.Models;
using TMPro;
using UnityEngine;


public static class LocalLeaderboards
{
    public static LeaderboardReference TestLeaderboard = new LeaderboardReference("fccf102ec5fa94a9adcc223c87738fa4f9fc8ec4692c8b8e0081953655ede9a7");
    public static LeaderboardReference Song1Leaderboard = new LeaderboardReference("639c1328cc5e1f40ea030ede958de4bb1bdafb5cefab0287638f311b95d56e9b");
    public static LeaderboardReference Song1aLeaderboard = new LeaderboardReference("639c1328cc5e1f40ea030ede958de4bb1bdafb5cefab0287638f311b95d56e9b");
    public static LeaderboardReference Song2Leaderboard = new LeaderboardReference("a166dc3e669f768f5547556ae9fefd694a6dbd40766ecfe447da364bac03e1e0");
    public static LeaderboardReference Song2aLeaderboard = new LeaderboardReference("e759b0ccf3f0124ae78ef9010eaf5065840e53b65be3debb93c0ef772f7d66e2");
    public static LeaderboardReference Song3Leaderboard = new LeaderboardReference("dd6df64fbd56838ebee113cbc64f095e03c9a4d64a876ae8941181307627f21b");
    public static LeaderboardReference Song3aLeaderboard = new LeaderboardReference("dd6df64fbd56838ebee113cbc64f095e03c9a4d64a876ae8941181307627f21b");
}
public class Leaderboard : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] entryFields;
        [SerializeField] private TextMeshProUGUI playerField;

        public static bool needLoad = false;
        public static int score = 0;
        public static GameObject board;
        private static int numEntriesHack = 0;
        private static LeaderboardReference selectedLeaderboard = LocalLeaderboards.TestLeaderboard;
        private static LeaderboardReference selectedLeaderboard2 = LocalLeaderboards.TestLeaderboard;

        private int i, j = 0;
        private static int size1, size2 = 0;

    private void Start()
        {
            board = GameObject.Find("ScrollView");
            playerField.text = "Thanks for Playing!";
            MakeInvisible();
            Load();
        }

    private void Update()
    {
        if (needLoad)
        {
            Load();
            needLoad = false;
        }
    }

    public static void MakeVisible()
        {
            board.SetActive(true);
        }

        public static void MakeInvisible()
        {
            board.SetActive(false);
        }

        public static void selectLeaderboard(int level)
        {
        if (level == 1 || level > 4)
        {
            selectedLeaderboard = LocalLeaderboards.Song1Leaderboard;
            selectedLeaderboard2 = LocalLeaderboards.Song1aLeaderboard;
        }
        if (level == 2) {
            selectedLeaderboard = LocalLeaderboards.Song2Leaderboard;
            selectedLeaderboard2 = LocalLeaderboards.Song2aLeaderboard;
        }
        if (level == 3) {
            selectedLeaderboard = LocalLeaderboards.Song3Leaderboard;
            selectedLeaderboard2 = LocalLeaderboards.Song3aLeaderboard;
        }
        }

    public static void LoadStatic()
    {
        needLoad = true;
    }

    //needs to be generalized for all leaderboards
    public void Load()
    {
        for (j = 0; j < entryFields.Length; j++)
        {
            entryFields[j].text = "";
        }
        selectedLeaderboard.GetEntries(OnLeaderboardLoaded);
    }

    private void OnLeaderboardLoaded(Entry[] entries)
        {
            size1 = entries.Length;
            while (i < Mathf.Min(entryFields.Length, entries.Length))
             {
                entryFields[i].text = $"{entries[i].RankSuffix()}. {entries[i].Username} : {entries[i].Score}";
                i++;
                // parse entries[i].Extra for SongName + ; + Gists Link
             }

            //selectedLeaderboard2.GetEntries(OnLeaderboardLoaded2);
           // numEntriesHack = size1 + size2;
            GetPersonalEntry();
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
        if (entry.Score == 0 || entry.Score < score) selectedLeaderboard.UploadNewEntry(NameHolder.username, PointTracker.points, System.DateTime.Now.ToString(), Callback, ErrorCallback);
    }

      public void GetPersonalEntry()
      {
            selectedLeaderboard.GetPersonalEntry(UpdateLeaderboard);
      }

         private void UpdateLeaderboard(Entry entry)
         {
            if (entry.Rank == 0)
            {
                if (numEntriesHack < entryFields.Length) entryFields[numEntriesHack].text = $"{RankSuffixLocal(numEntriesHack + 1)}. {NameHolder.username} : 0";
                else playerField.text = $"{RankSuffixLocal(9)}. {NameHolder.username} : 0";
            } else
            {
                if (entry.Rank > 8) playerField.text = $"{entry.Rank}. {entry.Username} : {entry.Score}";
                else playerField.text = "Congrats on making top 8!";
         }
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
