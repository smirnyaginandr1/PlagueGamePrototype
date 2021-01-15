using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
public class PauseMenu : MonoBehaviour
{
    public GameObject _text;
    public static bool _GameIsPaused = false;
    public GameObject _pauseMenu;
    public GameObject _settingsMenu;
    public GameObject _preferencesMenu;
    [SerializeField]
    private Image _liveOneImg, _liveTwoImg, _liveThreeImg;
    [SerializeField]
    private Sprite[] _liveOneSprites, _liveTwoSprites, _liveThreeSprites;
    [SerializeField]
    private Image _plagueOneImg, _plagueTwoImg;
    [SerializeField]
    private Sprite[] _plagueOneSprites, _plagueTwoSprites;
    [SerializeField]
    private Image _checkOneImg, _checkTwoImg, _checkThreeImg;
    [SerializeField]
    private Sprite[] _checkOneSprites, _checkTwoSprites, _checkThreeSprites;

    public int lvl;
    public int plague;
    public int checkpoint;

    public int lvlExit;
    public int plagueExit;
    public int checkpointExit;
    FileInfo settingsFile = new FileInfo("SettingsLVL.txt");
    public void QuitGame()
    {
        SceneManager.LoadScene(0);
    }
    public void ResumeGame()
    {
        _pauseMenu.SetActive(false);
        _GameIsPaused = false;
    }

    public void ExitSettingsMenu()
    {
        _settingsMenu.SetActive(false);
    }

    public void PauseGame()
    {
        _pauseMenu.SetActive(true);
        _GameIsPaused = true;
    }
    public void SettingsMenu()
    {
        setOneCheck();
        setOneLvl();
        setOnePlague();
        _settingsMenu.SetActive(true);
    }
    public void ResetSettingsMenu()
    {
        lvl = lvlExit;
        plague = plagueExit;
        checkpoint = checkpointExit;
        SaveSettings();
        _settingsMenu.SetActive(false);
        SceneManager.LoadScene(lvl);

    }
    public void setTwoLvl()
    {
        SetButtonColor(1, 3, 0);
        SetButtonColor(1, 1, 0);
        SetButtonColor(1, 2, 1);
        lvlExit = 2;
    }
    public void setOneLvl()
    {
        SetButtonColor(1, 2, 0);
        SetButtonColor(1, 3, 0);
        SetButtonColor(1, 1, 1);

        lvlExit = 1;
    }
    public void setThreeLvl()
    {
        SetButtonColor(1, 1, 0);
        SetButtonColor(1, 2, 0);
        SetButtonColor(1, 3, 1);
        lvlExit = 3;
    }
    public void setOnePlague()
    {
        SetButtonColor(2, 2, 0);
        SetButtonColor(2, 1, 1);
        plagueExit = 1;
    }
    public void setTwoPlague()
    {
        SetButtonColor(2, 1, 0);
        SetButtonColor(2, 2, 1);
        plagueExit = 2;
    }
    public void setOneCheck()
    {
        SetButtonColor(3, 1, 1);
        SetButtonColor(3, 2, 0);
        SetButtonColor(3, 3, 0);
        checkpointExit = 4;
    }
    public void setTwoCheck()
    {
        SetButtonColor(3, 1, 0);
        SetButtonColor(3, 2, 1);
        SetButtonColor(3, 3, 0);
        checkpointExit = 6;
    }
    public void setThreeCheck()
    {
        SetButtonColor(3, 1, 0);
        SetButtonColor(3, 2, 0);
        SetButtonColor(3, 3, 1);
        checkpointExit = 8;
    }

    public void SaveSettings()
    {
        using (StreamWriter sw = settingsFile.CreateText())
        {
            if (lvl == 3) sw.WriteLine("LVL: 3");
            else if (lvl == 2) sw.WriteLine("LVL: 2");
            else sw.WriteLine("LVL: 1");

            if (checkpoint == 8) sw.WriteLine("CC: 8");
            else if (checkpoint == 4) sw.WriteLine("CC: 4");
            else sw.WriteLine("CC: 6");

            if (plague == 2) sw.WriteLine("PC: 2");
            else sw.WriteLine("PC: 1");
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
    }
    public void PreferenceMenu()
    {
        _preferencesMenu.SetActive(true);
    }

    public void ExitPreferenceMenu()
    {
        _preferencesMenu.SetActive(false);
    }

    public void ResumeLevelOne()
    {
        lvl = 2;
        SaveSettings();
        SceneManager.LoadScene(2);
    }
    public void ResumeLevelTwo()
    {
        lvl = 3;
        SaveSettings();
        SceneManager.LoadScene(3);
    }
    public void SetButtonColor(int objectNumber, int buttonNumber, int imgNumber)
    {
        switch (objectNumber)
        {
            case 1:
                switch (buttonNumber)
                {
                    case 1:
                        _liveOneImg.sprite = _liveOneSprites[imgNumber];
                        break;
                    case 2:
                        _liveTwoImg.sprite = _liveTwoSprites[imgNumber];
                        break;
                    case 3:
                        _liveThreeImg.sprite = _liveThreeSprites[imgNumber];
                        break;
                }
                break;
            case 2:
                switch (buttonNumber)
                {
                    case 1:
                        _plagueOneImg.sprite = _plagueOneSprites[imgNumber];
                        break;
                    case 2:
                        _plagueTwoImg.sprite = _plagueTwoSprites[imgNumber];
                        break;
                }
                break;

            case 3:
                switch (buttonNumber)
                {
                    case 1:
                        _checkOneImg.sprite = _checkOneSprites[imgNumber];
                        break;
                    case 2:
                        _checkTwoImg.sprite = _checkTwoSprites[imgNumber];
                        break;
                    case 3:
                        _checkThreeImg.sprite = _checkThreeSprites[imgNumber];
                        break;
                }
                break;
        }
    }
}
