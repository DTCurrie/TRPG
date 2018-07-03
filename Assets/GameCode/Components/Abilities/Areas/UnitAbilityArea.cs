using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

public class UnitAbilityArea : MonoBehaviour, IAbilityArea
{
    public List<Tile> GetTilesInArea(Board board, float2 coordinates)
    {
        var tiles = new List<Tile>();
        var tile = board.GetTile(coordinates);
        if (tile != null) tiles.Add(tile);
        return tiles;
    }
}
