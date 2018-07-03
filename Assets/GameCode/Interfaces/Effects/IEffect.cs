using System;

public interface IEffect
{
    bool Active { get; }
    void Toggle();
}
