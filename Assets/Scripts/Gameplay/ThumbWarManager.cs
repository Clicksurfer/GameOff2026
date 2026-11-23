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
    [SerializeField] private float _cantPinDamagePerSecond = 5f;

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
        var initData = new CombatantInitializationData
        {
            CombatantType = CombatantType.Player,
            InitialHealth = _startHp,
            CantPinDamagePerSecond = _cantPinDamagePerSecond
        };
        
        _playerCombatant.Initialize(initData);
        
        // Since data type is struct, we can reuse the same variable and just change the CombatantType for the opponent.
        initData.CombatantType = CombatantType.Opponent;
        _opponentCombatant.Initialize(initData);
    }
    
    private void SetupListeners()
    {
        foreach (var detector in _collisionDetectors)
        {
            detector.OnCollisionEntered += HandleCollision;
            detector.OnCollisionOngoing += HandleOngoingCollision;
            detector.OnCollisionExited += HandleCollisionExit;
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
        CreateCollisionHandlerIfNeeded();
        
        _collisionHandler.HandleCollisionStart();
    }
    
    private void HandleOngoingCollision(Collision collision)
    {
        CreateCollisionHandlerIfNeeded();
        
        _collisionHandler.HandleOngoingCollision();
    }
    
    private void HandleCollisionExit(Collision collision)
    {
        CreateCollisionHandlerIfNeeded();
        
        _collisionHandler.HandleCollisionExit();
    }
    
    private void CreateCollisionHandlerIfNeeded()
    {
        if (_collisionHandler == null)
        {
            _collisionHandler = new ThumbCollisionHandler(_playerCombatant, _opponentCombatant, HandlePinStarted, HandlePinEscaped,_collisionDamage, _pinDamage);
        }
    }

    private void HandlePinStarted(CombantantController pinner, CombantantController target)
    {
        // Tell the pinner they began a pin
        pinner.SetAsPinning();
        
        // Tell the target they are being pinned
        target.SetAsPinned();
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

    private void HandlePinEscaped(CombantantController pinner, CombantantController target)
    {
        target.StopGettingPinned();
        pinner.SetAsNoLongerPinning();
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