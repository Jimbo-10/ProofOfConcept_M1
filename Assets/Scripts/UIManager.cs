using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    [SerializeField]
    float startTime = 60f;
    public TextMeshProUGUI timerText;

    float currentTime;
    bool isRunning;

    void Start()
    {
        currentTime = startTime;
        isRunning = true;
        UpdateUI();
    }

    void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            isRunning = false;
            OnTimerEnd();
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void OnTimerEnd()
    {
        Debug.Log("Time's up!");
    }
}
