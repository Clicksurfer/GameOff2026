using System;
using System.Collections;
using UnityEngine;

public class CombantantController : MonoBehaviour
{
    public event Action<CombatantType> OnInitialized;
    public event Action<CombatantType> OnStunned;
    public event Action<CombatantType> OnStunDone;
    public event Action<CombatantType> OnDied;
    
    [SerializeField] private HealthBarView _healthBarView;
    [SerializeField] private ThumbTipVelocityTracker _thumbTipVelocityTracker;
    [SerializeField] private DirectionGiver _directionGiver;
    
    private CombatantType _combatantType;
    private CombatantState _combatantState;
    private float _maxHealth;
    private float _currentHealth;
    
    public void Initialize(CombatantType combantantType, float initialHealth)
    {
        if (initialHealth <= 0f)
        {
            throw new ArgumentException("Initial health must be greater than zero.", nameof(initialHealth));
        }
        _combatantType = combantantType;
        _maxHealth = initialHealth;
        _currentHealth = initialHealth;
        OnInitialized?.Invoke(_combatantType);
    }
    
    public void Stun(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        OnStunned?.Invoke(_combatantType);
        _combatantState = CombatantState.Stunned;
        if (duration > 0f)
        {
            yield return new WaitForSeconds(duration);
        }
        _combatantState = CombatantState.Normal;
        OnStunDone?.Invoke(_combatantType);
    }
    
    public void TakeDamage(float damage)
    {
        // Don't hurt dead player
        if (_combatantState == CombatantState.Dead)
        {
            return;
        }
        
        // Sanity check max health
        if (_maxHealth <= 0f)
        {
            throw new InvalidOperationException("Max health must be greater than zero before taking damage.");
        }
        
        _currentHealth -= damage;
        _currentHealth = Mathf.Max(_currentHealth, 0f);
        _healthBarView.UpdateView(_currentHealth/_maxHealth);
        
        if (_currentHealth <= 0f)
        {
            _combatantState = CombatantState.Dead;
            OnDied?.Invoke(_combatantType);
        }
    }

    public Vector2 GetThumbVector()
    {
        // Thumb vector is determined partially by where the thumb's actually been, and what the combatant intends to do.
        Vector2 thumbTipVelocity = _thumbTipVelocityTracker.GetThumbTipVelocity();
        Vector2 intendedDirection = _directionGiver.GetDirectionalVector();
        Vector2 combinedVector = intendedDirection;//(thumbTipVelocity + intendedDirection) / 2f;
        return combinedVector;//(thumbTipVelocity + intendedDirection) / 2f;
    }
}