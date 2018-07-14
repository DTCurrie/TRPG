﻿using System;

public class MultiplyDeltaModifier : IValueModifier
{
    private readonly int _priority;

    public readonly float Value;

    public int Priority => _priority;

    public MultiplyDeltaModifier(int priority, float value)
    {
        _priority = priority;
        Value = value;
    }

    public float Modify(float from, float to) => from + ((to - from) * Value);
}
