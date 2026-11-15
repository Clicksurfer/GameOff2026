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
    private float _positionTime = 2f;
    
    private float _timeElapsed;
    private float _rightTime;
    private float _downTime;
    private float _leftTime;
    private float _upTime;
    private float _cycleTime;

    public ClockBotStrategy(MonoBehaviour owner) : base(owner)
    {
        _timeElapsed = 0f;
        _rightTime = _positionTime;
        _downTime = _rightTime + _positionTime;
        _leftTime = _downTime + _positionTime;
        _upTime = _leftTime + _positionTime;
        _cycleTime = _upTime;
    }

    public override Vector2 GetDirectionalVector()
    {
        _timeElapsed += Time.fixedDeltaTime;
        float timeInCycle = _timeElapsed % _cycleTime;
        if (timeInCycle < _rightTime)
        {
            return Vector2.right;
        }
        if (timeInCycle < _downTime)
        {
            return Vector2.down;
        }
        if (timeInCycle < _leftTime)
        {
            return Vector2.left;
        }
        return Vector2.up;
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