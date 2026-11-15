using UnityEngine;

public abstract class DirectionGiver : MonoBehaviour, IDirectionGiver
{
    public abstract Vector2 GetDirectionalVector();
}

public interface IDirectionGiver
{
    Vector2 GetDirectionalVector();
}