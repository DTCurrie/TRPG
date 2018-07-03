using UnityEngine;

public class Equippable : MonoBehaviour
{
    private bool _equipped;
    private IAttribute[] _attributes;

    public EquipmentSlots DefaultSlot;
    public EquipmentSlots SecondarySlot;
    public EquipmentSlots CurrentSlot;

    public void OnEquip()
    {
        if (_equipped) return;
        _equipped = true;

        _attributes = GetComponentsInChildren<IAttribute>();
        for (var i = 0; i < _attributes.Length; i++)
            _attributes[i].Activate(transform.parent.gameObject);
    }

    public void OnUnequip()
    {
        if (!_equipped) return;
        _equipped = false;

        for (var i = 0; i < _attributes.Length; i++)
            _attributes[i].Deactivate();
    }
}
