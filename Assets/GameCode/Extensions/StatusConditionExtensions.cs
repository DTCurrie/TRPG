using UnityEngine;

public static class StatusConditionExtensions
{
    public static void Remove<T>(this T condition) where T : Component, IStatusCondition
    {
        var status = condition.GetComponentInParent<Status>();
        if (status) status.Remove(condition);
    }
}
