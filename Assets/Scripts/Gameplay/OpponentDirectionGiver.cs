using System;
using UnityEngine;

public class OpponentDirectionGiver : DirectionGiver
{
    [SerializeField] private OpponentStrategy _strategy;
    
    private OpponentStrategyBase _strategyInstance;

    private void Awake()
    {
        _strategyInstance = CreateStrategyInstance(_strategy);
    }
    
    public override Vector2 GetDirectionalVector()
    {
        return _strategyInstance.GetDirectionalVector();
    }

    private OpponentStrategyBase CreateStrategyInstance(OpponentStrategy strategy)
    {
        switch (strategy)
        {
            case OpponentStrategy.ClockBot:
                return new ClockBotStrategy(this);
            default:
                throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
        }
    }
}

public class ClockBotStrategy : OpponentStrategyBase
{
    private float _timeElapsed;

    public ClockBotStrategy(MonoBehaviour owner) : base(owner)
    {
        _timeElapsed = 0f;
    }

    public override Vector2 GetDirectionalVector()
    {
        _timeElapsed += Time.fixedDeltaTime;
        switch ((int)_timeElapsed % 8)
        {
            case 0:
            case 1:
                return Vector2.right; // Move right for 2 seconds
            case 2:
            case 3:
                return Vector2.down; // Move down for 2 seconds
            case 4:
            case 5:
                return Vector2.left; // Move left for 2 seconds
            case 6:
            case 7:
                return Vector2.up; // Move up for 2 seconds
            default:
                return Vector2.zero; // Should never reach here
        }
    }
}

public abstract class OpponentStrategyBase : IDirectionGiver
{
    private MonoBehaviour _owner;
    
    public OpponentStrategyBase(MonoBehaviour owner)
    {
        _owner = owner;
    }
    
    public abstract Vector2 GetDirectionalVector();
}

public enum OpponentStrategy
{
    ClockBot
}