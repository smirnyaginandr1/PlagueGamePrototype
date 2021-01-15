using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlagueSpawner : MonoBehaviour
{
    
    public Spawn[] _plaguePrefab;
    public GameObject plague;
    [SerializeField]
    private Transform[] _waypointsLava;
    public int plagueCountInFile;
    public int plagueCount;
    public int checkCount;
    public bool minFlag = true;
    // Start is called before the first frame update
    void Start()
    {
            
        //      SpawnPlague();
    }

    // Update is called once per frame
    void Update(){
    }

    public void CreatePlague()
    {
        switch (checkCount)
        {
            case 4:
                if (plagueCountInFile == 1) plagueCount = 5;
                else plagueCount = 10;
                break;

            case 6:
                if (plagueCountInFile == 1) plagueCount = 7;
                else plagueCount = 14;
                break;

            case 8:
                if (plagueCountInFile == 1) plagueCount = 9;
                else plagueCount = 18;
                break;
            default:
                plagueCount = 6;
                break;
        }
        _plaguePrefab = new Spawn[plagueCount];
        for (int i = 0; i < plagueCount; i++)
            _plaguePrefab[i] = new Spawn();
        foreach (var plag in _plaguePrefab)
            plag._spawnPlague = Instantiate(plague, new Vector3(-5, -8, 0), Quaternion.identity);
    }

    public void OnPlayerDeath()
    {
        
    }
    // TODO: Рефакторить!!! Написал на скорую
    /// <summary>
    /// Метод присваивает объектам лавы позицию
    /// </summary>
    public void SpawnPlague(bool first)
    {
        foreach (Spawn lava in _plaguePrefab)
        {
            lava.min = minFlag;
            if (minFlag) minFlag = false;
            else minFlag = true;

            if (plagueCountInFile == 1)
            {
                int maxNum = 0, maxDen = 0, cellCount = 0;
                for (int count = lava.MinProbabilityRange; count <= lava.MaxProbabilityRange; count++)
                {
                    int flag = CompareProbabilities(maxNum, GameManager.fieldClass[count].probabilitiesNumerator, maxDen, GameManager.fieldClass[count].probabilitiesDenominator);
                    if (flag == 2)
                    {
                        maxNum = GameManager.fieldClass[count].probabilitiesNumerator;
                        maxDen = GameManager.fieldClass[count].probabilitiesDenominator;
                        cellCount = count;
                    }
                }
                lava.newCell = cellCount;
                if (first)
                    lava.cell = lava.newCell;
            } else
            {
                if (lava.min)
                {
                    int maxNum = 0, maxDen = 0, cellCount = 0;
                    for (int count = lava.MinProbabilityRange; count <= lava.MaxProbabilityRange; count++)
                    {
                        int flag = CompareProbabilities(maxNum, GameManager.fieldClass[count].probabilitiesNumerator, maxDen, GameManager.fieldClass[count].probabilitiesDenominator);
                        if (flag == 2)
                        {
                            maxNum = GameManager.fieldClass[count].probabilitiesNumerator;
                            maxDen = GameManager.fieldClass[count].probabilitiesDenominator;
                            cellCount = count;
                        }
                    }
                    lava.newCell = cellCount;
                    if (first)
                        lava.cell = lava.newCell;
                }
                else {
                    int minNum = 100, minDen = 100, cellCount = 0;
                    for (int count = lava.MinProbabilityRange; count <= lava.MaxProbabilityRange; count++)
                    {
                        int flag = CompareProbabilities(minNum, GameManager.fieldClass[count].probabilitiesNumerator, minDen, GameManager.fieldClass[count].probabilitiesDenominator);
                        if (flag == 1)
                        {
                            minNum = GameManager.fieldClass[count].probabilitiesNumerator;
                            minDen = GameManager.fieldClass[count].probabilitiesDenominator;
                            cellCount = count;
                        }
                    }
                    lava.newCell = cellCount;
                    if (first)
                        lava.cell = lava.newCell;
                }
            }

        }
    }

    public void GetPlague()
    {
        foreach (Spawn lava in _plaguePrefab)
        {
            lava._spawnPlague.transform.position = Vector2.MoveTowards(lava._spawnPlague.transform.position, _waypointsLava[lava.cell].transform.position, 100f);
            lava.cell = lava.newCell;
        }
    }

    private int CompareProbabilities(int num1, int num2, int den1, int den2)
    {
        if (num1 * den2 > num2 * den1) return 1;
        else if ((num1 * den2 < num2 * den1) || (num1 == 0 && den1 == 0)) return 2;
        else return 0;
    }

    public static void InitBorders()
    {
        int count = 0, min = 0, max = 0;
        for (int i = 0; i < GameManager.fieldClass.Count; i++)
        {
            if (i == GameManager.fieldClass.Count - 1) break;
            if (min != GameManager.fieldClass[i].minPositionPlague || max != GameManager.fieldClass[i].maxPositionPlague)
            {
                min = GameManager.fieldClass[i].minPositionPlague;
                max = GameManager.fieldClass[i].maxPositionPlague;

                GameObject.Find("SpawnManager").GetComponent<PlagueSpawner>()._plaguePrefab[count].MinProbabilityRange =
                    GameManager.fieldClass[i].minPositionPlague;

                GameObject.Find("SpawnManager").GetComponent<PlagueSpawner>()._plaguePrefab[count].MaxProbabilityRange =
                    GameManager.fieldClass[i].maxPositionPlague;
                count++;

                if (GameObject.Find("SpawnManager").GetComponent<PlagueSpawner>().plagueCountInFile == 2)
                {
                    min = GameManager.fieldClass[i + 1].minPositionPlague;
                    max = GameManager.fieldClass[i + 1].maxPositionPlague;

                    GameObject.Find("SpawnManager").GetComponent<PlagueSpawner>()._plaguePrefab[count].MinProbabilityRange =
                        GameManager.fieldClass[i + 1].minPositionPlague;

                    GameObject.Find("SpawnManager").GetComponent<PlagueSpawner>()._plaguePrefab[count].MaxProbabilityRange =
                        GameManager.fieldClass[i + 1].maxPositionPlague;
                    i++;
                    count++;
                }
            }
        }
        GameObject.Find("SpawnManager").GetComponent<PlagueSpawner>().SpawnPlague(true);
    }

    public void visible(bool startFlag)
    {
        foreach(Spawn a in _plaguePrefab)
        {
            a._spawnPlague.SetActive(startFlag);
            a._spawnPlague.GetComponent<SpriteRenderer>().enabled = startFlag;
        }
    }
 //   private static void 
}
[System.Serializable]
public class Spawn
{
    public GameObject _spawnPlague;
    public int MinProbabilityRange = 0;
    public int MaxProbabilityRange = 0;
    public int newCell = 0;
    public int cell = 0;
    public int minCell = 100;
    public bool min = true;
}
