using UnityEngine;
using System.Collections;

public class MagicalAbilityPower : MonoBehaviour, IAbilityPower
{
    public int Level;

    private void OnEnable() => AddObservers();
    private void OnDisable() => RemoveObservers();

    public int GetBaseAttack() => GetComponentInParent<Stats>()[StatTypes.MAT];
    public int GetBaseDefense(Unit target) => target.GetComponent<Stats>()[StatTypes.MDF];
    public int GetPower() => Level;

    public void AddObservers() => this.AddPowerObservers();
    public void RemoveObservers() => this.RemovePowerObservers();
}