using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    int lvl;
    int plague;
    int checkpoint;
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

    public GameObject _settingsMenu;

    FileInfo settingsFile = new FileInfo("SettingsLVL.txt");
    //TODO: добавить меню выбора настроек
    public void Start()
    {
        SaveSettings();
    }
    public void GameLoad()
    {
        SceneManager.LoadScene(1);
    }
    public void SettingsMenu()
    {
        _settingsMenu.SetActive(true);
    }
    public void ResetSettingsMenu()
    {
        SaveSettings();
        _settingsMenu.SetActive(false);
        SceneManager.LoadScene(lvl);

    }

    public void setOneLvl()
    {
        SetButtonColor(1, 2, 0);
        SetButtonColor(1, 3, 0);
        SetButtonColor(1, 1, 1);

        lvl = 1;
    }
    public void setTwoLvl()
    {
        lvl = 2;
        SetButtonColor(1, 3, 0);
        SetButtonColor(1, 1, 0);
        SetButtonColor(1, 2, 1);

    }
    public void setThreeLvl()
    {
        SetButtonColor(1, 1, 0);
        SetButtonColor(1, 2, 0);
        SetButtonColor(1, 3, 1);
        lvl = 3;
    }
    public void setOnePlague()
    {
        SetButtonColor(2, 2, 0);
        SetButtonColor(2, 1, 1);

        plague = 1;
    }
    public void setTwoPlague()
    {
        SetButtonColor(2, 1, 0);
        SetButtonColor(2, 2, 1);
        plague = 2;
    }
    public void setOneCheck()
    {
        SetButtonColor(3, 1, 1);
        SetButtonColor(3, 2, 0);
        SetButtonColor(3, 3, 0);
        checkpoint = 4;
    }
    public void setTwoCheck()
    {
        SetButtonColor(3, 1, 0);
        SetButtonColor(3, 2, 1);
        SetButtonColor(3, 3, 0);
        checkpoint = 6;
    }
    public void setThreeCheck()
    {
        SetButtonColor(3, 1, 0);
        SetButtonColor(3, 2, 0);
        SetButtonColor(3, 3, 1);
        checkpoint = 8;
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
    public void SettingLevel()
    {
        setOneCheck();
        setOneLvl();
        setOnePlague();
        _settingsMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        _settingsMenu.SetActive(false);
    }

    public void SetButtonColor(int objectNumber, int buttonNumber , int imgNumber)
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