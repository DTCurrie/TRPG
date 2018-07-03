using System;
using UnityEngine;

public interface IAttribute
{
    GameObject Owner { get; }
    GameObject Target { get; }

    void Activate(GameObject target, bool use = false);
    void Deactivate();

    void OnActivate();
    void OnDeactivate();
}
