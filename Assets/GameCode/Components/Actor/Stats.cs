using System;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour, IObserver
{
    private readonly int[] _data = new int[(int)StatTypes.Count];

    private static Dictionary<StatTypes, string> _beforeChangeMessages;
    private static Dictionary<StatTypes, string> _onChangeMessages;

    public Guid guid = Guid.NewGuid();
    public string OnDamagedMessage => $"{guid}.OnDamagedMessage";

    public int this[StatTypes type] { get => _data[(int)type]; set => SetValue(type, value, true); }

    private void OnEnable() => AddObservers();
    private void OnDestroy() => RemoveObservers();

    private void OnDamaged(object sender, object e) => this[StatTypes.HP] -= (int)e;

    public void AddObservers() => this.AddObserver(OnDamaged, OnDamagedMessage);
    public void RemoveObservers() => this.RemoveObserver(OnDamaged, OnDamagedMessage);

    public static string BeforeChangeMessage(StatTypes type)
    {
        if (_beforeChangeMessages == null)
            _beforeChangeMessages = new Dictionary<StatTypes, string>();

        if (!_beforeChangeMessages.ContainsKey(type))
            _beforeChangeMessages.Add(type, $"Stats.{type.ToString()}.BeforeChange");
        return _beforeChangeMessages[type];
    }

    public static string OnChangeMessage(StatTypes type)
    {
        if (_onChangeMessages == null)
            _onChangeMessages = new Dictionary<StatTypes, string>();

        if (!_onChangeMessages.ContainsKey(type))
            _onChangeMessages.Add(type, $"Stats.{type.ToString()}.OnChange");
        return _onChangeMessages[type];
    }

    public void SetValue(StatTypes type, int value, bool allowEffects)
    {
        var currentValue = this[type];
        if (currentValue == value) return;

        if (allowEffects)
        {
            var effect = new ValueChangeEffect(currentValue, value);
            this.PostMessage(BeforeChangeMessage(type), effect);
            value = Mathf.FloorToInt(effect.ModifiedValue());
            if (!effect.Active || value == currentValue) return;
        }

        _data[(int)type] = value;
        this.PostMessage(OnChangeMessage(type), currentValue);
    }
}
