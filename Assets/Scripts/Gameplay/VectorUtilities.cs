using UnityEngine;

public static class VectorUtilities
{

    public static bool AreVectorsRoughlyPerpendicular(Vector2 v1, Vector2 v2, float tolerance = 0.1f)
    {
        float dotProduct = Vector2.Dot(v1.normalized, v2.normalized);
        return Mathf.Abs(dotProduct) < tolerance;
    }

    public static bool AreVectorsRoughlyInTheSameDirection(Vector2 v1, Vector2 v2, float tolerance = 0.2f)
    {
        float dotProduct = Vector2.Dot(v1.normalized, v2.normalized);
        return dotProduct > (1f - tolerance);
    }

    public static bool AreVectorsRoughlyOpposite(Vector2 v1, Vector2 v2, float tolerance = 0.2f)
    {
        float dotProduct = Vector2.Dot(v1.normalized, v2.normalized);
        return dotProduct < (-1f + tolerance);
    }
}