using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private GameData gameData;

    int maxEntries = 20;

    public void SetCurrentProfile(string newPlayName)
    {
        int _score = 0;

        gameData.UpdateCurrentProfile(newPlayName, _score);
        SaveGameData();
    }

    public void SaveGameData()
    {
        string json = JsonUtility.ToJson(gameData, true);
        string path2 = Path.Combine(Application.persistentDataPath, "GameData.json");

        File.WriteAllText(path2, json);
    }

    public void LoadGameData()
    {
        string path = Path.Combine(Application.persistentDataPath, "GameData.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            gameData = JsonUtility.FromJson<GameData>(json);
            gameData.SortHighScoresDescending();
        }
    }

    public void AddNewEntryOfHighscores(string playerName, int newScore)
    {
        Array.Resize(ref gameData.playerEntryArray, gameData.playerEntryArray.Length + 1);
        gameData.playerEntryArray[gameData.playerEntryArray.Length - 1] = new PlayerEntry { playerName = playerName, highscore = newScore};
    }

    public void UpdateHighscoreScoreDatabase(string playerName, int newScore)
    {
        bool playerFound = false;

        if (newScore > gameData.currentProfile.highscore)
        {
            gameData.currentProfile.highscore = newScore;
        }

        foreach (var profile in gameData.playerEntryArray)
        {
            if (profile.playerName == gameData.currentProfile.playerName)
            {
                playerFound = true;
                profile.highscore = newScore;
                break;
            }
        }

        if (!playerFound)
        {
            if (gameData.playerEntryArray.Length == maxEntries)
            {
                if (newScore >= gameData.playerEntryArray[maxEntries - 1].highscore)
                {
                    List<PlayerEntry> playerEntryList = gameData.playerEntryArray.ToList();

                    playerEntryList.RemoveAt(maxEntries - 1);

                    PlayerEntry somethingEntry = new PlayerEntry { playerName = playerName, highscore = newScore };

                    playerEntryList.Add(somethingEntry);

                    gameData.playerEntryArray = playerEntryList.ToArray();
                }
            }

            AddNewEntryOfHighscores(playerName, newScore);
            gameData.SortHighScoresDescending();
        }

        SaveGameData();
    }


    [System.Serializable]
    public class PlayerEntry
    {
        public string playerName;
        public int highscore;
    }

    [System.Serializable]
    public class Profile
    {
        public string playerName;
        public int highscore;
    }

    [System.Serializable]
    public class GameData
    {
        public PlayerEntry[] playerEntryArray;
        public Profile currentProfile;

        public void SortHighScoresDescending()
        {
            Array.Sort(playerEntryArray, (x, y) => y.highscore.CompareTo(x.highscore));
        }

        public void UpdateCurrentProfile(string playerName, int score)
        {
            currentProfile.playerName = playerName;
            currentProfile.highscore = score;
        }
    }

    public GameData GetGameData() { return gameData; }

    public Profile GetCurrentProfile() { return gameData.currentProfile; }
}
