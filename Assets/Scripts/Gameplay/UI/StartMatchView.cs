using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class StartMatchView : MonoBehaviour, IGameStateProgressor
{
    [Header("References")]
    [SerializeField] private TMP_Text _countdownText;
    [SerializeField] private TMP_Text _startMatchText;

    [Header("Animation Settings - General")]
    [SerializeField] private float _animationStartDelay = 1f;
    
    [Header("Animation Settings - Countdown")]
    [SerializeField] private float _countdownSpeedPerNumber = 1f;
    [SerializeField] private float _countdownPunchDurationMultiplier = 0.5f;
    [SerializeField] private Vector3 _countdownTextPunchScale = new Vector3(1.5f, 1.5f, 1f);
    [SerializeField] private int _countdownTextPunchVibrato = 10;
    [SerializeField] private float _countdownTextPunchElasticity = 1f;
    
    [Header("Animation Settings - Start")]
    [SerializeField] private float _startMatchTextPunchDuration = 0.5f;
    [SerializeField] private Vector3 _startMatchTextPunchScale = new Vector3(1.5f, 1.5f, 1f);
    [SerializeField] private int _startMatchTextPunchVibrato = 10;
    [SerializeField] private float _startMatchTextPunchElasticity = 1f;
    [SerializeField] private float _startMatchDelayUntilFadeStarts = .7f;
    [SerializeField] private float _startMatchDelayFadeDuration = 0.5f;
    
    private Action<GameState> _updateGameStateCallback;

    private void Start()
    {
        Cleanup();
        StartCoroutine(MatchStartAnimationCoroutine());
    }

    private void Cleanup()
    {
        _countdownText.gameObject.SetActive(false);
        _startMatchText.gameObject.SetActive(false);
    }
    
    private IEnumerator MatchStartAnimationCoroutine()
    {
        yield return new WaitForSecondsRealtime(_animationStartDelay);
        
        yield return CountdownTextCoroutine();
        
        yield return StartMatchCoroutine();

        Cleanup();
    }

    private IEnumerator StartMatchCoroutine()
    {
        _startMatchText.gameObject.SetActive(true);
        _startMatchText.transform.DOPunchScale(_startMatchTextPunchScale, _startMatchTextPunchDuration,
            _startMatchTextPunchVibrato,
            _startMatchTextPunchElasticity).SetUpdate(true);
        yield return new WaitForSecondsRealtime(_startMatchDelayUntilFadeStarts);
        
        // At this point in time - we can tell the game to start proper
        _updateGameStateCallback?.Invoke(GameState.Playing);
        
        _startMatchText.DOFade(0f, _startMatchDelayFadeDuration).SetUpdate(true);
        yield return new WaitForSecondsRealtime(_startMatchDelayFadeDuration);
    }

    private IEnumerator CountdownTextCoroutine()
    {
        _countdownText.gameObject.SetActive(true);
        float punchDuration = _countdownSpeedPerNumber * _countdownPunchDurationMultiplier;
        
        for (int i = 3; i > 0; i--)
        {
            _countdownText.text = i.ToString();
            _countdownText.transform.DOPunchScale(_countdownTextPunchScale, punchDuration, _countdownTextPunchVibrato,
                _countdownTextPunchElasticity).SetUpdate(true);
            yield return new WaitForSecondsRealtime(_countdownSpeedPerNumber);
        }
        _countdownText.gameObject.SetActive(false);
    }

    public void InvokeThisWhenGameStateShouldUpdate(GameState newState, Action<GameState> onGameStateChanged)
    {
        _updateGameStateCallback = onGameStateChanged;
    }
}
