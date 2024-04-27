using UnityEngine;

public static class Vector3Extension
{
    public static Vector3 SumWithoutZCoordinate(this Vector3 vector3, Vector2 vector2)
    {
        return new Vector3(vector3.x + vector2.x, vector3.y + vector2.y, 1);
    }
}