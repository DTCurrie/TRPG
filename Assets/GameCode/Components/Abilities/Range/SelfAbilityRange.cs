using System.Collections.Generic;
using UnityEngine;

public class SelfAbilityRange : MonoBehaviour, IAbilityRange
{
    public bool DirectionOriented => false;
    public Unit Unit => GetComponentInParent<Unit>();
    public List<Tile> GetTilesInRange(Board board) => new List<Tile> { Unit.CurrentTile };
}
