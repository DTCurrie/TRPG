using System;
using System.Collections;
using UnityEngine;

using Handler = System.Action<System.Object, System.Object>;

public static class MessageExtensions
{
    public static void PostMessage(this object obj, string messageType) =>
        MessageCenter.Instance.PostMessage(messageType, obj);

    public static void PostMessage(this object obj, string messageType, object e) =>
        MessageCenter.Instance.PostMessage(messageType, obj, e);

    public static void AddObserver(this object obj, Handler handler, string messageType) =>
        MessageCenter.Instance.AddObserver(handler, messageType);

    public static void AddObserver(this object obj, Handler handler, string messageType, object sender) =>
        MessageCenter.Instance.AddObserver(handler, messageType, sender);

    public static void RemoveObserver(this object obj, Handler handler, string messageType) =>
        MessageCenter.Instance.RemoveObserver(handler, messageType);

    public static void RemoveObserver(this object obj, Handler handler, string messageType, System.Object sender) =>
        MessageCenter.Instance.RemoveObserver(handler, messageType, sender);
}