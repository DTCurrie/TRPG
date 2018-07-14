using Unity.Mathematics;

public class ClampValueModifier : IValueModifier
{
    private readonly int _priority;

    public readonly float Min;
    public readonly float Max;

    public int Priority => _priority;

    public ClampValueModifier(int priority, float min, float max)
    {
        _priority = priority;
        Min = min;
        Max = max;
    }

    public float Modify(float from, float to) => math.clamp(to, Min, Max);
}
