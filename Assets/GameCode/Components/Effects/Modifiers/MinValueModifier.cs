using Unity.Mathematics;

public class MinValueModifier : IValueModifier
{
    private readonly int _priority;

    public readonly float Min;

    public int Priority => _priority;

    public MinValueModifier(int priority, float min)
    {
        _priority = priority;
        Min = min;
    }

    public float Modify(float from, float to) => math.min(to, Min);
}