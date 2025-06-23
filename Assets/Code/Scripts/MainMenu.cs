using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject instructionPanel;
    public GameObject continueButton;
    public GameObject optionsPanel;
    private bool isOpen = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // Nếu không có dữ liệu lưu thì ẩn nút continue
        if (PlayerPrefs.GetInt("HasSaved", 0) == 1)
        {
            continueButton.SetActive(true);
        }
    }
    public void PlayGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadSceneAsync(1);
    }
    public void ContinueGame()
    {
        int savedScene = PlayerPrefs.GetInt("SavedScene", 1);
        Debug.Log("Saved scene"+ savedScene);
        SceneManager.LoadScene(savedScene);
        Debug.Log("Game loaded successfully!");
    }
    public void HowToPlay()
    {
        isOpen = !isOpen;
        instructionPanel.SetActive(true);
    }

    public void OpenOption()
    {
        isOpen = !isOpen;
        optionsPanel.SetActive(true);
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

    public void QuitOptions()
    {
        isOpen = !isOpen;
        optionsPanel.SetActive(false);
    }

}
