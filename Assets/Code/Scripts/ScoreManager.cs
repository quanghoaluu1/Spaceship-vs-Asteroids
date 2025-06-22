using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int score;
    public TextMeshProUGUI scoreText;

    void Start()
    {
        UpdateScoreUI();
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // dòng này để giữ lại object khi chuyển scene
        }
        else
        {
            Destroy(gameObject); // tránh trùng lặp
        }
        if (PlayerPrefs.GetInt("HasSaved", 0) == 1)
        {
            Debug.Log("Saved score: " + PlayerPrefs.GetInt("SavedScore", 9999));
            score = PlayerPrefs.GetInt("SavedScore", 999);
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
