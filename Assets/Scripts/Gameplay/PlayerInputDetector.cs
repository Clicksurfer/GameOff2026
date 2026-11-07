using UnityEngine;

public class PlayerInputDetector : MonoBehaviour
{
    [Tooltip("What is the minimum relative distance from the reference point where the vector is considered maxed out. 1 means that when the mouse is on the screen bounds it's considered maxed. 0.5 means when its halfway from the reference to the screen bound.")]
    [Range(0f,1f)] [SerializeField] private float _maxRelativeYDistance = 0.5f;
    
    [Tooltip("Controls the position of the reference point relative to the centre of the screen.")]
    [SerializeField] private Vector2 _referencePointDiffToCentre = new Vector2(0f, 0f);

    /// <summary>
    /// When invoked, checks the position of the mouse relative to the anchor point (screen center) and returns the directional vector, scaled to reference power
    /// </summary>
    public Vector2 GetDirectionalMouseVector()
    {
        Vector2 screenCentre = new Vector3(Screen.width / 2f, Screen.height / 2f);
        Vector2 referencePoint = screenCentre + _referencePointDiffToCentre;
        
        Vector2 mousePosition = Input.mousePosition;
        
        Vector2 relativeMousePosition = mousePosition - referencePoint;
        
        // Edge case - if max relative distance is 0, directional vector is always maxed
        if (_maxRelativeYDistance <= 0f)
        {
            return relativeMousePosition.normalized;
        }
        
        // Screens are horizontal, so the max magnitude is calculated based on the Y distance from the reference point to the screen bounds
        float maxMagnitude = Mathf.Min(Screen.height - referencePoint.y, referencePoint.y) * _maxRelativeYDistance;
        
        // Scale the relative mouse position to the max magnitude
        Vector2 scaledVector = relativeMousePosition / maxMagnitude;
        if (scaledVector.magnitude > 1f) // Make sure value is clamped so it doesn't pass 1.
        {
            scaledVector = scaledVector.normalized;
        }
        
        return scaledVector;
    }
}
