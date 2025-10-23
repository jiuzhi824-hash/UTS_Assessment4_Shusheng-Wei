using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountdownText : MonoBehaviour
{
    private Text countdownText; 
    private int remainingTime = 3; 

    void Awake()
    {
        
        countdownText = GetComponent<Text>();
        
        gameObject.SetActive(false);
    }

    
    void OnEnable()
    {
        if (countdownText != null)
        {
            remainingTime = 3; 
            countdownText.text = remainingTime.ToString(); 
            StartCoroutine(StartCountdown());
        }
    }

 
    private IEnumerator StartCountdown()
    {
        while (remainingTime > 0)
        {
            yield return new WaitForSeconds(1f); 
            remainingTime--;
            countdownText.text = remainingTime.ToString(); 
        }

       
        gameObject.SetActive(false);
    }
}