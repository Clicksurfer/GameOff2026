using UnityEngine;

public class ThumbMover : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _thumbTransform;
    [SerializeField] private DirectionGiver _directionGiver;

    [Header("Configuration")]
    [SerializeField] private Vector3 _thumbTransformOffset = new Vector3(0f, 0f, 0f);
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
        _thumbTransform.localPosition = _thumbTransformOffset + new Vector3(direction.x, 0, direction.y) * _thumbDistanceMultiplier;
    }
}
