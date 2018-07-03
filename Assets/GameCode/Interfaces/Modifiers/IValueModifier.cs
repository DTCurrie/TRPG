using System;

public interface IValueModifier : IModifier
{
    float Modify(float value);
}
