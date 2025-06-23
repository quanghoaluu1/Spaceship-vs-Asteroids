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
    public Button closeLeaderboardButton;

    private int finalScore;
    private float finalTime;
    private bool scoreSubmitted = false;

    public static GameOver Instance;


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

        submitButton.onClick.RemoveAllListeners();
        submitButton.onClick.AddListener(SubmitScore);

        if (closeLeaderboardButton != null)
        {
            closeLeaderboardButton.onClick.RemoveAllListeners();
            closeLeaderboardButton.onClick.AddListener(CloseLeaderboard);
        }

        leaderboardPanel.SetActive(false);

    }

    public void SubmitScore()
    {
        if (scoreSubmitted) return;
        scoreSubmitted = true;

        Debug.Log("SubmitScore() called");
        string name = nameInput.text;
        if (string.IsNullOrEmpty(name)) return;

        submitButton.interactable = false;

        finalScore = ScoreManager.Instance.score;
        finalTime = TimeManager.Instance.elapsedTime;
        Debug.Log("Score: " + finalScore.ToString());
        Debug.Log("Time: " + finalTime.ToString());

        leaderboardManager.AddEntry(name, finalScore, finalTime);
        ShowLeaderboard();
    }

    public void CloseLeaderboard()
    {
        leaderboardPanel.SetActive(false);
    }

    public void ShowLeaderboard()
    {
        leaderboardPanel.SetActive(true);

        foreach (Transform child in leaderboardContainer)
        {
            Destroy(child.gameObject);
        }

        List<ScoreEntry> top = leaderboardManager.GetTopEntries();
        Debug.Log("Top entries count: " + top.Count);

        for (int i = 0; i < top.Count; i++)
        {
            var entry = top[i];
            GameObject go = Instantiate(leaderboardEntryPrefab, leaderboardContainer);
            TextMeshProUGUI[] texts = go.GetComponentsInChildren<TextMeshProUGUI>();

            texts[0].text = (i + 1).ToString(); // Rank
            texts[1].text = entry.name;
            texts[2].text = entry.score.ToString();
            texts[3].text = $"{Mathf.FloorToInt(entry.time / 60):00}:{Mathf.FloorToInt(entry.time % 60):00}";
        }
    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
