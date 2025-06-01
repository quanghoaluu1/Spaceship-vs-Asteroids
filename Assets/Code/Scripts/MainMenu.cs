using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlayGame()
    {
        ScoreManager.Instance = null;
        SceneManager.LoadSceneAsync(1);
    }

    public void HowToPlay()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void GoBackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
