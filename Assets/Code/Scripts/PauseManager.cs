using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    private bool isPaused = false;
    public Button targetButton;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0 : 1;
        pausePanel.SetActive(isPaused);
        targetButton.gameObject.SetActive(false);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        targetButton.gameObject.SetActive(true);

    }

    public void BackToMenu()
    {
        SaveGame();
        Time.timeScale = 1f;
        if (PlayerController.Instance != null)
        {
            Destroy(PlayerController.Instance.gameObject);
        }
        if (ScoreManager.Instance != null)
        {
            Destroy(ScoreManager.Instance.gameObject);
        }
        if (TimeManager.Instance != null)
        {
            Destroy(TimeManager.Instance.gameObject);
        }
        SceneManager.LoadSceneAsync(0);
    }
    private void SaveGame()
    {
        PlayerPrefs.SetInt("SavedScore", ScoreManager.Instance.score);
        PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.SetInt("SavedHealth", PlayerController.Instance.currentHealth);
        PlayerPrefs.SetFloat("SavedTime", TimeManager.Instance.GetElapsedTime());
        PlayerPrefs.SetInt("HasSaved", 1);
        PlayerPrefs.Save();
        Debug.Log("Saved score: " + ScoreManager.Instance.score);
        Debug.Log("Saved scene: " + SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Saved health: " + PlayerController.Instance.currentHealth);
        Debug.Log("Game saved successfully!");
    }
}
