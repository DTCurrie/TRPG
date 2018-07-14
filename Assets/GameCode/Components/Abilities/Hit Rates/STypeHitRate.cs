using Unity.Mathematics;
using UnityEngine;

public class STypeHitRate : MonoBehaviour, IHitRate
{
    private Unit _unit;

    private void Start()
    {
        _unit = GetComponentInParent<Unit>();
    }

    private int GetResistance(Unit target)
    {
        var stats = target.GetComponentInParent<Stats>();
        return stats[StatTypes.RES];
    }

    private int AdjustForRelativeFlank(Unit attacker, Unit target, int rate)
    {
        switch (attacker.GetFlank(target))
        {
            case Flanks.Front: return rate;
            case Flanks.Side: return rate - 10;
            default: return rate - 20;
        }
    }

    public int Calculate(Tile target)
    {
        var targetUnit = target.Content.GetComponent<Unit>();
        if (targetUnit != null)
        {
            if (this.AutomaticMiss(_unit, targetUnit)) return this.Final(100);
            if (this.AutomaticHit(_unit, targetUnit)) return this.Final(0);

            var resistance = GetResistance(targetUnit);
            resistance = this.AdjustForStatusEffects(_unit, targetUnit, resistance);
            resistance = AdjustForRelativeFlank(_unit, targetUnit, resistance);
            resistance = math.clamp(resistance, 0, 100);
            return this.Final(resistance);
        }

        return this.Final(100);
    }
}
