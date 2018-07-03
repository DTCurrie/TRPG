using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ConstantAbilityRange : MonoBehaviour, IAbilityRange
{
    public int Horizontal = 1;
    public int Vertical = int.MaxValue;

    public bool DirectionOriented => false;

    public Unit Unit => GetComponentInParent<Unit>();

    public List<Tile> GetTilesInRange(Board board) =>
        board.RangeSearch(Unit.CurrentTile, ExpandSearch);

    private bool ExpandSearch(Tile from, Tile to) =>
        (from.PathfindingCost + 1) <= Horizontal && math.abs(to.Height - Unit.CurrentTile.Height) <= Vertical;
}
