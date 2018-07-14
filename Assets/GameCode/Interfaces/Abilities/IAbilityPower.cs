using System;
public interface IAbilityPower : IObserver
{
    int GetBaseAttack();
    int GetBaseDefense(Unit target);
    int GetPower();
}
