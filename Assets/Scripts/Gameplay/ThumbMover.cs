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

    [Header("Config")]
    [SerializeField] private float _thumbMaxDistancePerFrame = 0.001f;
    [SerializeField] private float _thumbMaxDegreesPerFrame = 1f;
    
    private bool _isEnabled = false;

    private void FixedUpdate()
    {
        Vector2 moveDirection = GetDirectionToMoveTo();
        MoveThumb(moveDirection);
    }
    
    public void SetEnabled(bool isEnabled)
    {
        _isEnabled = isEnabled;
    }
    
    private Vector2 GetDirectionToMoveTo()
    {
        return _directionGiver.GetDirectionalVector();
    }
    
    private void MoveThumb(Vector2 direction)
    {
        if (_isEnabled)
        {
            return;
        }
        
        float normalizedX = (direction.x + 1f) / 2f; // Normalize the x value to be between 0 and 1
        float normalizedY = (direction.y + 1f) / 2f; // Normalize the y value to be between 0 and 1

        // We control the thumb root's position and rotation based on the direction, and lerping between the reference transforms for the thumb root.
        Vector3 positionVerticalLerp = Vector3.Lerp(_thumbDownRootReferenceTransform.localPosition, _thumbUpRootReferenceTransform.localPosition, normalizedY);
        Vector3 positionHorizontalLerp = Vector3.Lerp(_thumbLeftRootReferenceTransform.localPosition, _thumbRightRootReferenceTransform.localPosition, normalizedX);
        Quaternion rotationVerticalLerp = Quaternion.Lerp(_thumbDownRootReferenceTransform.localRotation, _thumbUpRootReferenceTransform.localRotation, normalizedY);
        Quaternion rotationHorizontalLerp = Quaternion.Lerp(_thumbLeftRootReferenceTransform.localRotation, _thumbRightRootReferenceTransform.localRotation, normalizedX);
        
        // Now we lerp between horizontal and vertical based on the direction vector
        float verticalToHorizontalLerpFactor = Mathf.Abs(direction.x) / (Mathf.Abs(direction.x) + Mathf.Abs(direction.y));
        Vector3 positionLerp = Vector3.Lerp(positionVerticalLerp, positionHorizontalLerp, verticalToHorizontalLerpFactor);
        Quaternion rotationLerp = Quaternion.Lerp(rotationVerticalLerp, rotationHorizontalLerp, verticalToHorizontalLerpFactor);

        // Instead of directory setting the rotation and position of the thumb, we will move towards it, giving the sensation of "weight".
        _thumbRootTransform.localPosition =
            Vector3.MoveTowards(_thumbRootTransform.localPosition, positionLerp, _thumbMaxDistancePerFrame);
        _thumbRootTransform.localRotation =
            Quaternion.RotateTowards(_thumbRootTransform.localRotation, rotationLerp, _thumbMaxDegreesPerFrame);
    }
}