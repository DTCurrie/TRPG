using UnityEngine;
using System.Collections;

public class StatusAttribute<T, U> : MonoBehaviour, IAttribute where T : Component, IStatusEffect where U : Component, IStatusCondition
{
    public U Condition;

    public GameObject Owner { get; private set; }
    public GameObject Target { get; private set; }

    public void Activate(GameObject target, bool use = false)
    {
        if (Target == null)
        {
            Target = target;
            OnActivate();
            if (use) Target = null;
        }
    }

    public void Deactivate()
    {
        if (Target != null)
        {
            OnDeactivate();
            Target = null;
        }
    }

    public void OnActivate()
    {
        var status = GetComponentInParent<Status>();
        Condition = status.Add<T, U>();
    }

    public void OnDeactivate()
    {
        if (Condition != null) Condition.Remove();
    }
}

public class PoisonStatusAttribute : StatusAttribute<PoisonStatusEffect, DurationStatusCondition>
{
    private void Start() => Condition.Duration = 15;
}