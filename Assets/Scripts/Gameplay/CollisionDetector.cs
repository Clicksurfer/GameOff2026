using System;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public event Action<Collision> OnCollisionEntered;
    
    private void OnCollisionEnter(Collision other)
    {
        OnCollisionEntered?.Invoke(other);
    }
}