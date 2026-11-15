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