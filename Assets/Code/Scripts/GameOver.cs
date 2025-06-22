using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Assets.Code.Scripts.LeaderBoard;
using Assets.Code.Scripts.GameManager;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText1;
    public TMP_InputField playerNameInput;

    public Button submitButton;

    public GameObject leaderboardEntryPrefab;
    public Transform leaderboardContainer;

    private int currentScore = 0;
    private float timeSurvived = 0f;
    private string gameMode = "Arcade"; // tùy chọn game mode update sau

    void Start()
    {
        Debug.Log("ScoreText: " + (scoreText == null));
        Debug.Log("TimeText: " + (timeText1 == null));
        Debug.Log("ScoreManager: " + (ScoreManager.Instance == null));
        Debug.Log("TimeManager: " + (TimeManager.Instance == null));

        if (ScoreManager.Instance != null)
        {
            Debug.Log("ScoreManager found, score = " + ScoreManager.Instance.score);
            currentScore = ScoreManager.Instance.score;
            scoreText.text = currentScore.ToString();
        }

        if (TimeManager.Instance != null)
        {
            timeSurvived = TimeManager.Instance.elapsedTime;
            int minutes = Mathf.FloorToInt(TimeManager.Instance.elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(TimeManager.Instance.elapsedTime % 60f);
            timeText1.text = $"{minutes:00}:{seconds:00}";
        }
    }

    public void SubmitScore()
    {
        if (playerNameInput == null)
        {
            playerNameInput = GameObject.Find("PlayerNameInput")?.GetComponent<TMP_InputField>();
            Debug.LogWarning("Gán tự động playerNameInput = " + playerNameInput);
        }

        string playerName = playerNameInput.text.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("Tên người chơi trống.");
            return;
        }

        LeaderboardManager.Instance.AddEntry(
            name: playerName,
            score: currentScore,
            time: timeSurvived,
            mode: gameMode
        );

        Debug.Log("Ghi leaderboard thành công!");

        ShowLeaderboard();

        // Khóa nút submit
        submitButton.interactable = false;
        playerNameInput.interactable = false;
    }

    void ShowLeaderboard()
    {
        foreach (Transform child in leaderboardContainer)
        {
            Destroy(child.gameObject); // clear old entries
        }

        List<LeaderboardEntry> topScores = LeaderboardManager.Instance.GetTopScores("Arcade", 10);

        foreach (LeaderboardEntry entry in topScores)
        {
            GameObject go = Instantiate(leaderboardEntryPrefab, leaderboardContainer);
            TextMeshProUGUI[] texts = go.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = entry.playerName;
            texts[1].text = entry.score.ToString();
            texts[2].text = $"{Mathf.FloorToInt(entry.timeSurvived / 60f):00}:{Mathf.FloorToInt(entry.timeSurvived % 60f):00}";
        }
    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
