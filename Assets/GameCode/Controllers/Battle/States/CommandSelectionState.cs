using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;
using System;

public class CommandSelectionState : MonoBehaviour, IAbilityMenuState
{
    private Dictionary<string, Action> _menuOptions;

    public string Title => "Commands";
    public Dictionary<string, Action> MenuOptions => _menuOptions;

    public BattleController Controller { get; private set; }

    public AbilityMenuController AbilityMenuController => Controller.AbilityMenuController;
    public StatPanelController StatPanelController => Controller.StatPanelController;
    public HitSuccessIndicator HitSuccessIndicator => Controller.HitSuccessIndicator;

    public TurnData Turn => Controller.Turn;
    public List<Unit> Units => Controller.Units;

    private void Awake() => Controller = GetComponent<BattleController>();

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
            {"Move", Controller.StateMachine.ChangeState<MoveTargetState>},
            {"Action", Controller.StateMachine.ChangeState<CategorySelectionState>},
            {"Wait", Controller.StateMachine.ChangeState<EndFacingState>}
        };

        StartCoroutine(AbilityMenuController.Show(Title, _menuOptions));
        AbilityMenuController.SetLocked(0, Turn.Moved);
        AbilityMenuController.SetLocked(1, Turn.Acted);
    }

    public void Cancel()
    {
        if (Turn.Moved && !Turn.MoveLocked)
        {
            Turn.UndoMove();
            AbilityMenuController.SetLocked(0, false);
            this.SelectTile(Turn.Actor.CurrentTile.Coordinates);
        }
        else
        {
            Controller.StateMachine.ChangeState<ExploreState>();
        }
    }
}