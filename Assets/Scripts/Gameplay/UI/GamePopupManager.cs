using System;
using UnityEngine;

public class GamePopupManager : MonoBehaviour
{
    [SerializeField] private GameObject _startMatchPopup;
    [SerializeField] private GameStateManager _gameStateManager;

    private void Awake()
    {
        _gameStateManager.OnGameStateChanged += ShowPopupForStateIfNeeded;
    }

    private void ShowPopupForStateIfNeeded(GameState gameState)
    {
        if (gameState == GameState.Countdown)
        {
            OnCountdownStarted();
        }
    }

    private void OnCountdownStarted()
    {
        ShowStartMatchPopup();
    }

    public void ShowStartMatchPopup()
    {
        _startMatchPopup.SetActive(true);
    }
}
