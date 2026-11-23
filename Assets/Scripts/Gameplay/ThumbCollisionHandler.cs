using System;
using Gameplay;
using UnityEngine;

/// <summary>
/// The core of the game - where we determine what each thumb was doing, and what should be happening as a result
/// </summary>
public class ThumbCollisionHandler
{
    private CombantantController _combatantOne;
    private CombantantController _combatantTwo;
    private float _clashingDamage;
    private float _pinDamage;
    private event Action <CombantantController, CombantantController> _onPinStarted;
    private event Action <CombantantController, CombantantController> _onPinStopped;
    public ThumbCollisionHandler(CombantantController c1, CombantantController c2, Action<CombantantController, CombantantController> onPinStarted, Action<CombantantController, CombantantController> onPinStopped, float clashingDamage = 1f, float pinDamage = 5f)
    {
        _combatantOne = c1;
        _combatantTwo = c2;
        _clashingDamage = clashingDamage;
        _pinDamage = pinDamage;
        _onPinStarted = onPinStarted;
        _onPinStopped = onPinStopped;
    }

    public void HandleCollisionStart()
    {
        // For COLLISIONS specifically, we're only looking at swipe damage.
        // As such, we only care if the thumbs can be swiped at or not and if one of them is swiping.
        TrySwipe(_combatantOne, _combatantTwo);
        TrySwipe(_combatantTwo, _combatantOne);
    }
    
    public void HandleOngoingCollision()
    {
        // For ONGOING collisions, we're only looking at ongoing pin damage.
        TryPin(_combatantOne, _combatantTwo);
        TryPin(_combatantTwo, _combatantOne);
    }
    
    public void HandleCollisionExit()
    {
        // Try to stop pin if it's happening
        TryStopPin(_combatantOne, _combatantTwo);
        TryStopPin(_combatantTwo, _combatantOne);
    }

    private bool TrySwipe(CombantantController potentialAttacker, CombantantController target)
    {
        // First, establish the player intends to swipe and also can swipe.
        if (potentialAttacker.GetThumbState() != ThumbState.Swiping)
        {
            return false;
        }

        if (potentialAttacker.GetFlags().HasAnyFlag())
        {
            return false;
        }
        
        // Next, establish the opponent is swipable
        if (target.GetThumbState() == ThumbState.Pinning || target.GetThumbState() == ThumbState.TryingToPin)
        {
            return false;
        }
        
        // Swiping possible - perform attack
        target.TakeDamage(_clashingDamage);
        Debug.Log(potentialAttacker.name + " swiped " + target.name + " for " + _clashingDamage + " damage!");
        return true;
    }
    
    private bool TryPin(CombantantController potentialPinner, CombantantController target)
    {
        // First, establish the player intends to pin and also can pin.
        if (potentialPinner.GetThumbState() != ThumbState.Pinning && potentialPinner.GetThumbState() != ThumbState.TryingToPin)
        {
            return false;
        }

        if (potentialPinner.GetFlags().HasFlag(CombatantFlags.Dead))
        {
            return false;
        }
        
        // Next, establish the opponent is pinnable
        if (target.GetThumbState() == ThumbState.Blocking)
        {
            return false;
        }
        
        // Pinning possible
        // First - if this is the first frame of pinning, we need to notify the game that a pin has started.
        if (potentialPinner.GetThumbState() == ThumbState.TryingToPin)
        {
            _onPinStarted?.Invoke(potentialPinner, target);
        }
        
        // Damage for pin
        target.TakeDamage(_pinDamage);
        Debug.Log(potentialPinner.name + " pinning " + target.name + " for " + _pinDamage + " damage!");
        return true;
    }
    
    private bool TryStopPin(CombantantController potentialPinner, CombantantController target)
    {
        // If the pinner is no longer pinning, we need to stop the pin.
        if (potentialPinner.GetThumbState() == ThumbState.Pinning && !potentialPinner.GetFlags().HasAnyFlag())
        {
            if (target.GetFlags().HasFlag(CombatantFlags.Pinned))
            {
                _onPinStopped?.Invoke(potentialPinner, target);
                Debug.Log(potentialPinner.name + " stopped pinning " + target.name);
                return true;
            }
        }

        return false;
    }
}