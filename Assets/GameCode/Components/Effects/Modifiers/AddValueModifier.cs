
public class AddValueModifier : IValueModifier
{
    private readonly int _priority;

    public readonly float Value;

    public int Priority => _priority;

    public AddValueModifier(int priority, float value)
    {
        _priority = priority;
        Value = value;
    }

    public float Modify(float from, float to) => to + Value;
}