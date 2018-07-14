using System;

public interface IValueModifier : IModifier
{
    float Modify(float from, float to);
}
