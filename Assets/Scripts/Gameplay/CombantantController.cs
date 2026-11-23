using System;
using System.Collections;
using Gameplay;
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
    [SerializeField] private ThumbMover _thumbMover;
    
    // Initialization data
    private CombatantType _combatantType;
    private float _maxHealth;
    private float _tryingToPinDurationUntilStun;
    private float _stunDuration;
    private float _pinAttemptRecoverySpeed;
    
    // State
    private CombatantFlags _combatantFlags;
    private float _currentHealth;
    private ThumbState _thumbState;

    // Pinning
    private float _timeUntilStun;

    private void FixedUpdate()
    {
        // We determine the intention of the thumb based on the direction giver
        _thumbState = GetThumbState(_directionGiver.GetDirectionalVector());

        HandlePinAndStun();
    }

    public void Initialize(CombatantInitializationData data)
    {
        if (data.InitialHealth <= 0f)
        {
            throw new ArgumentException("Initial health must be greater than zero.", nameof(data.InitialHealth));
        }

        _combatantType = data.CombatantType;
        _maxHealth = data.InitialHealth;
        _currentHealth = _maxHealth;
        _tryingToPinDurationUntilStun = data.TryingToPinDurationUntilStun;
        _stunDuration = data.StunDuration;
        _pinAttemptRecoverySpeed = data.PinAttemptRecoverySpeed;
        _timeUntilStun = _tryingToPinDurationUntilStun;
        OnInitialized?.Invoke(_combatantType);
    }

    #region Pin

    /// <summary>
    /// Mark as pinning. This prevents the thumb from getting stunned due to failed pin attempts.
    /// </summary>
    public void SetAsPinning()
    {
        _thumbState = ThumbState.Pinning;
    }
    
    /// <summary>
    /// Marks the combatant as pinned. While pinned, they can't attack, take damage and their only viable move is to block (escape) if able.
    /// </summary>
    public void SetAsPinned()
    {
        _combatantFlags.SetFlag(CombatantFlags.Pinned);
        _thumbMover.SetEnabled(true);
    }
    
    /// <summary>
    /// The player is no longer pinned, either because they escaped or because the opponent stopped pinning
    /// </summary>
    public void StopGettingPinned()
    {
        _combatantFlags.ClearFlag(CombatantFlags.Pinned);
        _thumbMover.SetEnabled(false);
    }

    /// <summary>
    /// The player stopped pinning. They will now be vulnerable to being stunned if they continue to hold the pin position.
    /// </summary>
    public void SetAsNoLongerPinning()
    {
        if (_thumbState == ThumbState.Pinning)
        {
            _thumbState = ThumbState.TryingToPin;
        }
    }

    private void HandlePinAndStun()
    {
        if (!_combatantFlags.HasAnyFlag() && _thumbState == ThumbState.TryingToPin)
        {
            _timeUntilStun -= Time.fixedDeltaTime;
            
            // Stun player if needed
            if (_timeUntilStun <= 0f && !_combatantFlags.HasFlag(CombatantFlags.Stunned))
            {
                Stun(_stunDuration);
            }
        }
        else
        {
            // Recover stun timer over time
            _timeUntilStun += _pinAttemptRecoverySpeed * Time.fixedDeltaTime;
            _timeUntilStun = Mathf.Min(_timeUntilStun, _tryingToPinDurationUntilStun);
        }
    }

    #endregion


    #region Stun

    public void Stun(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        OnStunned?.Invoke(_combatantType);
        _combatantFlags.SetFlag(CombatantFlags.Stunned);
        if (duration > 0f)
        {
            yield return new WaitForSeconds(duration);
        }
        _combatantFlags.ClearFlag(CombatantFlags.Stunned);
        OnStunDone?.Invoke(_combatantType);
    }

    #endregion

    
    public void TakeDamage(float damage)
    {
        // Don't hurt dead player
        if (_combatantFlags.HasFlag(CombatantFlags.Dead))
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
            _combatantFlags.SetFlag(CombatantFlags.Dead);
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
    
    private ThumbState GetThumbState(Vector2 thumbVector)
    {
        bool forcefulVector = thumbVector.magnitude >= 0.5f;
        
        if (forcefulVector && VectorUtilities.AreVectorsRoughlyInTheSameDirection(thumbVector, Vector2.down))
        {
            if (_combatantFlags.HasFlag(CombatantFlags.Pinned) && !_combatantFlags.HasFlag(CombatantFlags.Stunned))
            {
                // Not stunned and trying to defend - escape pin
                _thumbMover.SetEnabled(false);
            }
            return ThumbState.Blocking;
        }
        
        if (forcefulVector && VectorUtilities.AreVectorsRoughlyInTheSameDirection(thumbVector, Vector2.up))
        {
            if (_thumbState == ThumbState.Pinning)
            {
                // Ongoing pin
                return ThumbState.Pinning;
            }
            
            return ThumbState.TryingToPin;
        }
        
        return ThumbState.Swiping;
    }

    public ThumbState GetThumbState()
    {
        return _thumbState;
    }
    
    public CombatantFlags GetFlags()
    {
        return _combatantFlags;
    }
}