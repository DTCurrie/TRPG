using UnityEngine;
using System.Collections;
using System;

public class WeaponAbilityPower : MonoBehaviour, IAbilityPower, IObserver
{
    private void OnEnable() => AddObservers();
    private void OnDisable() => RemoveObservers();

    public int GetBaseAttack() => GetComponentInParent<Stats>()[StatTypes.ATK];
    public int GetBaseDefense(Unit target) => target.GetComponent<Stats>()[StatTypes.DEF];

    public void AddObservers() => this.AddPowerObservers();
    public void RemoveObservers() => this.RemovePowerObservers();

    private int PowerFromEquippedWeapon()
    {
        var power = 0;
        var equipment = GetComponentInParent<Equipment>();
        var item = equipment.GetItem(EquipmentSlots.Primary);
        var features = item.GetComponentsInChildren<StatModifierAttribute>();

        for (var i = 0; i < features.Length; i++)
            if (features[i].Stat == StatTypes.ATK)
                power += features[i].Modifier;

        return power;
    }

    private int UnarmedPower()
    {
        var job = GetComponentInParent<Job>();

        for (var i = 0; i < Job.StatOrder.Length; i++)
            if (Job.StatOrder[i] == StatTypes.ATK)
                return job.BaseStats[i];

        return 0;
    }

    public int GetPower()
    {
        var power = PowerFromEquippedWeapon();
        return power > 0 ? power : UnarmedPower();
    }
}
