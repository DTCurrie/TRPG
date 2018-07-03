using System;

[Flags]
public enum EquipmentSlots
{
    None = 0,
    Primary = 1 << 0,
    Secondary = 1 << 1,
    Head = 1 << 2,
    Body = 1 << 3,
    Accesory = 1 << 4
}
