using UnityEngine;
using System.Collections;
using System;

public class StatusTest : MonoBehaviour
{
    public Unit CursedUnit;
    public Equippable CursedItem;
    public int Step;

    private void OnEnable()
    {
        this.AddObserver(OnTurnCheck, TurnOrderController.OnTurnCheckedMessage);
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnTurnCheck, TurnOrderController.OnTurnCheckedMessage);
    }


    private void Add<T>(Unit target, int duration) where T : Component, IStatusEffect
    {
        var condition = target.GetComponent<Status>().Add<T, DurationStatusCondition>();
        condition.Duration = duration;
    }

    private void EquipCursedItem(Unit target)
    {
        var obj = new GameObject("Cursed Sword");
        var feature = obj.AddComponent<PoisonStatusAttribute>();
        var equipment = target.GetComponent<Equipment>();

        CursedUnit = target;
        CursedItem = obj.AddComponent<Equippable>();
        CursedItem.DefaultSlot = EquipmentSlots.Primary;

        equipment.Equip(CursedItem, EquipmentSlots.Primary);
    }

    private void UnEquipCursedItem(Unit target)
    {
        if (target != CursedUnit || Step < 10) return;

        var equipment = target.GetComponent<Equipment>();
        equipment.Unequip(CursedItem);

        Destroy(CursedItem.gameObject);
        Destroy(this);
    }

    private void OnTurnCheck(object sender, object args)
    {
        var effect = (FlagEffect)args;
        if (effect == null || effect.Active == false) return;

        var target = (Unit)sender;
        switch (Step)
        {
            case 0:
                EquipCursedItem(target);
                break;
            case 1:
                Add<SlowStatusEffect>(target, 15);
                break;
            case 2:
                Add<StopStatusEffect>(target, 15);
                break;
            case 3:
                Add<HasteStatusEffect>(target, 15);
                break;
            default:
                UnEquipCursedItem(target);
                break;
        }

        Step++;
    }
}
