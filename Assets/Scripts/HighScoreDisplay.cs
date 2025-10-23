using UnityEngine;
using UnityEngine.UI;

public class HighScoreDisplay : MonoBehaviour
{

   
    public Text sceneA_HighScoreText; 
    public string sceneAName = "Level"; 

    public Text sceneB_HighScoreText; 
    public Text sceneB_BestTimeText;  
    public string sceneBName = "Level_Recreation"; 

    void Start()
    {
        UpdateAllSceneDataDisplay();
    }

    public void UpdateAllSceneDataDisplay()
    {
        
        UpdateSceneAData();
      
        UpdateSceneBData();
    }

   
    private void UpdateSceneAData()
    {
        string highScoreKey = "HighScore_" + sceneAName;
        int highScore = PlayerPrefs.GetInt(highScoreKey, 0);

        if (sceneA_HighScoreText != null)
        {
            sceneA_HighScoreText.text = $"HighScore: {highScore:D6}";
        }
    }

    private void UpdateSceneBData()
    {
        
        string highScoreKey = "HighScore_" + sceneBName;
        int highScore = PlayerPrefs.GetInt(highScoreKey, 0);
        
        string bestTimeKey = "BestTime_" + sceneBName;
        float bestTime = PlayerPrefs.GetFloat(bestTimeKey, 0f);

    
        if (sceneB_HighScoreText != null)
        {
            sceneB_HighScoreText.text = $"HighScore: {highScore:D6}";
        }

       
        if (sceneB_BestTimeText != null)
        {
            int totalSeconds = (int)bestTime;
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            int milliseconds = (int)((bestTime - totalSeconds) * 1000) % 100;

            sceneB_BestTimeText.text = $"BestTime: {minutes:00}:{seconds:00}:{milliseconds:00}";
        }
    }

}