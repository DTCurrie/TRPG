using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public const string AddedMessage = "Status.AddedMessage";
    public const string RemovedMessage = "Status.RemovedMessage";

    public U Add<T, U>() where T : Component, IStatusEffect where U : Component, IStatusCondition
    {
        var effect = GetComponentInChildren<T>();

        if (effect == null)
        {
            effect = gameObject.AddChildComponent<T>();
            this.PostMessage(AddedMessage, effect);
        }

        var condition = effect.gameObject.AddChildComponent<U>();
        return condition;
    }

    public void Remove<T>(T condition) where T : Component, IStatusCondition
    {
        var effect = (Component)condition.GetComponentInParent<IStatusEffect>();

        condition.transform.SetParent(null);
        Destroy(condition.gameObject);

        condition = effect.GetComponentInChildren<T>();
        if (condition == null)
        {
            effect.transform.SetParent(null);
            Destroy(effect.gameObject);
            this.PostMessage(RemovedMessage, effect);
        }

    }
}
