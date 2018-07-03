using UnityEngine;
using System.Collections.Generic;

public class Equipment : MonoBehaviour
{
    public const string EquipMessage = "Equipment.Equip";
    public const string UnequipMessage = "Equipment.Unequip";

    private List<Equippable> _items = new List<Equippable>();
    public IList<Equippable> Items => _items.AsReadOnly();

    public void Equip(Equippable equipment, EquipmentSlots slot)
    {
        Unequip(slot);

        _items.Add(equipment);
        equipment.transform.SetParent(transform);
        equipment.CurrentSlot = slot;
        equipment.OnEquip();

        this.PostMessage(EquipMessage, equipment);
    }

    public void Unequip(Equippable equipment)
    {
        equipment.OnUnequip();
        equipment.CurrentSlot = EquipmentSlots.None;
        equipment.transform.SetParent(transform);
        _items.Remove(equipment);

        this.PostMessage(UnequipMessage, equipment);
    }

    public void Unequip(EquipmentSlots slot)
    {
        for (var i = 0; i < _items.Count; i++)
        {
            var item = _items[i];
            if ((item.CurrentSlot & slot) != EquipmentSlots.None)
                Unequip(item);
        }
    }
}
