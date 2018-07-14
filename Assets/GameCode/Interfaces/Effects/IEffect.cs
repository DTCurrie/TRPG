using System;

public interface IEffect
{
    bool DefaultActive { get; }
    bool Active { get; set; }
}
