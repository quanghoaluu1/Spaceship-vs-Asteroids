using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int score = 0;
    public TextMeshProUGUI scoreText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 👈 dòng này để giữ lại object khi chuyển scene
        }
        else
        {
            Destroy(gameObject); // tránh trùng lặp
        }

    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
        //UpdateBackground();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }


}
