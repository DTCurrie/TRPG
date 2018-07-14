using UnityEngine;
using System.Collections;

public class DurationStatusCondition : MonoBehaviour, IStatusCondition
{
    public int Duration = 10;

    private void OnEnable() => AddObservers();
    private void OnDisable() => RemoveObservers();

    private void OnNewTurn(object sender, object args)
    {
        Duration--;
        if (Duration <= 0) this.Remove();
    }

    public void AddObservers() =>
        this.AddObserver(OnNewTurn, TurnOrderController.BeforeRoundMessage);

    public void RemoveObservers() =>
        this.RemoveObserver(OnNewTurn, TurnOrderController.OnRoundCompletedMessage);
}
