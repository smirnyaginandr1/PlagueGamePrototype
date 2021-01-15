using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSpawner : MonoBehaviour
{
    [HideInInspector]
    public int lvl;
    [HideInInspector]
    public int checkSize;
    public GameObject check;
    public Check[] _checkpoints;
    [SerializeField]
    private Transform[] _waypointsCheck;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void CreateCheck()
    {
        int[] mass = CheckSettings();

        _checkpoints = new Check[checkSize];

        for (int i = 0; i < checkSize; i++)
            _checkpoints[i] = new Check();

        int massCount = 0;

        foreach (var checkpoint in _checkpoints)
        {
            checkpoint._checkpoint = Instantiate(check, new Vector3(-5, 8, 0), Quaternion.identity);
            checkpoint._checkpoint.transform.position = Vector2.MoveTowards(checkpoint._checkpoint.transform.position,
                _waypointsCheck[mass[massCount]].transform.position, 100f);
            checkpoint.position = mass[massCount];
            massCount++;
        }
    }
    public int[] CheckSettings()
    {
        switch (lvl)
        {
            case 1:
                switch (checkSize)
                {
                    case 4:
                        return new int[4] { 10, 19, 26, 31 };

                    case 6:
                        return new int[6] { 5, 10, 15, 19, 23, 29 };

                    case 8:
                        return new int[8] { 5, 10, 15, 19, 23, 26, 29, 32 };
                    default:
                        return null;
                }

            case 2:
                switch (checkSize)
                {
                    case 4:
                        return new int[4] { 12, 23, 32, 39 };

                    case 6:
                        return new int[6] { 6, 12, 18, 28, 36, 42 };

                    case 8:
                        return new int[8] { 6, 12, 18, 23, 28, 32, 36, 42 };
                    default:
                        return null;
                }

            case 3:
                switch (checkSize)
                {
                    case 4:
                        return new int[4] { 14, 27, 38, 51 };

                    case 6:
                        return new int[6] { 7, 21, 33, 43, 51, 57 };

                    case 8:
                        return new int[8] { 7, 21, 27, 33, 38, 43, 51, 57 };
                    default:
                        return null;
                }
            default:
                return null;
        }
    }

}


    public class Check
{
    public GameObject _checkpoint;
    public int position;
}
