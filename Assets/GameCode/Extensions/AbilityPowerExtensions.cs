using System;
using System.Collections.Generic;
using UnityEngine;

public static class AbilityPowerExtensions
{
    private static void OnGetBaseAttack(this IAbilityPower power, object sender, object args)
    {
        var data = args as Data<Unit, Unit, List<IValueModifier>>;
        if (data.Arg0 != ((Component)power).GetComponentInParent<Unit>()) return;

        var mod = new AddValueModifier(0, power.GetBaseAttack());
        data.Arg2.Add(mod);
    }

    private static void OnGetBaseDefense(this IAbilityPower power, object sender, object args)
    {
        var data = args as Data<Unit, Unit, List<IValueModifier>>;
        if (data.Arg0 != ((Component)power).GetComponentInParent<Unit>()) return;

        var mod = new AddValueModifier(0, power.GetBaseDefense(data.Arg1));
        data.Arg2.Add(mod);
    }

    private static void OnGetPower(this IAbilityPower power, object sender, object args)
    {
        var data = args as Data<Unit, Unit, List<IValueModifier>>;
        if (data.Arg0 != ((Component)power).GetComponentInParent<Unit>()) return;

        var mod = new AddValueModifier(0, power.GetPower());
        data.Arg2.Add(mod);
    }

    public static void AddPowerObservers(this IAbilityPower power)
    {
        power.AddObserver(power.OnGetBaseAttack, DamageAbilityEffect.GetAttackMessage);
        power.AddObserver(power.OnGetBaseDefense, DamageAbilityEffect.GetDefenseMessage);
        power.AddObserver(power.OnGetPower, DamageAbilityEffect.GetPowerMessage);
    }

    public static void RemovePowerObservers(this IAbilityPower power)
    {
        power.RemoveObserver(power.OnGetBaseAttack, DamageAbilityEffect.GetAttackMessage);
        power.RemoveObserver(power.OnGetBaseDefense, DamageAbilityEffect.GetDefenseMessage);
        power.RemoveObserver(power.OnGetPower, DamageAbilityEffect.GetPowerMessage);
    }
}
