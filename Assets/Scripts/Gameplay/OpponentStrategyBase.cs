using UnityEngine;

public abstract class OpponentStrategyBase : IDirectionGiver
{
    private MonoBehaviour _owner;
    
    public OpponentStrategyBase(MonoBehaviour owner)
    {
        _owner = owner;
    }
    
    public abstract Vector2 GetDirectionalVector();
}