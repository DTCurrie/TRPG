using UnityEngine;

public class SlowStatusEffect : MonoBehaviour, IStatusEffect
{
    private Stats _stats;

    private void OnCounterModified(object sender, object args)
    {
        var effect = (ValueChangeEffect)args;
        var modifier = new MultiplyDeltaModifier(0, 0.5f);
        effect.AddModifier(modifier);
    }

    private void OnEnable()
    {
        _stats = GetComponentInParent<Stats>();
        if (_stats) AddObservers();
    }

    private void OnDisable() => RemoveObservers();

    public void AddObservers() =>
        this.AddObserver(OnCounterModified, Stats.BeforeChangeMessage(StatTypes.CTR), _stats);

    public void RemoveObservers() =>
        this.RemoveObserver(OnCounterModified, Stats.BeforeChangeMessage(StatTypes.CTR), _stats);
}
