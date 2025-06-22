using System;
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
        if (PlayerPrefs.GetInt("HasSaved", 0) == 1)
        {
            //Debug.Log("Saved Time: " + PlayerPrefs.GetFloat("SavedTime", 9999));
            elapsedTime = PlayerPrefs.GetFloat("SavedTime", 999);
        }
    }

    void Start()
    {
        TryReconnectText();
        UpdateTimeUI();
    }

    void Update()
    {
        if (!isGameOver)
        {
            elapsedTime += Time.deltaTime;

            // Gán lại nếu bị null sau khi load scene
            if (timeText == null)
                TryReconnectText();

            UpdateTimeUI();
        }
    }

    void TryReconnectText()
    {
        if (timeText == null)
        {
            GameObject textObj = GameObject.Find("TimeText"); // 👈 Đảm bảo UI text đúng tên này
            if (textObj != null)
            {
                timeText = textObj.GetComponent<TextMeshProUGUI>();
            }
        }
    }


    void UpdateTimeUI()
    {
        if (timeText == null) return;
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timeText.text = $"{minutes:00}:{seconds:00}";
    }

    public float GetElapsedTime() => elapsedTime;

    public void SetGameOver()
    {
        isGameOver = true;
    }
    public void ResetTime()
    {
        elapsedTime = 0f;
        isGameOver = false;
        UpdateTimeUI(); // update giao diện sau khi reset
    }


}
