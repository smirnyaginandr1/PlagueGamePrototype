using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public Sprite _heart;
    private SpriteRenderer _checkpointSpriteRenderer;
    public bool _checkpointReached;
    private void Start()
    {
        _checkpointSpriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
}
