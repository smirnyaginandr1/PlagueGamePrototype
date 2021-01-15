using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private UIManager _uIManager;
    public static List<Field> fieldClass = new List<Field>(); //Лист ячеек на поле. Содержит объекты класса Field
    public static int[,] massProbabilities; //Тут содержатся вероятности для заполнения строк и столбцов
    public static int[,] finalMassProbabilities; //Тут содержатся вероятности выпадения лавы
    FileInfo settingsFile = new FileInfo("SettingsLVL.txt");
    public static GameObject[] _fractionTextRow;
    public static GameObject[] _fractionTextColumn;
    [SerializeField]
    //private static GameObject _GameResultText;
    //private static GameObject _RestartButton;
    private static GameObject _player;
    public static int lvl;
    public static int _diceSideThrown;
    public static int _playerStartWaypoint;
    public static bool _gameOver;
    private static bool firstDice;
    public GameObject _plagueWin, _playerWin;
    // Start is called before the first frame update
    void Start()
    {
        if (fieldClass.Count != 0)
            fieldClass.Clear();
        _gameOver = false;
        firstDice = true;
        _diceSideThrown = 0;
        _playerStartWaypoint = 0;
        _fractionTextColumn = GameObject.FindGameObjectsWithTag("Column");
        _fractionTextRow = GameObject.FindGameObjectsWithTag("Row");
       // _GameResultText = GameObject.Find("GameResultText");
       // _RestartButton = GameObject.Find("RestartButton");
        _player = GameObject.Find("Player");
        _player.GetComponent<FollowThePath>()._moveAllowed = false;

        FileParse();

        CalculatingRowAndColumn();
        CalculatingListInit();
        GameObject.Find("SpawnManager").GetComponent<CheckpointSpawner>().CreateCheck();
        GameObject.Find("SpawnManager").GetComponent<PlagueSpawner>().CreatePlague();
        PlagueSpawner.InitBorders();
        SetProbabilitiesText();
        SetCheckInDice(true);
    }
    void Update()
    {
        if ((_player.GetComponent<FollowThePath>()._waypointIndex > _playerStartWaypoint + _diceSideThrown) ||
            ((_player.GetComponent<FollowThePath>()._waypointIndex > 0) && (firstDice == true)))
        {
            firstDice = false;
            _player.GetComponent<FollowThePath>()._moveAllowed = false;
            _playerStartWaypoint = _player.GetComponent<FollowThePath>()._waypointIndex - 1;
            Debug.Log(_player.GetComponent<FollowThePath>()._waypointIndex);
            CalculatingRowAndColumn();
            CalculatingListProbabilities();
            GameObject.Find("SpawnManager").GetComponent<PlagueSpawner>().SpawnPlague(false);
            GameObject.Find("SpawnManager").GetComponent<PlagueSpawner>().GetPlague();
        }
        if (_player.GetComponent<Player>()._lives == 0)
        {
            //TODO: добавить конец игры при поражении
            _plagueWin.gameObject.SetActive(true);
            GameObject.Find("AudioManager").GetComponent<MusicManager>().YouLoseMusic();
            _gameOver = true;
        }

    }
    /// <summary>
    /// Движение игрока
    /// </summary>

    public void FileParse()
    {
        using (StreamReader sr = settingsFile.OpenText())
        {
            var s = "";
            while ((s = sr.ReadLine()) != null)
            {
                switch (s[0])
                {
                    case 'L':
                        GameObject.Find("SpawnManager").GetComponent<CheckpointSpawner>().lvl = (int)Char.GetNumericValue(s[5]);
                        lvl = (int)Char.GetNumericValue(s[5]);
                        break;

                    case 'P':
                        GameObject.Find("SpawnManager").GetComponent<PlagueSpawner>().plagueCountInFile = (int)Char.GetNumericValue(s[4]);
                        break;

                    case 'C':
                        GameObject.Find("SpawnManager").GetComponent<PlagueSpawner>().checkCount = (int)Char.GetNumericValue(s[4]);
                        GameObject.Find("SpawnManager").GetComponent<CheckpointSpawner>().checkSize = (int)Char.GetNumericValue(s[4]);
                        break;
                }
            }
        }
    }

    public static void MovePlayer(int playerToMove, int randomPoint)
    {
        switch (playerToMove)
        {
            case 1:
                _player.GetComponent<FollowThePath>().timerFlag = true;
                if (_player.GetComponent<FollowThePath>()._waypointIndex + randomPoint >= _player.GetComponent<FollowThePath>()._waypoints.Length)
                    _player.GetComponent<FollowThePath>()._waypointIndex = _player.GetComponent<FollowThePath>()._waypoints.Length - 1;
                else
                {
                    _player.GetComponent<FollowThePath>()._waypointIndexFirst = _player.GetComponent<FollowThePath>()._waypointIndex;
                    _player.GetComponent<FollowThePath>()._waypointIndex += randomPoint;
                    _player.GetComponent<FollowThePath>()._moveAllowed = true;
                }
                break; 
        }
    }


    /// <summary>
    /// Заполняет массивы полей и текста
    /// </summary>
    private void CalculatingRowAndColumn()
    {
       
        massProbabilities = GetComponent<Calculator>().CalculatingProbabilities(_fractionTextRow.Length, _fractionTextColumn.Length, lvl);
        finalMassProbabilities = GetComponent<Calculator>().FormationField(massProbabilities, _fractionTextRow.Length, _fractionTextColumn.Length);
    }

    public void SetProbabilitiesText()
    {
        bool check = true;
        int count = 0;
        for (int i = 0; i < _fractionTextColumn.Length + _fractionTextRow.Length + 1; i++)
        {
            if (massProbabilities[0, i] == 0)
            {
                check = false;
                count = 0;
                continue;
            }
            if (check)
            {
                _fractionTextColumn[count].GetComponent<Text>().text = massProbabilities[0, i].ToString() + "/" + massProbabilities[1, i].ToString();
                count++;
            }
            else
            {
                _fractionTextRow[count].GetComponent<Text>().text = massProbabilities[0, i].ToString() + "/" + massProbabilities[1, i].ToString();
                count++;
            }
        }
    }

    /// <summary>
    /// Инициализирует лист с классами
    /// </summary>
    private void CalculatingListInit()
    {
        int[] massCheck = GameObject.Find("SpawnManager").GetComponent<CheckpointSpawner>().CheckSettings();
        for (int i = 0; i < _player.GetComponent<FollowThePath>()._waypoints.Length; i++)
        {
            bool flag = false;
            foreach (int a in massCheck)
                if (i == a)
                    flag = true;
            if (flag)
                fieldClass.Add(new Field(i, finalMassProbabilities[0, i], finalMassProbabilities[1, i], true, false));
            else
                fieldClass.Add(new Field(i, finalMassProbabilities[0, i], finalMassProbabilities[1, i], false, false));
        }
        
        int lastCount = 0;
        foreach (Field a in fieldClass)
        {
            if (a.checkCheckpoint == true || a.cellPosition == _player.GetComponent<FollowThePath>()._waypoints.Length - 1)
            {
                for (int i = lastCount; i < a.cellPosition; i++)
                {
                    fieldClass[i].maxPositionPlague = a.cellPosition - 1;
                    fieldClass[i].minPositionPlague = lastCount + 1;
                }
                a.maxPositionPlague = 0;
                a.minPositionPlague = 0;
                lastCount = a.cellPosition;
            }
        }
        //GameObject.Find("SpawnManager").GetComponent<PlagueSpawner>().SpawnPlague(true);

    }
    /// <summary>
    /// Обновление вероятностей в листе
    /// </summary>
    private void CalculatingListProbabilities()
    {
        
        for (int i = 0; i < _player.GetComponent<FollowThePath>()._waypoints.Length; i++)
        {
            fieldClass[i].probabilitiesNumerator = finalMassProbabilities[0, i];
            fieldClass[i].probabilitiesDenominator = finalMassProbabilities[1, i];
        }
    }

    public void PlayerDamageMove(bool visible)
    {
        _player.SetActive(visible);
        _player.GetComponent<SpriteRenderer>().enabled = visible;
    }

    public void PlayerWin()
    {
        _playerWin.gameObject.SetActive(true);
        GameObject.Find("AudioManager").GetComponent<MusicManager>().YouWinMusic();
        _gameOver = true;
    }
    public static void SetPlayerActive(bool active)
    {
        _player.SetActive(active);
    }
    public static void SetCheckInDice(bool check)
    {
        GameObject.Find("Canvas").GetComponent<Dice>().SetCheck(check);
    }
}
 public class Field
{
    public int cellPosition;
    public int probabilitiesNumerator;
    public int probabilitiesDenominator;
    public bool checkCheckpoint;
    public bool plague;
    public int minPositionPlague;
    public int maxPositionPlague;
    public Field(int cell, int num, int den, bool chekpoint, bool plag)
    {
        cellPosition = cell;
        probabilitiesNumerator = num;
        probabilitiesDenominator = den;
        checkCheckpoint = chekpoint;
        plague = plag;
    }
}
