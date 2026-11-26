using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndgamePopupView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _playerImage;
    [SerializeField] private Image _opponentImage;
    [SerializeField] private TMP_Text _text;

    [Header("Config")]
    [SerializeField] private float _delay = 1f;
    [SerializeField] private float _fadeInDuration = .6f;
    [SerializeField] private float _imagesFadeInDuration = .5f;
    [SerializeField] private float _textFadeInDuration = .5f;
    [SerializeField] private EndgameVisualsConfig _playerWinConfig;
    [SerializeField] private EndgameVisualsConfig _opponentWinConfig;
    
    public void Show(bool playerWon)
    {
        SetValues(playerWon);
        StartCoroutine(ShowCoroutine());
    }
    
    private void SetValues(bool playerWon)
    {
        // Set visual states based on winner
        EndgameVisualsConfig config = playerWon ? _playerWinConfig : _opponentWinConfig;
        _playerImage.sprite = config.PlayerSprite;
        _opponentImage.sprite = config.OpponentSprite;
        _text.text = config.Text;
        
        // Set these always
        _canvasGroup.alpha = 0f;
        _playerImage.gameObject.SetActive(false);
        _playerImage.color = new Color(_playerImage.color.r, _playerImage.color.g, _playerImage.color.b, 0f);
        _opponentImage.gameObject.SetActive(false);
        _opponentImage.color = new Color(_opponentImage.color.r, _opponentImage.color.g, _opponentImage.color.b, 0f);
        _text.gameObject.SetActive(false);
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 0f);
        gameObject.SetActive(true);
    }

    private IEnumerator ShowCoroutine()
    {
        yield return new WaitForSeconds(_delay);
        
        _canvasGroup.DOFade(1, _fadeInDuration);
        yield return new WaitForSeconds(_fadeInDuration);
        
        _playerImage.gameObject.SetActive(true);
        _opponentImage.gameObject.SetActive(true);
        _playerImage.DOFade(1, _imagesFadeInDuration);
        _opponentImage.DOFade(1, _imagesFadeInDuration);
        yield return new WaitForSeconds(_imagesFadeInDuration);
        
        _text.gameObject.SetActive(true);
        _text.DOFade(1, _textFadeInDuration);
    }
}