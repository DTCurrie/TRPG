using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MovementExtensions
{
    public static bool SimpleSearch(this IMovement movement, Tile from) => (from.PathfindingCost + 1) <= movement.Range;

    public static void FilterOccupiedTiles(List<Tile> tileList)
    {
        for (var i = 0; i < tileList.Count; i++)
            if (tileList[i].Content != null)
                tileList.RemoveAt(i);
    }

    public static List<Tile> GetTilesInRange(this IMovement movement, Board board)
    {
        var tileList = board.RangeSearch(movement.Unit.CurrentTile, movement.ExpandSearch);
        FilterOccupiedTiles(tileList);
        return tileList;
    }

    public static IEnumerator Turn(this IMovement movement, Directions direction)
    {
        var transform = ((Component)movement).transform;
        var rotation = transform.rotation;
        var targetRotation = Quaternion.Euler(direction.ToEuler());
        var speed = 3f;
        var time = (Quaternion.Angle(rotation, targetRotation) / 90) / speed;
        var threshold = 0.5f;
        var elapsed = 0f;

        while (Quaternion.Angle(transform.rotation, targetRotation) >= threshold)
        {
            elapsed += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(rotation, targetRotation, elapsed / time);
            yield return new WaitForEndOfFrame();
        }

        transform.rotation = targetRotation;
        yield return null;
    }
}
