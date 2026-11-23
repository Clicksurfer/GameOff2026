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

    public ThumbCollisionHandler(CombantantController c1, CombantantController c2, float clashingDamage = 1f, float pinDamage = 5f)
    {
        _combatantOne = c1;
        _combatantTwo = c2;
        _clashingDamage = clashingDamage;
        _pinDamage = pinDamage;
    }

    public void HandleCollision()
    {
        // Establish thumb vectors
        Vector2 thumbVectorOne = _combatantOne.GetThumbVector();
        Vector2 thumbVectorTwo = _combatantTwo.GetThumbVector();
        
        // Determine interaction
        if (VectorUtilities.AreVectorsRoughlyInTheSameDirection(thumbVectorOne, thumbVectorTwo))
        {
            // Thumbs are in the same direction - stun should ensue
            // TODO - Figure out how to stun
            Debug.Log("Stun!");
            return;
        }
        
        if (VectorUtilities.AreVectorsRoughlyOpposite(thumbVectorOne, thumbVectorTwo))
        {
            // Thumbs are in opposite directions - they're clashing and should slowly damage each other
            _combatantOne.TakeDamage(_clashingDamage);
            _combatantTwo.TakeDamage(_clashingDamage);
            Debug.Log("Clash!");
            return;
        }
        
        if (VectorUtilities.AreVectorsRoughlyPerpendicular(thumbVectorOne, thumbVectorTwo))
        {
            // A thumb is trying to pin the other one. Figure out which one.
            // Since thumbs with a "down" vector shouldn't be reachable, the "pinner" should be the one with a right or left vector.
            CombantantController victim = Mathf.Abs(thumbVectorOne.x) > Mathf.Abs(thumbVectorTwo.x) ? _combatantTwo : _combatantOne;
            victim.TakeDamage(_pinDamage);
            Debug.Log("Pin!");
            return;
        }
    }
}