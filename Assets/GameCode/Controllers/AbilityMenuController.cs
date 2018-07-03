using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using TMPro;

public class AbilityMenuController : MonoBehaviour, IPooler<AbilityMenuEntry>
{
    private const int _menuCount = 4;
    private List<AbilityMenuEntry> _menuEntries = new List<AbilityMenuEntry>(_menuCount);

    private Vector2 _menuPosition;
    [SerializeField] private Vector2 _menuHiddenPosition;

    [SerializeField] private GameObject _entryPrefab;
    private int _index;
    private List<Poolable> _instances = new List<Poolable>();

    private RectTransform _entryPrefabRect;

    public TextMeshProUGUI Title;
    public AbilityMenuPanel Panel;
    public Canvas Canvas;

    public int CurrentSelection { get; private set; }

    private void Awake()
    {
        _index = PoolController.GetIndex("AbilityMenuEntries");
        PoolController.AddPool(_index, _entryPrefab, _menuCount, 0);
    }

    private void Start()
    {
        _menuPosition = Panel.PanelRect.anchoredPosition;
        _menuHiddenPosition = new float2(-Panel.PanelRect.anchoredPosition.x, Panel.PanelRect.anchoredPosition.y);

        Panel.PanelRect.anchoredPosition = _menuHiddenPosition;
        Canvas.gameObject.SetActive(false);

        _entryPrefabRect = _entryPrefab.GetComponent<RectTransform>();
    }

    public AbilityMenuEntry Dequeue()
    {
        var poolable = PoolController.Dequeue(_index);
        var entry = poolable.GetComponent<AbilityMenuEntry>();
        entry.transform.SetParent(Panel.transform);
        entry.Reset();
        return entry;
    }

    public void Enqueue(AbilityMenuEntry obj) => PoolController.Enqueue(obj.GetComponent<Poolable>());

    private void ClearEntries()
    {
        for (int i = 0; i < _menuEntries.Count; i++)
            Enqueue(_menuEntries[i]);
        _menuEntries.Clear();
    }

    private IEnumerator MovePanel(Vector2 targetPosition)
    {
        var position = Panel.PanelRect.anchoredPosition;
        var time = 0.5f;
        var threshold = 0.05f;
        var elapsed = 0f;

        while (Vector3.Distance(Panel.PanelRect.anchoredPosition, targetPosition) >= threshold)
        {
            elapsed += Time.deltaTime;
            Panel.PanelRect.anchoredPosition = Vector2.Lerp(position, targetPosition, elapsed / time);
            yield return new WaitForEndOfFrame();
        }

        Panel.PanelRect.anchoredPosition = targetPosition;
        yield return null;
    }

    public bool SetSelection(AbilityMenuEntry value)
    {
        if (value.IsLocked) return false;

        if (CurrentSelection >= 0 && CurrentSelection < _menuEntries.Count)
            _menuEntries[CurrentSelection].IsSelected = false;

        CurrentSelection = _menuEntries.IndexOf(value);

        if (CurrentSelection >= 0 && CurrentSelection < _menuEntries.Count)
            _menuEntries[CurrentSelection].IsSelected = true;

        return true;
    }

    public void NextEntry()
    {
        for (int i = CurrentSelection + 1; i < _menuEntries.Count; i++)
            if (SetSelection(_menuEntries[i])) break;
    }

    public void PreviousEntry()
    {
        for (int i = CurrentSelection - 1; i >= 0; i--)
            if (SetSelection(_menuEntries[i])) break;
    }

    public IEnumerator Show(string title, Dictionary<string, Action> options)
    {
        ClearEntries();
        Title.text = title;

        foreach (var option in options)
        {
            var entry = Dequeue();
            var rect = entry.GetComponent<RectTransform>();

            entry.Title = option.Key;
            entry.Action = option.Value;
            entry.Controller = this;
            entry.transform.SetParent(Panel.transform);
            entry.gameObject.SetActive(true);

            _menuEntries.Add(entry);

            rect = rect.CopyComponent(_entryPrefabRect);
            rect.transform.localRotation = Quaternion.identity;
            rect.transform.localScale = Vector3.one;
            rect.anchoredPosition += (Vector2.down * rect.rect.height * _menuEntries.IndexOf(entry));
        }

        SetSelection(_menuEntries[0]);
        Panel.PanelRect.rect.Set(Panel.PanelRect.rect.x, Panel.PanelRect.rect.y, Panel.PanelRect.rect.width, 100 + options.Count * 60);
        Panel.PanelRect.anchoredPosition = new Vector2(Panel.PanelRect.anchoredPosition.x, -Panel.PanelRect.rect.height / 2);
        yield return null;

        Canvas.gameObject.SetActive(true);
        yield return StartCoroutine(MovePanel(_menuPosition));
    }

    public IEnumerator Hide()
    {
        yield return StartCoroutine(MovePanel(_menuPosition));
        Canvas.gameObject.SetActive(false);
        yield return null;
    }

    public void SetLocked(int index, bool value)
    {
        if (index < 0 || index >= _menuEntries.Count) return;
        _menuEntries[index].IsLocked = value;
        if (value && CurrentSelection == index) NextEntry();
    }
}
