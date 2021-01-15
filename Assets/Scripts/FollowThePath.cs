using System.Collections;
using UnityEngine;

public class FollowThePath : MonoBehaviour
{

    public Transform[] _waypoints;
    public static int count, randomCount;
    [SerializeField]
    private float _moveSpeed = 5f;

    [HideInInspector]
    public int _waypointIndex;
    public int _waypointIndexFirst;
    public bool _moveAllowed;
    public bool timerFlag;
    // Use this for initialization
    private void Start()
    {
        count = 0;
        _waypointIndex = 0;
        _waypointIndexFirst = 0;
        _moveAllowed = false;
        timerFlag = false;
        transform.position = _waypoints[_waypointIndex].transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        if ((_waypointIndexFirst < _waypointIndex) && timerFlag == true)
        {
            timerFlag = false;
            StartCoroutine("ExecuteAfterTime");
        }
        Move();
    }

    private void Move()
    {
        if (_waypointIndex < _waypoints.Length)
        {
            transform.position = Vector2.MoveTowards(transform.position,
            _waypoints[_waypointIndexFirst].transform.position,
            _moveSpeed * Time.deltaTime);

            if (transform.position == _waypoints[_waypointIndex].transform.position)
            {
                _waypointIndex++;
                
            }

        }
    }
    private IEnumerator ExecuteAfterTime()
    {
        yield return new WaitForSeconds(0.6f);
        _waypointIndexFirst++;
        timerFlag = true;
        count++;
        StartCoroutine("wait");
    }

    private IEnumerator wait()
    {
        
        if (count == randomCount + 1)
        {
            yield return new WaitForSeconds(0.6f);
            GameObject.Find("SpawnManager").GetComponent<PlagueSpawner>().visible(true);
            GameObject.Find("Canvas").GetComponent<UIManager>().SetStroke(true);
            GameObject.Find("GameManager").GetComponent<GameManager>().SetProbabilitiesText();
            GameManager.SetCheckInDice(true);
        }
    }
}