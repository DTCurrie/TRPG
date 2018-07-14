using System;
public class MatchEffect : IEffect
{
    public readonly Unit Attacker;
    public readonly Unit Target;

    public bool DefaultActive { get; private set; }
    public bool Active { get; set; }

    public MatchEffect(Unit attacker, Unit target, bool defaultActive = false)
    {
        Attacker = attacker;
        Target = target;
        DefaultActive = defaultActive;
        Active = defaultActive;
    }
}