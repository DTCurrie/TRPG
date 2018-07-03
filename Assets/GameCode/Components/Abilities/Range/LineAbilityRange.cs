using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LineAbilityRange : MonoBehaviour, IAbilityRange
{
    public int Horizontal = 3;
    public int Vertical = 1;

    public bool DirectionOriented => true;

    public Unit Unit => GetComponentInParent<Unit>();

    public List<Tile> GetTilesInRange(Board board)
    {
        var start = Unit.CurrentTile.Coordinates;
        var end = new float2();
        var tiles = new List<Tile>();

        switch (Unit.Direction)
        {
            case Directions.North:
                end = new float2(start.x, start.y + Horizontal);
                break;
            case Directions.East:
                end = new float2(start.x + Horizontal, start.y);
                break;
            case Directions.South:
                end = new float2(start.x, start.y - Horizontal);
                break;
            case Directions.West:
                end = new float2(start.x - Horizontal, start.y);
                break;
        }

        for (int i = 0; i < Horizontal; i++)
        {
            if (start.x < end.x) start.x++;
            else if (start.x > end.x) start.x--;

            if (start.y < end.y) start.y++;
            else if (start.y > end.y) start.y--;

            var tile = board.GetTile(start);
            if (tile != null && math.abs(tile.Height - Unit.CurrentTile.Height) <= Vertical)
                tiles.Add(tile);
        }


        return tiles;
    }
}
