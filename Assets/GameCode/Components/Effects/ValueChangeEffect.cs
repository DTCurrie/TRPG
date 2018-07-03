using System.Collections.Generic;

public class ValueChangeEffect : IValueEffect
{
    private bool _active;
    private List<IValueModifier> _modifiers;

    public readonly float FromValue;
    public readonly float ToValue;
    public float Delta => ToValue - FromValue;
    
    public bool Active => _active;
    public List<IValueModifier> Modifiers => _modifiers;

    public ValueChangeEffect(float fromValue, float toValue, bool active = true) 
    {
        FromValue = fromValue;
        ToValue = toValue;
        _active = active;
    }

    public void AddModifier(IValueModifier modifier)
    {
        if (_modifiers == null) _modifiers = new List<IValueModifier>();
        _modifiers.Add(modifier);
    }

    public float ModifiedValue()
    {
        var value = ToValue;
        if (_modifiers == null) return value;

        _modifiers.Sort(ComparePriorities);
        for (int i = 0; i < _modifiers.Count; ++i)
            value = _modifiers[i].Modify(value);

        return value;
    }

    public void Toggle() => _active = !_active;

    public int ComparePriorities(IModifier a, IModifier b) => a.Priority.CompareTo(b.Priority);
}
