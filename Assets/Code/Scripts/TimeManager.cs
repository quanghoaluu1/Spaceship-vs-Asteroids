using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    public TextMeshProUGUI timeText;
    public float elapsedTime = 0f;
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 👈 dòng này để giữ lại object khi chuyển scene
        }
        else
        {
            Destroy(gameObject); // tránh trùng lặp
        }
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
