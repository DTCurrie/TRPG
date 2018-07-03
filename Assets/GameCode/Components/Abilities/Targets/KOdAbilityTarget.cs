using UnityEngine;
using System.Collections;

public class KOdAbilityTarget : MonoBehaviour, IAbilityTarget
{
    public bool IsTarget(Tile tile)
    {
        if (tile == null || tile.Content == null) return false;
        var stats = tile.GetComponent<Stats>();
        return stats != null && stats[StatTypes.HP] <= 0;
    }
}
