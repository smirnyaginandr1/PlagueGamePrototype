using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int _lives;
    
    public Vector3 respawnPoint;
    public int checkCount;
    [SerializeField]    
    private PlagueSpawner _plagueSpawner;
    [SerializeField]
    private UIManager _uIManager;
    private bool triggerFixed;
    
    // Start is called before the first frame update
    void Start()
    {
        triggerFixed = true;
        checkCount = 0;
        _lives = 3;
        respawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Damage()
    {
        _lives--;
        GameObject.Find("Canvas").GetComponent<UIManager>().UpdateLives(_lives);
        GameObject.Find("Canvas").GetComponent<UIManager>().UpdateLivesForMenu(_lives);
       // GameObject.Find("Canvas").GetComponent<UIManager>().UpdateLives(_lives);
        if (_lives == 0)
        {
            GetComponent<FollowThePath>().transform.position = Vector3.MoveTowards(GetComponent<FollowThePath>().transform.position,
                respawnPoint, 10.0f);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        int[] checkMass = AppendToStart();
        switch (other.tag)
        {
            case "Plague":
                GetComponent<FollowThePath>()._waypointIndex = checkMass[checkCount];
                GetComponent<FollowThePath>()._waypointIndexFirst = checkMass[checkCount];
                GameManager._playerStartWaypoint = GetComponent<FollowThePath>()._waypointIndex - 1;
                GameManager.SetPlayerActive(false);
                transform.position = respawnPoint;
                GameManager.SetPlayerActive(true);
                Damage();
                GameObject.Find("AudioManager").GetComponent<MusicManager>()._onCollisionWithPlague.playOnAwake=true;
                GameObject.Find("AudioManager").GetComponent<MusicManager>()._onCollisionWithPlague.Play();
                if (checkCount != 0)
                    triggerFixed = false;
                break;

            case "Checkpoint":
                if (triggerFixed)
                {
                    respawnPoint = other.transform.position;
                    checkCount++;
                }
                triggerFixed = true;
                break;

            case "FinalCell":
                GameObject.Find("GameManager").GetComponent<GameManager>().PlayerWin();
                break;

        }      
    }

    static int[] AppendToStart()
    {
        int[] mass = GameObject.Find("SpawnManager").GetComponent<CheckpointSpawner>().CheckSettings();
        int[] result = new int[mass.Length + 1];
        result[0] = 0;
        for (int i = 0; i < mass.Length; i++)
            result[i + 1] = mass[i];

        return result;
    }
}
