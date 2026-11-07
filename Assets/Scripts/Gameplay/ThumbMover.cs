using UnityEngine;

public class ThumbMover : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _thumbTransform;
    [SerializeField] private DirectionGiver _directionGiver;

    [Header("Configuration")]
    [SerializeField] private float _thumbDistanceMultiplier = 1f;

    private void FixedUpdate()
    {
        Vector2 moveDirection = GetDirectionToMoveTo();
        MoveThumb(moveDirection);
    }
    
    private Vector2 GetDirectionToMoveTo()
    {
        return _directionGiver.GetDirectionalVector();
    }
    
    private void MoveThumb(Vector2 direction)
    {
        _thumbTransform.localPosition = direction * _thumbDistanceMultiplier;
    }
}
