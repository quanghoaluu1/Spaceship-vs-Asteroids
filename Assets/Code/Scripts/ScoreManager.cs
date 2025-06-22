using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int score;
    public TextMeshProUGUI scoreText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (PlayerPrefs.GetInt("HasSaved", 0) == 1)
        {
            //Debug.Log("Saved score: " + PlayerPrefs.GetInt("SavedScore", 9999));
            score = PlayerPrefs.GetInt("SavedScore", 999);
        }
    }

    void Start()
    {
        TryReconnectText(); // 👈 gán lại text nếu cần
        UpdateScoreUI();
    }

    void Update()
    {
        if (scoreText == null)
            TryReconnectText();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
    }

    void TryReconnectText()
    {
        // Gán lại nếu chuyển scene bị mất text
        if (SceneManager.GetActiveScene().name == "Gameplay") // 👈 đổi theo tên scene gameplay bạn dùng
        {
            GameObject textObj = GameObject.Find("ScoreText"); // 👈 đảm bảo tên này đúng
            if (textObj != null)
            {
                scoreText = textObj.GetComponent<TextMeshProUGUI>();
            }
        }
    }
}
