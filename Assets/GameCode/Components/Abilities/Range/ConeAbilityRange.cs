using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ConeAbilityRange : MonoBehaviour, IAbilityRange
{
    public int Horizontal = 1;
    public int Vertical = int.MaxValue;

    public bool DirectionOriented => true;

    public Unit Unit => GetComponentInParent<Unit>();

    public List<Tile> GetTilesInRange(Board board)
    {
        var coordinates = Unit.CurrentTile.Coordinates;
        var tiles = new List<Tile>();
        var direction = (Unit.Direction == Directions.North || Unit.Direction == Directions.East) ? 1 : -1;
        var lateral = 1;

        if (Unit.Direction == Directions.North || Unit.Direction == Directions.South)
        {
            for (var y = 1; y < Horizontal; y++)
            {
                var min = -(lateral / 2);
                var max = lateral / 2;

                for (var x = min; x <= max; x++)
                {
                    var next = new float2((int)(coordinates.x + x), (int)(coordinates.y + (y * direction)));
                    var tile = board.GetTile(next);
                    if (ValidTile(tile)) tiles.Add(tile);
                }

                lateral += 2;
            }
        }
        else
        {
            for (var x = 1; x <= Horizontal; x++)
            {
                var min = -(lateral / 2);
                var max = (lateral / 2);

                for (var y = min; y <= max; y++)
                {
                    var next = new float2(((int)coordinates.x + (x * direction)), ((int)coordinates.y + y));
                    var tile = board.GetTile(next);
                    if (ValidTile(tile)) tiles.Add(tile);
                }

                lateral += 2;
            }
        }

        return tiles;
    }

    private bool ValidTile(Tile tile) =>
        tile != null && math.abs(tile.Height - Unit.CurrentTile.Height) <= Vertical;
}
