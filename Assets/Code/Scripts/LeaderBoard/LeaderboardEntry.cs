

namespace Assets.Code.Scripts.LeaderBoard
{
    [System.Serializable]
    public class LeaderboardEntry
    {
        public string playerName;
        public int score;
        public float timeSurvived;
        public string mode;    // Normal, Hard, Nightmare
    }
}
