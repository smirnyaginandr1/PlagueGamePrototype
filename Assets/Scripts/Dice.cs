using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class Dice : MonoBehaviour
{
    [SerializeField]
    private Image _dice2Img;
    [SerializeField]
    private Sprite[] _dice2Sprites;
    [SerializeField]
    private Image _dice4Img;
    [SerializeField]
    private Sprite[] _dice4Sprites;
    [SerializeField]
    private Image _dice6Img;
    [SerializeField]
    private Sprite[] _dice6Sprites;

    private int usingDice;

    private bool check;
    private void Start()
    {
        
    }
    public void SetCheck(bool checking)
    {
        check = checking;
    }

    public void SetTwoDice()
    {
        usingDice = 2;
        ClickDice();
    }
    public void SetFourDice()
    {
        usingDice = 4;
        ClickDice();
    }
    public void SetSixDice()
    {
        usingDice = 6;
        ClickDice();
    }

    private void ClickDice()
    {
        if (check)
        {
            GameManager.SetCheckInDice(false);
            GameObject.Find("AudioManager").GetComponent<MusicManager>()._onClickMusic.Play();
            if (!GameManager._gameOver)
            {
                StartCoroutine("RollTheDice");
            }
            GameObject.Find("SpawnManager").GetComponent<PlagueSpawner>().visible(false);
            GameObject.Find("Canvas").GetComponent<UIManager>().SetStroke(false);
        }
    }
    private IEnumerator RollTheDice()
    {
        int randomDiceSide = 0;
        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = Random.Range(0, usingDice);
            switch (usingDice)
            {
                case 2:
                    _dice2Img.sprite = _dice2Sprites[randomDiceSide];
                    break;
                case 4:
                    _dice4Img.sprite = _dice4Sprites[randomDiceSide];
                    break;
                case 6:
                    _dice6Img.sprite = _dice6Sprites[randomDiceSide];
                    break;
            }
            yield return new WaitForSeconds(0.05f);
        }

        GameManager._diceSideThrown = randomDiceSide + 1;
        GameManager.MovePlayer(1, randomDiceSide + 1);
        FollowThePath.randomCount = randomDiceSide;
        FollowThePath.count = 0;
    }
}
