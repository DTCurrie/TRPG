public static class HitRateExtensions
{
    public const string OnAutomaticHitCheckMessage = "HitRate.OnAutomaticHitCheckMessage";
    public const string OnAutomaticMissCheckMessage = "HitRate.OnAutomaticMissCheckMessage";
    public const string OnStatusCheckMessage = "HitRate.OnStatusCheckMessage";

    public static bool AutomaticHit(this IHitRate hitRate, Unit attacker, Unit target)
    {
        var effect = new MatchEffect(attacker, target);
        hitRate.PostMessage(OnAutomaticHitCheckMessage, effect);
        return effect.Active;
    }

    public static bool AutomaticMiss(this IHitRate hitRate, Unit attacker, Unit target)
    {
        var effect = new MatchEffect(attacker, target);
        hitRate.PostMessage(OnAutomaticMissCheckMessage, effect);
        return effect.Active;
    }

    public static int AdjustForStatusEffects(this IHitRate hitRate, Unit attacker, Unit target, int rate)
    {
        var args = new Data<Unit, Unit, int>(attacker, target, rate);
        hitRate.PostMessage(OnStatusCheckMessage, args);
        return args.Arg2;
    }

    public static int Final(this IHitRate hitRate, int evade) => 100 - evade;
}
