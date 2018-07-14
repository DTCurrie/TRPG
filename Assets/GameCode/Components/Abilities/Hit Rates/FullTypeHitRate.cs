using UnityEngine;

public class FullTypeHitRate : MonoBehaviour, IHitRate
{
    private Unit _unit;

    private void Start()
    {
        _unit = GetComponentInParent<Unit>();
    }

    public int Calculate(Tile target)
    {
        var targetUnit = target.Content.GetComponent<Unit>();
        if (targetUnit != null)
        {
            if (this.AutomaticMiss(_unit, targetUnit)) return this.Final(100);
            return this.Final(0);
        }

        return this.Final(100);
    }
}
