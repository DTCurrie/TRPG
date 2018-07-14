using Unity.Mathematics;
using UnityEngine;

public class PoisonStatusEffect : MonoBehaviour, IStatusEffect
{
    private Unit _unit;

    private void OnNewTurn(object sender, object args)
    {
        var stats = GetComponentInParent<Stats>();
        var currentHP = stats[StatTypes.HP];
        var maxHP = stats[StatTypes.MHP];
        var damage = math.min(currentHP, (int)math.floor(maxHP * 0.1f));

        this.PostMessage(stats.OnDamagedMessage, damage);
    }

    private void OnEnable()
    {
        _unit = GetComponentInParent<Unit>();
        if (_unit) AddObservers();
    }

    private void OnDisable() => RemoveObservers();

    public void AddObservers() =>
        this.AddObserver(OnNewTurn, TurnOrderController.BeforeTurnMessage, _unit);

    public void RemoveObservers() =>
        this.RemoveObserver(OnNewTurn, TurnOrderController.BeforeTurnMessage, _unit);
}
