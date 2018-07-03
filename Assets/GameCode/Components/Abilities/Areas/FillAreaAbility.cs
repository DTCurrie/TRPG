using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

public class FillAreaAbility : MonoBehaviour, IAbilityArea
{
    public List<Tile> GetTilesInArea(Board board, float2 coordinates)
    {
        var range = GetComponent<IAbilityRange>();
        return range.GetTilesInRange(board);
    }
}
