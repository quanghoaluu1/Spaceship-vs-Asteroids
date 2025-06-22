using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Assets.Code.Scripts;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText1;

    public TMP_InputField nameInput;
    public Button submitButton;
    public GameObject leaderboardPanel;
    public GameObject leaderboardEntryPrefab;
    public Transform leaderboardContainer;
    public LeaderboardManager leaderboardManager;

    private int finalScore;
    private float finalTime;

    void Start()
    {

        if (ScoreManager.Instance != null)
        {
            Debug.Log("ScoreManager found, score = " + ScoreManager.Instance.score);
            scoreText.text = ScoreManager.Instance.score.ToString();
        }

        if (TimeManager.Instance != null)
        {
            int minutes = Mathf.FloorToInt(TimeManager.Instance.elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(TimeManager.Instance.elapsedTime % 60f);
            timeText1.text = $"{minutes:00}:{seconds:00}";
        }
        submitButton.onClick.AddListener(SubmitScore);
        leaderboardPanel.SetActive(false);

    }

    public void SubmitScore()
    {
        string name = nameInput.text;
        if (string.IsNullOrEmpty(name)) return;

        finalScore = ScoreManager.Instance.score;
        finalTime = TimeManager.Instance.elapsedTime;

        leaderboardManager.AddEntry(name, finalScore, finalTime);
        ShowLeaderboard();
    }

    public void ShowLeaderboard()
    {
        leaderboardPanel.SetActive(true);

        // Clear old entries
        foreach (Transform child in leaderboardContainer)
        {
            Destroy(child.gameObject);
        }

        List<ScoreEntry> top = leaderboardManager.GetTopEntries();
        foreach (var entry in top)
        {
            GameObject go = Instantiate(leaderboardEntryPrefab, leaderboardContainer);
            TextMeshProUGUI[] texts = go.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = entry.name;
            texts[1].text = entry.score.ToString();
            texts[2].text = $"{Mathf.FloorToInt(entry.time / 60):00}:{Mathf.FloorToInt(entry.time % 60):00}";
        }
    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
