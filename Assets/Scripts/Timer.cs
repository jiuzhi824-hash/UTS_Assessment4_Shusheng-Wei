using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    private float totalTime = 0f;
    public bool isRunning;
    public Text timerText;

    public PlayerMovement playerMovement;
    private string currentSceneName;
    private string bestTimeKey;
 
    public string targetSceneName = "Level_Recreation";

    void Awake()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        bestTimeKey = "BestTime_" + currentSceneName;
    }

    void Update()
    {
        if (isRunning)
        {
            totalTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    
    public void OnGameEnd()
    {
        StopTimer();

     
        if (currentSceneName != targetSceneName)
            return;

       
        if (playerMovement != null)
        {
            var highScoreField = typeof(PlayerMovement).GetField("highScore", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var scoreField = typeof(PlayerMovement).GetField("score", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (highScoreField != null && scoreField != null)
            {
                int currentHighScore = (int)highScoreField.GetValue(playerMovement);
                int currentScore = (int)scoreField.GetValue(playerMovement);

                if (currentScore == currentHighScore)
                {
                    PlayerPrefs.SetFloat(bestTimeKey, totalTime);
                    PlayerPrefs.Save();
                }
            }
        }
    }

    
    public void StartTimer() { isRunning = true; }
    public void StopTimer() { isRunning = false; }
    public void ResetTimer()
    {
        totalTime = 0f;
        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        if (timerText == null) return;

        int totalSeconds = (int)totalTime;
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        int milliseconds = (int)((totalTime - totalSeconds) * 1000) % 100;

        timerText.text = $"{minutes:00}:{seconds:00}:{milliseconds:00}";
    }
}