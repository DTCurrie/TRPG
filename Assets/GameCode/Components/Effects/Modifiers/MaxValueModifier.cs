using Unity.Mathematics;

public class MaxValueModifier : IValueModifier
{
    private readonly int _priority;

    public readonly float Max;

    public int Priority => _priority;

    public MaxValueModifier(int priority, float max)
    {
        _priority = priority;
        Max = max;
    }

    public float Modify(float value) => math.max(value, Max);
}
