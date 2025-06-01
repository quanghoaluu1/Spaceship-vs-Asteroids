using UnityEngine;
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

    //public void BackToMenu()
    //{
    //    Time.timeScale = 1;
    //    UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    //}
}
