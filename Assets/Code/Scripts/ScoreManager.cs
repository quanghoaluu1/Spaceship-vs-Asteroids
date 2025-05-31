using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static ScoreManager Instance;

    public int score = 0;
    public TMP_Text scoreText;
    public bool gameStarted = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        if (scoreText != null)
            scoreText.gameObject.SetActive(true);
    }

    public void AddScore(int amount)
    {
        if (!gameStarted) return;

        score += amount;
        Update();
    }

    public void StartGame()
    {
        gameStarted = true;
        if (scoreText != null)
            scoreText.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }
}
