using UnityEngine;

public class StopStatusEffect : MonoBehaviour, IStatusEffect
{
    private Stats _stats;

    private void OnCounterModified(object sender, object args)
    {
        var effect = (ValueChangeEffect)args;
        effect.Toggle();
    }

    private void OnAutomaticHitCheck(object sender, object args)
    {
        var owner = GetComponentInParent<Unit>();
        var effect = (MatchEffect)args;
        if (owner == effect.Target) effect.Toggle();
    }

    private void OnEnable()
    {
        _stats = GetComponentInParent<Stats>();
        AddObservers();
    }

    private void OnDisable() => RemoveObservers();

    public void AddObservers()
    {
        if (_stats != null)
            this.AddObserver(OnCounterModified, Stats.BeforeChangeMessage(StatTypes.CTR), _stats);

        this.AddObserver(OnAutomaticHitCheck, HitRateExtensions.OnAutomaticHitCheckMessage);
    }

    public void RemoveObservers()
    {
        this.RemoveObserver(OnCounterModified, Stats.BeforeChangeMessage(StatTypes.CTR), _stats);
        this.RemoveObserver(OnAutomaticHitCheck, HitRateExtensions.OnAutomaticHitCheckMessage);
    }
}
