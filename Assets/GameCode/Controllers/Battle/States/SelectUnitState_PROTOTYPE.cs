using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class SelectUnitState_PROTOTYPE : MonoBehaviour, IBattleState
{
    private int _index = -1;

    public BattleController Controller { get; private set; }

    public AbilityMenuController AbilityMenuController => Controller.AbilityMenuController;
    public StatPanelController StatPanelController => Controller.StatPanelController;
    public HitSuccessIndicator HitSuccessIndicator => Controller.HitSuccessIndicator;

    public TurnData Turn => Controller.Turn;
    public List<Unit> Units => Controller.Units;

    private void Awake() => Controller = GetComponent<BattleController>();

    private IEnumerator ChangeCurrentUnit()
    {
        Controller.Round.MoveNext();
        this.SelectTile(Turn.Actor.CurrentTile.Coordinates);
        this.RefreshPrimaryStatPanel(Controller.CurrentCoordinates);
        yield return null;
        Controller.StateMachine.ChangeState<CommandSelectionState>();
    }

    public void Enter()
    {
        AddListeners();
        StartCoroutine(ChangeCurrentUnit());
    }

    public void Exit()
    {
        RemoveListeners();
        StatPanelController.HidePrimary();
    }

    public void OnDestroy() => RemoveListeners();
    public void AddListeners() => this.ToggleListeners(true);
    public void RemoveListeners() => this.ToggleListeners(false);
    public void OnMove(object sender, DataEventArgs<float2> e) { }
    public void OnFire(object sender, DataEventArgs<int> e) { }
}