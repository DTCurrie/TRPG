using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CategorySelectionState : MonoBehaviour, IAbilityMenuState
{
    private Dictionary<string, Action> _menuOptions;

    public string Title => "Action";
    public Dictionary<string, Action> MenuOptions => _menuOptions;

    public BattleController Controller { get; private set; }

    public AbilityMenuController AbilityMenuController => Controller.AbilityMenuController;
    public StatPanelController StatPanelController => Controller.StatPanelController;
    public HitSuccessIndicator HitSuccessIndicator => Controller.HitSuccessIndicator;

    public TurnData Turn => Controller.Turn;
    public List<Unit> Units => Controller.Units;

    private void Awake() => Controller = GetComponent<BattleController>();

    private void Attack()
    {
        Turn.Ability = Turn.Actor.transform.Find("Attack").gameObject;
        Controller.StateMachine.ChangeState<AbilityTargetState>();
    }

    private void PoisonDart()
    {
        Turn.Ability = Turn.Actor.transform.Find("Poison Dart").gameObject;
        Controller.StateMachine.ChangeState<AbilityTargetState>();
    }

    public void Enter()
    {
        this.AbilityMenuStateEnter();
        StatPanelController.ShowPrimary(Turn.Actor.gameObject);
    }

    public void Exit() => this.AbilityMenuStateExit();
    public void OnDestroy() => RemoveListeners();
    public void AddListeners() => this.ToggleListeners(true);
    public void RemoveListeners() => this.ToggleListeners(false);
    public void OnMove(object sender, DataEventArgs<float2> e) => this.AbilityMenuStateOnMove(sender, e);
    public void OnFire(object sender, DataEventArgs<int> e) => this.AbilityMenuStateOnFire(sender, e);

    public void LoadMenu()
    {
        _menuOptions = new Dictionary<string, Action>
        {
            {"Attack", Attack},
            {"Poison Dart", PoisonDart},
            {
                "White Magic",
                () => {
                    ActionSelectionState.Category = 0;
                    Controller.StateMachine.ChangeState<ActionSelectionState>();
                }
            },
            {
                "Black Magic",
                () => {
                    ActionSelectionState.Category = 1;
                    Controller.StateMachine.ChangeState<ActionSelectionState>();
                }
            }
        };
        StartCoroutine(AbilityMenuController.Show(Title, _menuOptions));
    }

    public void Cancel() => Controller.StateMachine.ChangeState<CommandSelectionState>();
}