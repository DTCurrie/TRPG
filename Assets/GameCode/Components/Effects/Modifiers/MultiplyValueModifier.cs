using System;

public class MultiplyValueModifier : IValueModifier
{
    private readonly int _priority;

    public readonly float Value;

    public int Priority => _priority;

    public MultiplyValueModifier(int priority, float value)
    {
        _priority = priority;
        Value = value;
    }

    public float Modify(float value) => value * Value;
}
