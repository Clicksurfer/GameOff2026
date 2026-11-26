using System;
using UnityEngine;

[Serializable]
public class EndgameVisualsConfig
{
    [SerializeField] private Sprite _playerSprite;
    [SerializeField] private Sprite _opponentSprite;
    [SerializeField] private string _text;
    
    public Sprite PlayerSprite => _playerSprite;
    public Sprite OpponentSprite => _opponentSprite;
    public string Text => _text;
}