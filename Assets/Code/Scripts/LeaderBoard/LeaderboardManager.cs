using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Scripts.LeaderBoard
{
    public class LeaderboardManager : MonoBehaviour
    {
        public static LeaderboardManager Instance;

        public List<LeaderboardEntry> leaderboard1 = new List<LeaderboardEntry>();
        public List<LeaderboardEntry> leaderboard2 = new List<LeaderboardEntry>();

        private string lb1Path => Path.Combine(Application.persistentDataPath, "leaderboard1.json");
        private string lb2Path => Path.Combine(Application.persistentDataPath, "leaderboard2.json");

        void Awake()
        {
            if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
            else Destroy(gameObject);

            LoadLeaderboard();
        }

        public void AddEntry(string name, int score, float time, string mode)
        {
            if (mode == "Arcade" || mode == "Normal")
            {
                leaderboard1.Add(new LeaderboardEntry { playerName = name, score = score, mode = mode });
                leaderboard1 = leaderboard1.OrderByDescending(e => e.score).Take(10).ToList();
            }
            else
            {
                leaderboard2.Add(new LeaderboardEntry { playerName = name, score = score, timeSurvived = time, mode = mode });
                leaderboard2 = leaderboard2.OrderBy(e => e.timeSurvived).Take(10).ToList();
            }

            SaveLeaderboard();
        }

        public void SaveLeaderboard()
        {
            File.WriteAllText(lb1Path, JsonUtility.ToJson(new LeaderboardWrapper { list = leaderboard1 }, true));
            File.WriteAllText(lb2Path, JsonUtility.ToJson(new LeaderboardWrapper { list = leaderboard2 }, true));
            Debug.Log("Leaderboard saved to: " + lb1Path);
        }

        public void LoadLeaderboard()
        {
            if (File.Exists(lb1Path))
            {
                string json1 = File.ReadAllText(lb1Path);
                leaderboard1 = JsonUtility.FromJson<LeaderboardWrapper>(json1)?.list ?? new List<LeaderboardEntry>();
            }

            if (File.Exists(lb2Path))
            {
                string json2 = File.ReadAllText(lb2Path);
                leaderboard2 = JsonUtility.FromJson<LeaderboardWrapper>(json2)?.list ?? new List<LeaderboardEntry>();
            }
        }

        public List<LeaderboardEntry> GetTopScores(string mode, int count)
        {
            // Gộp cả hai leaderboard, sau đó lọc theo mode
            var allEntries = leaderboard1.Concat(leaderboard2)
                .Where(e => e.mode == mode)
                .OrderByDescending(e => e.score)
                .ThenBy(e => e.timeSurvived)
                .Take(count)
                .ToList();

            return allEntries;
        }

        [System.Serializable]
        class LeaderboardWrapper
        {
            public List<LeaderboardEntry> list;
        }
    }
}