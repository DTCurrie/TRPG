using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class AbilityMenuEntry : MonoBehaviour, IPointerClickHandler
{
    private Outline _outline;
    private Color _outlineColor = new Color(20, 36, 44, 255);
    private MenuStates _currentState;

    [Flags]
    private enum MenuStates
    {
        None = 0,
        Selected = 1 << 0,
        Locked = 1 << 10
    }

    public Image Bullet;
    public Sprite Normal;
    public Sprite Selected;
    public Sprite Disabled;
    public TextMeshProUGUI Label;
    public Action Action;
    public AbilityMenuController Controller;

    private MenuStates CurrentState
    {
        get => _currentState;
        set
        {
            if (_currentState == value) return;
            _currentState = value;

            if (IsLocked)
            {
                Bullet.sprite = Disabled;
                Label.color = Color.gray;
                _outline.effectColor = _outlineColor;
            }
            else if (IsSelected)
            {
                Bullet.sprite = Selected;
                Label.color = new Color(249, 210, 118, 255);
                _outline.effectColor = new Color(255, 160, 72, 255);
            }
            else
            {
                Bullet.sprite = Normal;
                Label.color = Color.black;
                _outline.effectColor = _outlineColor;
            }
        }
    }

    public string Title { get => Label.text; set => Label.text = value; }


    public bool IsLocked
    {
        get { return (CurrentState & MenuStates.Locked) != MenuStates.None; }
        set
        {
            if (value)
                CurrentState |= MenuStates.Locked;
            else
                CurrentState &= ~MenuStates.Locked;
        }
    }

    public bool IsSelected
    {
        get { return (CurrentState & MenuStates.Selected) != MenuStates.None; }
        set
        {
            if (value)
                CurrentState |= MenuStates.Selected;
            else
                CurrentState &= ~MenuStates.Selected;
        }
    }

    private void Awake()
    {
        _outline = Label.GetComponent<Outline>();
    }

    public void Reset() => CurrentState = MenuStates.None;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (CurrentState == MenuStates.Selected)
                Action.Invoke();
            else
                Controller.SetSelection(this);
        }
    }
}
