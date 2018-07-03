using UnityEngine;
using System.Collections;

public class FlagEffect : MonoBehaviour, IEffect
{
    private bool _active;
    public bool Active => _active;

    public FlagEffect(bool active)
    {
        _active = active;
    }

    public void Toggle() => _active = !_active;
}
