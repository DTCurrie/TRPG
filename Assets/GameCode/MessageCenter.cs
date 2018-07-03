using System.Collections.Generic;
using UnityEngine;

using Handler = System.Action<System.Object, System.Object>;
using SenderTable = System.Collections.Generic.Dictionary<System.Object, System.Collections.Generic.List<System.Action<System.Object, System.Object>>>;

public class MessageCenter
{
    private readonly Dictionary<string, SenderTable> _tables = new Dictionary<string, SenderTable>();
    private readonly HashSet<List<Handler>> _invoking = new HashSet<List<Handler>>();

    public static readonly MessageCenter Instance = new MessageCenter();

    public void AddObserver(Handler handler, string messageType) =>
        AddObserver(handler, messageType, null);

    public void AddObserver(Handler handler, string messageType, System.Object sender)
    {
        if (handler == null)
        {
            Debug.LogError("Can't add null handler for message type " + messageType);
            return;
        }

        if (string.IsNullOrEmpty(messageType))
        {
            Debug.LogError("Can't observe a message with no type");
            return;
        }

        if (!_tables.ContainsKey(messageType))
            _tables.Add(messageType, new SenderTable());

        var senders = _tables[messageType];
        var key = sender ?? this;

        if (!senders.ContainsKey(key)) senders.Add(key, new List<Handler>());

        var list = senders[key];

        if (!list.Contains(handler))
        {
            if (_invoking.Contains(list)) senders[key] = new List<Handler>(list);
            list.Add(handler);
        }
    }

    public void RemoveObserver(Handler handler, string messageType) =>
        RemoveObserver(handler, messageType, null);

    public void RemoveObserver(Handler handler, string messageType, System.Object sender)
    {
        if (handler == null)
        {
            Debug.LogError("Can't remove null handler for message type " + messageType);
            return;
        }

        if (string.IsNullOrEmpty(messageType))
        {
            Debug.LogError("Can't stop observing a message with no type");
            return;
        }

        if (!_tables.ContainsKey(messageType)) return;

        var senders = _tables[messageType];
        var key = sender ?? this;

        if (!senders.ContainsKey(key)) return;

        var list = senders[key];
        var index = list.IndexOf(handler);

        if (index != -1)
        {
            if (_invoking.Contains(list))
                senders[key] = list = new List<Handler>(list);

            list.RemoveAt(index);
        }
    }

    public void Clean()
    {
        var messageTypes = new string[_tables.Keys.Count];
        _tables.Keys.CopyTo(messageTypes, 0);

        for (int i = messageTypes.Length - 1; i >= 0; --i)
        {
            var messageType = messageTypes[i];
            var senderTable = _tables[messageType];
            var senders = new object[senderTable.Keys.Count];

            senderTable.Keys.CopyTo(senders, 0);

            for (int j = senders.Length - 1; j >= 0; --j)
            {
                var sender = senders[j];
                var handlers = senderTable[sender];
                if (handlers.Count == 0) senderTable.Remove(sender);
            }

            if (senderTable.Count == 0) _tables.Remove(messageType);
        }
    }

    public void PostMessage(string messageType) => PostMessage(messageType, null);

    public void PostMessage(string messageType, System.Object sender) =>
        PostMessage(messageType, sender, null);

    public void PostMessage(string messageType, System.Object sender, System.Object e)
    {
        if (string.IsNullOrEmpty(messageType))
        {
            Debug.LogError("A message type is required");
            return;
        }

        if (!_tables.ContainsKey(messageType))
        {
            Debug.LogWarning("Message type " + messageType + " is not being observed.");
            return;
        }

        var senders = _tables[messageType];

        if (sender != null && senders.ContainsKey(sender))
        {
            var handlers = senders[sender];
            _invoking.Add(handlers);
            for (int i = 0; i < handlers.Count; i++) handlers[i](sender, e);
            _invoking.Remove(handlers);
        }

        if (senders.ContainsKey(this))
        {
            var handlers = senders[this];
            _invoking.Add(handlers);
            for (int i = 0; i < handlers.Count; i++) handlers[i](sender, e);
            _invoking.Remove(handlers);
        }
    }
}
