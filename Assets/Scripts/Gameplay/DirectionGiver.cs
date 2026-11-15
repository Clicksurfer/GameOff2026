using UnityEngine;

public abstract class DirectionGiver : MonoBehaviour, IDirectionGiver
{
    public abstract Vector2 GetDirectionalVector();
}