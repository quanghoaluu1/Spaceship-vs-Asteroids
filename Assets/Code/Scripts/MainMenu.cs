using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject instructionPanel;
    private bool isOpen = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlayGame()
    {
        ScoreManager.Instance = null;
        TimeManager.Instance = null;
        SceneManager.LoadSceneAsync(1);
    }

    public void HowToPlay()
    {
        isOpen = !isOpen;
        instructionPanel.SetActive(true);
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

    public void QuitInstruction()
    {
        isOpen = !isOpen;
        instructionPanel.SetActive(false);

    }
}
