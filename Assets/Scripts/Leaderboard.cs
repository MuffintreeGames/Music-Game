using Dan.Main;
using Dan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public static bool needReload = false;
        public static GameObject board;
        private static LeaderboardReference selectedLeaderboard = LocalLeaderboards.TestLeaderboard;
        private static LeaderboardReference selectedLeaderboard2 = LocalLeaderboards.TestLeaderboard;

        private Entry myEntry;
        private Entry[] board1;
        private Entry[] board2;
        private Entry[] finalboard;

    private void Start()
        {
            needLoad = false;
            needReload = false;
            board = GameObject.Find("ScrollView");
            playerField.text = "Thanks for Playing!";
            MakeInvisible();
            Load();
        }

    private void Update()
    {
        if (needLoad)
        {
            FinalLeaderboard();
            MakeVisible();
            needLoad = false;
        }
        if (needReload)
        {
            playerField.text = "Thanks for Playing!";
            needReload = false;
            MakeInvisible();
            Load();
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

    public static void ReloadStatic()
    {
        needReload = true;
    }

    //needs to be generalized for all leaderboards
    public void Load()
    {
        for (int j = 0; j < entryFields.Length; j++)
        {
            entryFields[j].text = "";
        }
        selectedLeaderboard.GetEntries(OnLeaderboardLoaded);
    }

    private void OnLeaderboardLoaded(Entry[] entries)
        {
            board1 = entries;
            selectedLeaderboard2.GetEntries(OnLeaderboardLoaded2);
    }

    private void OnLeaderboardLoaded2(Entry[] entries)
    {
        board2 = entries;
        BuildLeaderboard();

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

    public void LogScore()
    {
        Debug.Log("LogScore: " + PointTracker.points);
        if (PointTracker.points < myEntry.Score) { return; }
        if (board1.Length < 99)
        {
            selectedLeaderboard.UploadNewEntry(NameHolder.username, PointTracker.points, System.DateTime.Now.ToString(), Callback, ErrorCallback);
            myEntry.Score = PointTracker.points;
        }
        else if (board2.Length < 99)
        {
            selectedLeaderboard2.UploadNewEntry(NameHolder.username, PointTracker.points, System.DateTime.Now.ToString(), Callback, ErrorCallback);
            myEntry.Score = PointTracker.points;
        }
    }

      public void BuildLeaderboard()
      {
        finalboard = board1.Concat(board2).OrderByDescending(item => item.Score).ToArray();
        int i = 0;
        while (i < Mathf.Min(entryFields.Length, finalboard.Length))
        {
            entryFields[i].text = $"{RankSuffixLocal(i+1)}. {finalboard[i].Username} : {finalboard[i].Score}";
            i++;
        }
            selectedLeaderboard.GetPersonalEntry(UpdateLeaderboard);
      }

         private void UpdateLeaderboard(Entry entry)
         {
            myEntry = entry;
            if (myEntry.Rank == 0)
            {
                if (finalboard.Length < entryFields.Length) entryFields[finalboard.Length].text = $"{RankSuffixLocal(finalboard.Length+1)}. {NameHolder.username} : TBD";
                else playerField.text = $"{RankSuffixLocal(entryFields.Length+1)}. {NameHolder.username} : TBD";
            } else
            {
                if (myEntry.Rank > 8) playerField.text = $"{RankSuffixLocal(Array.IndexOf(finalboard, myEntry) +1)}. {myEntry.Username} : {myEntry.Score}";
                else playerField.text = "Congrats on making top 8!";
         }
         }

        private void FinalLeaderboard()
        {
        /*for (int j = 0; j < entryFields.Length; j++)
        {
            entryFields[j].text = "";
        }*/
            Debug.Log("FinalLeaderboard");
            int i = 0;
            while (i < finalboard.Length)
            {
                //if (finalboard[i].Score > PointTracker.points && i < 8) entryFields[i].text = $"{RankSuffixLocal(i + 1)}. {finalboard[i].Username} : {finalboard[i].Score}";
                if (finalboard[i].Score < PointTracker.points) break;
                i++;
            }
            playerField.text = $"Result: { RankSuffixLocal(i + 1)}. { NameHolder.username} : { PointTracker.points}";
            if (PointTracker.points > myEntry.Score) playerField.text = $"New Highscore: { RankSuffixLocal(i + 1)}. { NameHolder.username} : { PointTracker.points}";
            LogScore();
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
