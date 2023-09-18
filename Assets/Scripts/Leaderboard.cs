using Dan.Main;
using Dan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public static class LocalLeaderboards
{
    public static LeaderboardReference Song1Leaderboard = new LeaderboardReference("94d544ca9c95f64b2d708fdc2338637f8dfd0de98f039525b98f3ae29e1f14ca");
    public static LeaderboardReference Song1aLeaderboard = new LeaderboardReference("aad90ae6cf5948cff94d7489a241914cf23609cd254998aa87c9ddc9faa25bf0");
    public static LeaderboardReference Song2Leaderboard = new LeaderboardReference("a97cd46e768468844a689c003115575d7fcec06347fcff16e312eadd0facb9ef");
    public static LeaderboardReference Song2aLeaderboard = new LeaderboardReference("3f98b3b5c5548374cc9792773fa729fcdf1ba31b97419dcf4e8ef3b6320eaaff");
    public static LeaderboardReference Song3Leaderboard = new LeaderboardReference("b1f789b7950127d08c044dad397597f70be0d06882df9557963df19a95b47478");
    public static LeaderboardReference Song3aLeaderboard = new LeaderboardReference("68a65a7635a5c071e2529a5e94d2206462963baade3974fd1fd1fa6989e79818");
}
public class Leaderboard : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] entryFields;
        [SerializeField] private TextMeshProUGUI playerField;

        public static bool needLoad = false;
        public static bool needReload = false;
        public static GameObject board;
        private static LeaderboardReference selectedLeaderboard = LocalLeaderboards.Song1Leaderboard;
        private static LeaderboardReference selectedLeaderboard2 = LocalLeaderboards.Song1aLeaderboard;

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
                else playerField.text = "Keep it up!";
            } else
            {
                if (myEntry.Rank > 8) playerField.text = $"{RankSuffixLocal(Array.IndexOf(finalboard, myEntry) +1)}. {myEntry.Username} : {myEntry.Score}";
                else playerField.text = "Keep it up!";
         }
         }

        private void FinalLeaderboard()
        {
            for (int j = 0; j < entryFields.Length; j++)
            {
                entryFields[j].text = "";
            }
            
            //fills top 8
            int i = 0;
            while (i < finalboard.Length && i < 8)
            {
                if (finalboard[i].Score > PointTracker.points) entryFields[i].text = $"{RankSuffixLocal(i + 1)}. {finalboard[i].Username} : {finalboard[i].Score}";
                else if (finalboard[i].Score <= PointTracker.points) 
                {
                    entryFields[i].text = $"{ RankSuffixLocal(i + 1)}. { NameHolder.username} : { PointTracker.points}";
                    i++;
                    while (i < finalboard.Length+1 && i < 8)
                    {
                        entryFields[i].text = $"{RankSuffixLocal(i + 1)}. {finalboard[i - 1].Username} : {finalboard[i - 1].Score}";
                        i++;
                    }
                    break;
                }
                i++;
            }

            // index in total board
            int z = 0;
            while (z < finalboard.Length)
            {
                if (finalboard[z].Score <= PointTracker.points) break;
                z++;
            }

            if (PointTracker.points > myEntry.Score) playerField.text = $"Highscore: { RankSuffixLocal(z + 1)}. { NameHolder.username} : { PointTracker.points}";
            else if (z < 8) playerField.text = $"Placed: { RankSuffixLocal(z + 1)}. { NameHolder.username} : { PointTracker.points}";
            else playerField.text = $"Result: { RankSuffixLocal(z + 1)}. { NameHolder.username} : { PointTracker.points}";
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
