using Unity.Mathematics;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageAbilityEffect : MonoBehaviour, IAbilityEffect
{
    public const string GetAttackMessage = "DamageAbilityEffect.GetAttackMessage";
    public const string GetDefenseMessage = "DamageAbilityEffect.GetDefenseMessage";
    public const string GetPowerMessage = "DamageAbilityEffect.GetPowerMessage";
    public const string TweakDamageMessage = "DamageAbilityEffect.TweakDamageMessage";

    const int MinDamage = -9999;
    const int MaxDamage = 9999;

    private int GetStat(Unit attacker, Unit target, string message, int initial)
    {
        var mods = new List<IValueModifier>();
        var value = initial;

        this.PostMessage(message, new Data<Unit, Unit, List<IValueModifier>>(attacker, target, mods));
        mods.Sort((a, b) => a.Priority.CompareTo(b.Priority));

        for (int i = 0; i < mods.Count; ++i)
            value = (int)mods[i].Modify(initial, value);

        return Mathf.Clamp(Mathf.FloorToInt(value), MinDamage, MaxDamage);
    }

    public int Predict(Tile target)
    {
        var attacker = GetComponentInParent<Unit>();
        var defender = GetComponentInParent<Unit>();
        var attack = GetStat(attacker, defender, GetAttackMessage, 0);
        var defense = GetStat(attacker, defender, GetDefenseMessage, 0);
        var power = GetStat(attacker, defender, GetPowerMessage, 0);
        var damage = math.max(attack - (defense / 2), 1);

        damage = power * damage / 100;
        damage = math.max(damage, 1);
        damage = GetStat(attacker, defender, TweakDamageMessage, damage);

        return damage;
    }

    public void Apply(Tile target)
    {
        var defender = target.Content;
        if (defender != null)
        {
            var damage = Predict(target);
            damage *= (int)math.floor(Random.Range(0.9f, 1.1f));

            var stats = defender.GetComponent<Stats>();
            if (stats != null) this.PostMessage(stats.OnDamagedMessage, damage);
        }
    }
}
