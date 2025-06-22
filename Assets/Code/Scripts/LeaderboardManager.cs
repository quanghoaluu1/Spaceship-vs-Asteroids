using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Scripts
{
    [System.Serializable]
    public class ScoreEntry
    {
        public string name;
        public int score;
        public float time;
    }

    public class LeaderboardManager : MonoBehaviour
    {
        private string filePath;
        public List<ScoreEntry> entries = new List<ScoreEntry>();

        private void Awake()
        {
            filePath = Application.persistentDataPath + "/leaderboard.json";
            LoadLeaderboard();
        }

        public void LoadLeaderboard()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                entries = JsonUtilityWrapper.FromJson<ScoreEntry>(json);
            }
        }

        public void SaveLeaderboard()
        {
            string json = JsonUtilityWrapper.ToJson(entries);
            File.WriteAllText(filePath, json);
        }

        public void AddEntry(string playerName, int score, float time)
        {
            ScoreEntry entry = new ScoreEntry { name = playerName, score = score, time = time };
            entries.Add(entry);
            entries = entries.OrderByDescending(e => e.score).Take(10).ToList();
            SaveLeaderboard();
        }

        public List<ScoreEntry> GetTopEntries()
        {
            return entries.OrderByDescending(e => e.score).Take(10).ToList();
        }
    }

    // Helper for list-to-json serialization
    public static class JsonUtilityWrapper
    {
        public static string ToJson<T>(List<T> list)
        {
            return JsonUtility.ToJson(new Wrapper<T> { Items = list }, true);
        }

        public static List<T> FromJson<T>(string json)
        {
            return JsonUtility.FromJson<Wrapper<T>>(json).Items;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public List<T> Items;
        }
    }
}
