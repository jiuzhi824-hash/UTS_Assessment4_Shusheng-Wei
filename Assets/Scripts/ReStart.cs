using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReStart : MonoBehaviour
{
 
    void Awake()
    {

    }

    public void OnOldReloadButtonClick()
    {

        SceneManager.LoadScene("Level");
    }
    public void OnReloadButtonClick()
    { 
       
        SceneManager.LoadScene("Level_Recreation");
    }

    public void OnStartButtonClick()
    {

        SceneManager.LoadScene("StartScene");
    }
}
