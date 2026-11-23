using System;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public event Action<Collision> OnCollisionEntered;
    public event Action<Collision> OnCollisionOngoing;
    public event Action<Collision> OnCollisionExited;

    private void OnCollisionEnter(Collision other)
    {
        OnCollisionEntered?.Invoke(other);
    }

    private void OnCollisionStay(Collision other)
    {
        OnCollisionOngoing?.Invoke(other);
    }
    
    private void OnCollisionExit(Collision other)
    {
        OnCollisionExited?.Invoke(other);
    }
}