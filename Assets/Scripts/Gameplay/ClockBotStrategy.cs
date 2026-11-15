using UnityEngine;

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