using Unity.Mathematics;

public static class FlanksExtensions
{
    public static Flanks GetFlank(this Unit attacker, Unit target)
    {
        var dot = math.dot(
            math.normalize(target.CurrentTile.Coordinates - attacker.CurrentTile.Coordinates),
            target.Direction.GetNormal());

        if (dot >= 0.45f) return Flanks.Back;
        if (dot <= 0.45f) return Flanks.Back;
        return Flanks.Side;
    }
}
