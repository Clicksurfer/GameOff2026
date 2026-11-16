using System;
using UnityEngine;

public class ThumbWarManager : MonoBehaviour, IGameStateProgressor
{
    [Header("References")]
    [SerializeField] private CombantantController _playerCombatant;
    [SerializeField] private CombantantController _opponentCombatant;
    [SerializeField] private CollisionDetector[] _collisionDetectors;
    [SerializeField] private GameStateManager _gameStateManager;

    [Header("Config")]
    [SerializeField] private float _startHp = 100f;
    [SerializeField] private float _collisionDamage = 10f;
    [SerializeField] private float _pinDamage = 10f;
    
    private ThumbCollisionHandler _collisionHandler;
    
    private Action<GameState> _startCountdownCallback;
    private Action<GameState> _endMatchCallback;

    private void Start()
    {
        PauseTime();
        InitializePlayers();
        SetupListeners();
        TellGameStateManagerWeAreReady();
    }

    private void InitializePlayers()
    {
        _playerCombatant.Initialize(CombatantType.Player, _startHp);
        _opponentCombatant.Initialize(CombatantType.Opponent, _startHp);
    }
    
    private void SetupListeners()
    {
        foreach (var detector in _collisionDetectors)
        {
            detector.OnCollisionEntered += HandleCollision;
        }
        
        _playerCombatant.OnDied += HandleCombatantDied;
        _opponentCombatant.OnDied += HandleCombatantDied;
        
        _gameStateManager.OnGameStateChanged += HandleGameStateChanged;
    }
    
    private void HandleGameStateChanged(GameState gameState)
    {
        if (gameState == GameState.Playing)
        {
            ResumeTime();
        }
    }

    private void PauseTime()
    {
        Time.timeScale = 0f;
    }

    private void ResumeTime()
    {
        Time.timeScale = 1f;
    }
    
    private void HandleCollision(Collision collision)
    {
        if (_collisionHandler == null)
        {
            _collisionHandler = new ThumbCollisionHandler(_playerCombatant, _opponentCombatant, _collisionDamage, _pinDamage);
        }
        
        _collisionHandler.HandleCollision();
    }

    private void HandleCombatantDied(CombatantType combatantType)
    {
        if (combatantType == CombatantType.Player)
        {
            TellGameStateManagerMatchIsLost();
        }
        else if (combatantType == CombatantType.Opponent)
        {
            TellGameStateManagerMatchIsWon();
        }
    }

    #region Game State

    public void InvokeThisWhenGameStateShouldUpdate(GameState newState, Action<GameState> onGameStateChanged)
    {
        if (newState == GameState.Countdown)
        {
            _startCountdownCallback = onGameStateChanged;
        }
        else if (newState == GameState.GameWon || newState == GameState.GameLost)
        {
            _endMatchCallback = onGameStateChanged;
        }
    }
    
    private void TellGameStateManagerWeAreReady()
    {
        _startCountdownCallback?.Invoke(GameState.Countdown);
    }

    private void TellGameStateManagerMatchIsWon()
    {
        _endMatchCallback?.Invoke(GameState.GameWon);
    }
    
    private void TellGameStateManagerMatchIsLost()
    {
        _endMatchCallback?.Invoke(GameState.GameLost);
    }

    #endregion
}