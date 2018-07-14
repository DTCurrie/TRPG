using UnityEngine;
using System.Collections;

public class PhysicalAbilityPower : MonoBehaviour, IAbilityPower, IObserver
{
    public int Level;

    private void OnEnable() => AddObservers();
    private void OnDisable() => RemoveObservers();

    public int GetBaseAttack() => GetComponentInParent<Stats>()[StatTypes.ATK];
    public int GetBaseDefense(Unit target) => target.GetComponent<Stats>()[StatTypes.DEF];
    public int GetPower() => Level;

    public void AddObservers() => this.AddPowerObservers();
    public void RemoveObservers() => this.RemovePowerObservers();
}
