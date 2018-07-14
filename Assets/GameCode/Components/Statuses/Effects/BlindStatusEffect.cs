using UnityEngine;
using System.Collections;

public class BlindStatusEffect : MonoBehaviour, IStatusEffect
{
    private void OnHitRateStatusCheck(object sender, object args)
    {
        var data = (Data<Unit, Unit, int>)args;
        var owner = GetComponentInParent<Unit>();

        if (owner == data.Arg0)
            data.Arg2 += 50;
        else if (owner == data.Arg1)
            data.Arg2 -= 20;
    }

    private void OnEnable() => AddObservers();
    private void OnDisable() => RemoveObservers();

    public void AddObservers() =>
        this.AddObserver(OnHitRateStatusCheck, HitRateExtensions.OnStatusCheckMessage);

    public void RemoveObservers() =>
        this.RemoveObserver(OnHitRateStatusCheck, HitRateExtensions.OnStatusCheckMessage);
}
