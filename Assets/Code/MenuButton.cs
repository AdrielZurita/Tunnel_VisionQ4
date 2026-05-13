using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public PlayerSettings PlayerSettings;
    public CoinTrackerScript CoinTrackerScript;

    public string level = "MainGame";
    public void EnterLevel()
    {
        SceneManager.LoadScene(level);
    }

    public void ResetPlayerSettings()
    {
        PlayerSettings.maxLives = 3;
        PlayerSettings.currentLives = PlayerSettings.maxLives;
        CoinTrackerScript.SetCoinCount(0);
    }

    public void QuitGame()
    {
        Application.Quit();
        print("Quit Game");
    } 

    public void increaseLevelPartCount()
    {
        PlayerSettings.levelPartCount += 1;
    }

    public void resetLevelPartCount()
    {
        PlayerSettings.levelPartCount = 7;
    }
}
