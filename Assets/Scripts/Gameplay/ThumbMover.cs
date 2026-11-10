using UnityEngine;

public class ThumbMover : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _thumbRootTransform;
    [SerializeField] private Transform _thumbDownRootReferenceTransform;
    [SerializeField] private Transform _thumbUpRootReferenceTransform;
    [SerializeField] private Transform _thumbLeftRootReferenceTransform;
    [SerializeField] private Transform _thumbRightRootReferenceTransform;
    [SerializeField] private DirectionGiver _directionGiver;

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
        float normalizedX = (direction.x + 1f) / 2f; // Normalize the x value to be between 0 and 1
        float normalizedY = (direction.y + 1f) / 2f; // Normalize the y value to be between 0 and 1
        
        // We control the thumb root's position and rotation based on the direction, and lerping between the reference transforms for the thumb root.
        Vector3 positionVerticalLerp = Vector3.Lerp(_thumbDownRootReferenceTransform.localPosition, _thumbUpRootReferenceTransform.localPosition, normalizedY);
        Vector3 positionHorizontalLerp = Vector3.Lerp(_thumbLeftRootReferenceTransform.localPosition, _thumbRightRootReferenceTransform.localPosition, normalizedX);
        Quaternion rotationVerticalLerp = Quaternion.Lerp(_thumbDownRootReferenceTransform.localRotation, _thumbUpRootReferenceTransform.localRotation, normalizedY);
        Quaternion rotationHorizontalLerp = Quaternion.Lerp(_thumbLeftRootReferenceTransform.localRotation, _thumbRightRootReferenceTransform.localRotation, normalizedX);

        _thumbRootTransform.localPosition = new Vector3(positionHorizontalLerp.x, positionVerticalLerp.y, _thumbRootTransform.localPosition.z);
        _thumbRootTransform.localRotation = new Quaternion(rotationHorizontalLerp.x, rotationVerticalLerp.y, rotationHorizontalLerp.z, rotationVerticalLerp.w);
    }
}
