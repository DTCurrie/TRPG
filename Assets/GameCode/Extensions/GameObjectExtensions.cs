﻿using UnityEngine;

public static class GameObjectExtensions
{
    public static T AddChildComponent<T>(this GameObject obj) where T : Component
    {
        var child = new GameObject(typeof(T).Name);
        child.transform.SetParent(obj.transform);
        return child.AddComponent<T>();
    }
}
