using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    [SerializeField]
    float startTime = 60f;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI killCountText;
    public TextMeshProUGUI healthText;

    public float maxHealth = 100;
    public float currentHealth;

    float killCount = 0;

    float currentTime;
    bool isRunning;

    MainMenuUI ui;
    void Start()
    {
        ui = FindObjectOfType<MainMenuUI>();
        currentTime = startTime;
        currentHealth = maxHealth;
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

        healthText.text = "Health: " + currentHealth;

        killCountText.text = "Kills: " + killCount;
    }

    void OnTimerEnd()
    {
        Debug.Log("Time's up!");
        Time.timeScale = 0f;

        if(currentHealth > 0f)
        {
            SceneManager.LoadScene("WinScene");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            PlayerDied();
        }

        UpdateUI();
    }

    public void AddKill()
    {
        killCount++;
        UpdateUI();
    }

    void PlayerDied()
    {
        Debug.Log("Player Died");
        Time.timeScale = 0f;
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }
}
