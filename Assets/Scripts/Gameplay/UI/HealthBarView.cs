using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider _slider;

    [Header("Config")]
    [Tooltip("How quickly it can go from 0 to 1")] [SerializeField] private float _tweenSpeed = 1f;
    [SerializeField] private Ease _tweenEase = Ease.Linear;

    private Tween _tween;
    
    public void UpdateView(float target)
    {
        if (_slider == null)
        {
            Debug.LogWarning("Slider reference is not set in HealthBarView. Please assign a Slider component in the inspector.");
            return;
        }
        
        // Clamp input
        target = Mathf.Clamp01(target);
        
        KillProgressBarTween();
        TweenProgress(target);
    }

    private void TweenProgress(float target)
    {
        float currentValue = _slider.value;
        float distance = Mathf.Abs(target - currentValue);
        float duration = distance / _tweenSpeed;
        _tween = _slider.DOValue(target, duration).SetEase(_tweenEase);
    }

    private void KillProgressBarTween()
    {
        _tween?.Kill();
    }
}
