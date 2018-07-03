using System.Collections.Generic;

public interface IValueEffect : IEffect
{
    List<IValueModifier> Modifiers { get; }

    void AddModifier(IValueModifier modifier);
    int ComparePriorities(IModifier a, IModifier b);
    float ModifiedValue();
}
