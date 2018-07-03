using UnityEngine;
using System.Collections;

public class DefaultAbilityTarget : MonoBehaviour, IAbilityTarget
{
    public bool IsTarget(Tile tile)
    {
        if (tile == null || tile.Content == null) return false;
        var stats = tile.Content.GetComponent<Stats>();
        return stats != null && stats[StatTypes.HP] > 0;
    }
}
