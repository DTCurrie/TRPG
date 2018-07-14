using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ActionSelectionState : MonoBehaviour, IAbilityMenuState
{
    private Dictionary<string, Action> _menuOptions = new Dictionary<string, Action>();

    private string[] _whiteMagicOptions = { "Cure", "Raise", "Holy" };
    private string[] _blackMagicOptions = { "Fire", "Ice", "Lightning" };

    public static int Category;

    public string Title => Category == 0 ? "White Magic" : "Black Magic";
    public Dictionary<string, Action> MenuOptions => _menuOptions;

    public BattleController Controller { get; private set; }

    public AbilityMenuController AbilityMenuController => Controller.AbilityMenuController;
    public StatPanelController StatPanelController => Controller.StatPanelController;
    public HitSuccessIndicator HitSuccessIndicator => Controller.HitSuccessIndicator;

    public TurnData Turn => Controller.Turn;
    public List<Unit> Units => Controller.Units;

    private void Awake() => Controller = GetComponent<BattleController>();

    private void SetOptions(string[] options)
    {
        _menuOptions.Clear();
        for (int i = 0; i < options.Length; i++) MenuOptions.Add(options[i], Confirm);
    }

    public void Enter()
    {
        this.AbilityMenuStateEnter();
        StatPanelController.ShowPrimary(Turn.Actor.gameObject);
    }

    public void Exit()
    {
        this.AbilityMenuStateExit();
        StatPanelController.HidePrimary();
    }

    public void OnDestroy() => RemoveListeners();
    public void AddListeners() => this.ToggleListeners(true);
    public void RemoveListeners() => this.ToggleListeners(false);

    public void OnMove(object sender, DataEventArgs<float2> e) => this.AbilityMenuStateOnMove(sender, e);

    public void OnFire(object sender, DataEventArgs<int> e) => this.AbilityMenuStateOnFire(sender, e);

    public void LoadMenu()
    {
        SetOptions(Category == 0 ? _whiteMagicOptions : _blackMagicOptions);
        StartCoroutine(AbilityMenuController.Show(Title, MenuOptions));
    }

    public void Confirm()
    {
        Turn.Acted = true;
        if (Turn.Moved) Turn.MoveLocked = true;
        Controller.StateMachine.ChangeState<CommandSelectionState>();
    }

    public void Cancel() => Controller.StateMachine.ChangeState<CommandSelectionState>();
}