using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public event Action<GameState> OnGameStateChanged;

    // I would ideally put interfaces here, but Unity doesn't know how to serialize them. This is a crappy workaround.
    [SerializeField] private GameObject _startCountdownProgressorGameObject;
    [SerializeField] private GameObject _startMatchProgressorGameObject;
    [SerializeField] private GameObject _pauseGameProgressorGameObject;
    [SerializeField] private GameObject _endMatchProgressorGameObject;
    
    private GameState _gameState;

    private void Awake()
    {
        ConnectProgressors();
    }

    private void ConnectProgressors()
    {
        TryConnectProgressor(_startCountdownProgressorGameObject, StartCountdown, GameState.Countdown);
        TryConnectProgressor(_startMatchProgressorGameObject, StartMatch, GameState.Playing);
        TryConnectProgressor(_pauseGameProgressorGameObject, PauseGame, GameState.Paused);
        TryConnectProgressor(_endMatchProgressorGameObject, EndMatch, GameState.GameWon);
    }
    
    private void StartCountdown(GameState proposedState)
    {
        TransitionToStateIfAllowed(proposedState, GameState.Countdown);
    }
    
    private void StartMatch(GameState proposedState)
    {
        TransitionToStateIfAllowed(proposedState, GameState.Playing);
    }
    
    private void PauseGame(GameState proposedState)
    {
        TransitionToStateIfAllowed(GameState.Paused, GameState.Paused);
    }
    
    private void EndMatch(GameState proposedState)
    {
        TransitionToStateIfAllowed(proposedState, GameState.GameLost, GameState.GameWon);
    }
    
    private bool TryConnectProgressor(GameObject progressorImplementingGameObject, Action<GameState> methodToInvokeWhenProgressShouldHappen, GameState gameStateType)
    {
        if (progressorImplementingGameObject == null)
        {
            Debug.LogWarning("Progressor GameObject is not assigned in the inspector. This means that the corresponding game state transition will not be triggered.");
            return false;
        }
        
        if (progressorImplementingGameObject.TryGetComponent(out IGameStateProgressor progressor))
        {
            progressor.InvokeThisWhenGameStateShouldUpdate(gameStateType, methodToInvokeWhenProgressShouldHappen);
            return true;
        }
        return false;
    }

    #region Utility methods

    private void TransitionToStateIfAllowed(GameState proposedNewState, params GameState[] allowedNewStates)
    {
        if (IsStateAllowed(proposedNewState, allowedNewStates))
        {
            GameState = proposedNewState;
        }
    }

    private bool IsStateAllowed(GameState proposedNewState, params GameState[] allowedNewStates)
    {
        if (allowedNewStates == null || allowedNewStates.Length == 0)
        {
            Debug.LogError("IsStateAllowed called with no allowed states. This shouldn't happen.");
            return false;
        }
        
        foreach (var allowedState in allowedNewStates)
        {
            if (proposedNewState == allowedState)
            {
                return true;
            }
        }
        
        Debug.LogWarning($"Proposed new state {proposedNewState} is not allowed. Allowed states are: {string.Join(", ", allowedNewStates)}");
        return false;
    }

    #endregion
    

    private GameState GameState
    {
        get => _gameState;
        set
        {
            if (_gameState != value)
            {
                _gameState = value;
                OnGameStateChanged?.Invoke(_gameState);
                Debug.Log($"Game state changed to: {_gameState}");
            }
        }
    }
}