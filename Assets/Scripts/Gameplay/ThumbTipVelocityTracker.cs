using System.Collections.Generic;
using UnityEngine;

public class ThumbTipVelocityTracker : MonoBehaviour
{
    [SerializeField] private Transform _thumbTipTransform;
    [SerializeField] private int _thumbTipPosHistorySize = 5;

    private Queue<Vector3> _thumbTipPosHistory = new Queue<Vector3>();

    private void FixedUpdate()
    {
        UpdateThumbHistory();
    }
    
    public Vector3 GetThumbTipVelocity()
    {
        if (_thumbTipPosHistory.Count < 2)
        {
            return Vector3.zero;
        }

        Vector3 oldestPosition = _thumbTipPosHistory.Peek();
        Vector3 newestPosition = _thumbTipTransform.position;

        Vector3 velocity = (newestPosition - oldestPosition) / (Time.fixedDeltaTime * (_thumbTipPosHistory.Count - 1));
        return velocity;
    }
    
    private void UpdateThumbHistory()
    {
        if (_thumbTipPosHistory.Count >= _thumbTipPosHistorySize)
        {
            _thumbTipPosHistory.Dequeue();
        }
        _thumbTipPosHistory.Enqueue(_thumbTipTransform.position);
    }
}