using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

public class EndFacingState : MonoBehaviour, IBattleState
{
    private Directions _startDirection;

    public BattleController Controller { get; private set; }

    public AbilityMenuController AbilityMenuController => Controller.AbilityMenuController;
    public StatPanelController StatPanelController => Controller.StatPanelController;
    public HitSuccessIndicator HitSuccessIndicator => Controller.HitSuccessIndicator;

    public TurnData Turn => Controller.Turn;
    public List<Unit> Units => Controller.Units;

    private void Awake() => Controller = GetComponent<BattleController>();

    public void Enter()
    {
        AddListeners();
        _startDirection = Turn.Actor.Direction;
        this.SelectTile(Turn.Actor.CurrentTile.Coordinates);
    }

    public void Exit() => RemoveListeners();
    public void OnDestroy() => RemoveListeners();
    public void AddListeners() => this.ToggleListeners(true);
    public void RemoveListeners() => this.ToggleListeners(false);

    public void OnMove(object sender, DataEventArgs<float2> e)
    {
        Turn.Actor.Direction = e.Data.GetDirection();
        Turn.Actor.Refresh();
    }

    public void OnFire(object sender, DataEventArgs<int> e)
    {
        switch (e.Data)
        {
            case 1:
                Controller.StateMachine.ChangeState<SelectUnitState_PROTOTYPE>();
                break;
            case 2:
                Turn.Actor.Direction = _startDirection;
                Turn.Actor.Refresh();
                Controller.StateMachine.ChangeState<CommandSelectionState>();
                break;
        }
    }
}
