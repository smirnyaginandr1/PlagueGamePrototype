using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Image _livesForMenu;
    [SerializeField]
    private Sprite[] _liveSpritesForMenu;
    [SerializeField]
    private Image _strokeImg;
    [SerializeField]
    private Sprite[] _strokeSprites;
    [SerializeField]
    private Image _strokeIconPlayer;
    [SerializeField]
    private Sprite[] _strokeSpritesIconPlayer;
    [SerializeField]
    private Image _strokeIconPlague;
    [SerializeField]
    private Sprite[] _strokeSpritesIconPlague;


    // Start is called before the first frame update

    public void UpdateLives(int currentLives)
    {
        
        _livesImg.sprite = _liveSprites[currentLives];
  //      GameObject.Find("LivesDisplay").GetComponent<SpriteRenderer>().sprite = _liveSprites[currentLives];
    }

    public void UpdateLivesForMenu(int currentLives)
    {
        _livesForMenu.sprite = _liveSpritesForMenu[currentLives];
    }

    public void SetStroke(bool playerStroke)
    {
        if (playerStroke)
        {
            _strokeImg.sprite = _strokeSprites[0];
            _strokeIconPlayer.sprite = _strokeSpritesIconPlayer[0];
            _strokeIconPlague.sprite = _strokeSpritesIconPlague[1];
        }
        else
        {
            _strokeImg.sprite = _strokeSprites[1];
            _strokeIconPlayer.sprite = _strokeSpritesIconPlayer[1];
            _strokeIconPlague.sprite = _strokeSpritesIconPlague[0];
        }   
    }
}
