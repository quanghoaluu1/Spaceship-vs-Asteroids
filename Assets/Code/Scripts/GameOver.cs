using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText1;

    void Start()
    {

        if (ScoreManager.Instance != null)
        {
            Debug.Log("ScoreManager found, score = " + ScoreManager.Instance.score);
            scoreText.text = ScoreManager.Instance.score.ToString();
        }

        //if (TimeManager.Instance != null)
        //{
        //    int minutes = Mathf.FloorToInt(TimeManager.Instance.elapsedTime / 60f);
        //    int seconds = Mathf.FloorToInt(TimeManager.Instance.elapsedTime % 60f);
        //    timeText1.text = $"{minutes:00}:{seconds:00}";
        //}


    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
