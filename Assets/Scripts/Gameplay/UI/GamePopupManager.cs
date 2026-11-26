using UnityEngine;

public class GamePopupManager : MonoBehaviour
{
    [SerializeField] private GameObject _startMatchPopup;
    [SerializeField] private GameStateManager _gameStateManager;
    [SerializeField] private EndgamePopupView _endgamePopupView;

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
        
        else if (gameState == GameState.GameWon)
        {
            _endgamePopupView.Show(true);
        }
        
        else if (gameState == GameState.GameLost)
        {
            _endgamePopupView.Show(false);
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
