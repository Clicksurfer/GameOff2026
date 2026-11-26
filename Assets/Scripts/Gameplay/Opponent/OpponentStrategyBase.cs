using UnityEngine;

public abstract class OpponentStrategyBase : IDirectionGiver
{
    protected MonoBehaviour _owner;
    
    public OpponentStrategyBase(MonoBehaviour owner)
    {
        _owner = owner;
    }
    
    public abstract Vector2 GetDirectionalVector();
}