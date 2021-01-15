using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public Scrollbar _volume;
    public Scrollbar _soundEffect;
    public AudioSource _mainMusic;
    public AudioSource _onClickMusic;
    public AudioSource _onLoseMusic;
    public AudioSource _onWinMusic;
    public AudioSource _onCollisionWithPlague;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _mainMusic.volume = _volume.value;
        _onClickMusic.volume = _soundEffect.value;
    }
    public void YouWinMusic()
    {
        if(_mainMusic.isPlaying)
        {
            _mainMusic.Stop();
            _onWinMusic.playOnAwake = true;
            _onWinMusic.Play();
        }
    }
    public void YouLoseMusic()
    {
        if (_mainMusic.isPlaying)
        {
            _mainMusic.Stop();
            _onLoseMusic.playOnAwake = true;
            _onLoseMusic.Play();
        }
    }
}
