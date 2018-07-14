using System.Collections.Generic;

public class ValueChangeEffect : IEffect
{
    public readonly float FromValue;
    public readonly float ToValue;
    public float Delta => ToValue - FromValue;

    public bool DefaultActive { get; private set; }
    public bool Active { get; set; }
    public List<IValueModifier> Modifiers { get; private set; }

    public ValueChangeEffect(float fromValue, float toValue, bool defaultActive = true)
    {
        FromValue = fromValue;
        ToValue = toValue;
        DefaultActive = defaultActive;
        Active = defaultActive;
    }

    public void AddModifier(IValueModifier modifier)
    {
        if (Modifiers == null) Modifiers = new List<IValueModifier>();
        Modifiers.Add(modifier);
    }

    public float ModifiedValue()
    {
        if (Modifiers == null) return ToValue;
        var value = ToValue;

        Modifiers.Sort(ComparePriorities);
        for (int i = 0; i < Modifiers.Count; ++i)
            value = Modifiers[i].Modify(FromValue, value);

        return value;
    }

    public int ComparePriorities(IModifier a, IModifier b) => a.Priority.CompareTo(b.Priority);
}
