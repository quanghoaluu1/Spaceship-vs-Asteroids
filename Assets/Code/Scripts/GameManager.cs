using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TMP_Text timeText;
    private float elapsedTime = 0f;
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;

    }

    void Update()
    {
        if (!isGameOver)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimeUI();
        }
    }

    void UpdateTimeUI()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timeText.text = $"{minutes:00}:{seconds:00}";
    }

    public float GetElapsedTime() => elapsedTime;

    public void SetGameOver()
    {
        isGameOver = true;
    }
}
