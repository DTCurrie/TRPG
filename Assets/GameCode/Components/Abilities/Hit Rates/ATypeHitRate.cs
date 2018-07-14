using Unity.Mathematics;
using UnityEngine;

public class ATypeHitRate : MonoBehaviour, IHitRate
{
    private Unit _unit;

    private void Start()
    {
        _unit = GetComponentInParent<Unit>();
    }

    private int GetEvade(Unit target)
    {
        var stats = target.GetComponentInParent<Stats>();
        return math.clamp(stats[StatTypes.EVD], 0, 100);
    }

    private int AdjustForRelativeFlank(Unit attacker, Unit target, int rate)
    {
        switch (attacker.GetFlank(target))
        {
            case Flanks.Front: return rate;
            case Flanks.Side: return rate / 2;
            default: return rate / 4;
        }
    }

    public int Calculate(Tile target)
    {
        var targetUnit = target.Content.GetComponent<Unit>();
        if (targetUnit != null)
        {
            if (this.AutomaticHit(_unit, targetUnit)) return this.Final(0);
            if (this.AutomaticMiss(_unit, targetUnit)) return this.Final(100);

            var evade = GetEvade(targetUnit);
            evade = AdjustForRelativeFlank(_unit, targetUnit, evade);
            evade = this.AdjustForStatusEffects(_unit, targetUnit, evade);
            evade = math.clamp(evade, 5, 95);
            return this.Final(evade);
        }

        return this.Final(100);
    }
}
