using UnityEngine;

public class InflictAbilityEffect<T> : MonoBehaviour, IAbilityEffect where T : Component, IStatusEffect
{
    public T Effect;
    public int Duration;

    public int Predict(Tile target) => 0;

    public void Apply(Tile target)
    {
        var status = target.Content.GetComponent<Status>();
        if (status != null)
        {
            var effect = status.Add<T, DurationStatusCondition>();
            effect.Duration = Duration;
        }
    }
}