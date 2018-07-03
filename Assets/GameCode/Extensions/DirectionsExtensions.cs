using Unity.Mathematics;
using UnityEngine;

public static class DirectionsExtensions
{
    public static Directions GetDirection(this Tile start, Tile end)
    {
        if (start.Coordinates.y < end.Coordinates.y) return Directions.North;
        if (start.Coordinates.x < end.Coordinates.x) return Directions.East;
        if (start.Coordinates.y > end.Coordinates.y) return Directions.South;
        return Directions.West;
    }

    public static Vector3 ToEuler(this Directions direction) =>
        new Vector3(0, (int)direction * 90, 0);

    public static Directions GetDirection(this float2 coordinates)
    {
        if (coordinates.y > 0)
            return Directions.North;
        if (coordinates.x > 0)
            return Directions.East;
        if (coordinates.y < 0)
            return Directions.South;
        return Directions.West;
    }
}
