using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MovesBotStrategy : OpponentStrategyBase
{
    private float _timingVariance = 0.3f;

    private float _iterimIdleDuration = 1f;
    private float _moveIdleDuration = 1f;
    private float _individualSwipeDuration = 0.5f;
    private float _pinDuration = 1f;
    private float _defendDuration = 2.5f;
    
    private int _minSwipes = 1;
    private int _maxSwipes = 5;

    private Move _currentMove;
    private Vector2 _direction;
    
    public MovesBotStrategy(MonoBehaviour owner) : base(owner)
    {
        owner.StartCoroutine(MoveChangingCoroutine());
    }

    public override Vector2 GetDirectionalVector()
    {
        return _direction;
    }
    
    private IEnumerator MoveChangingCoroutine()
    {
        _currentMove = Move.Idle;
        while (true)
        {
            yield return DoInterimIdle();
            _currentMove = GetNextMove(_currentMove);
            Debug.Log($"MovesBotStrategy: Next Move: {_currentMove}");
            yield return DoMove(_currentMove);
        }
    }

    #region Moves

    private IEnumerator DoMove(Move currentMove)
    {
        switch (currentMove)
        {
            case Move.Pin:
                yield return DoPin();
                break;
            case Move.Defend:
                yield return DoDefend();
                break;
            case Move.Swipe:
                yield return DoSwipes();
                break;
            case Move.Idle:
            default:
                yield return DoIdle();
                break;
        }
    }
    
    private IEnumerator DoInterimIdle()
    {
        yield return DoSimpleMoveForSeconds(Vector2.zero, GetVariantDuration(_iterimIdleDuration));
    }

    private IEnumerator DoPin()
    {
        yield return DoSimpleMoveForSeconds(Vector2.up, GetVariantDuration(_pinDuration));
    }
    
    private IEnumerator DoDefend()
    {
        yield return DoSimpleMoveForSeconds(Vector2.down, GetVariantDuration(_defendDuration));
    }
    
    private IEnumerator DoIdle()
    {
        yield return DoSimpleMoveForSeconds(Vector2.zero, GetVariantDuration(_moveIdleDuration));
    }
    
    private IEnumerator DoSwipes()
    {
        int swipeCount = Random.Range(_minSwipes, _maxSwipes + 1);
        bool isRightSwipe = Random.value > 0.5f;
        for (int i = 0; i < swipeCount; i++)
        {
            Vector2 swipeDirection = isRightSwipe ? Vector2.right : Vector2.left;
            isRightSwipe = !isRightSwipe; // Alternate direction for next swipe
            yield return DoSimpleMoveForSeconds(swipeDirection, GetVariantDuration(_individualSwipeDuration));
        }
    }
    
    private IEnumerator DoSimpleMoveForSeconds(Vector2 direction, float duration, Move? move = null)
    {
        if (move != null)
        {
            _currentMove = move.Value;
        }
        _direction = direction;
        yield return new WaitForSeconds(duration);
    }

    #endregion

    private Move GetNextMove(Move currentMove, bool allowRepeats = false)
    {
        List<Move> possibleMoves = Enum.GetValues(typeof(Move))
            .Cast<Move>()
            .ToList();
        if (!allowRepeats)
        {
            possibleMoves.Remove(currentMove);
        }
        
        int randomIndex = Random.Range(0, possibleMoves.Count);
        return possibleMoves[randomIndex];
    }

    private float GetVariantDuration(float baseDuration)
    {
        float variance = Random.Range(-_timingVariance, _timingVariance);
        return Mathf.Max(baseDuration + variance, 0f);
    }

    enum Move
    {
        Swipe, Pin, Defend, Idle
    }
}