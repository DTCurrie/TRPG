using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

public class SpecifiedAbilityArea : MonoBehaviour, IAbilityArea
{
    private Tile _tile;

    public int Horizontal;
    public int Vertical;

    private bool ExpandSearch(Tile from, Tile to) =>
        (from.PathfindingCost + 1) <= Horizontal && math.abs(to.Height - _tile.Height) <= Vertical;

    public List<Tile> GetTilesInArea(Board board, float2 coordinates)
    {
        _tile = board.GetTile(coordinates);
        return board.RangeSearch(_tile, ExpandSearch);
    }
}
