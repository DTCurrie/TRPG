using System;
public static class EffectExtensions
{
    public static void Toggle(this IEffect effect)
    {
        effect.Active = !effect.DefaultActive;
    }
}
