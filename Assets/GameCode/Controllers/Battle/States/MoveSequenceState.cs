using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

public class MoveSequenceState : MonoBehaviour, IBattleState
{
    public BattleController Controller { get; private set; }

    public AbilityMenuController AbilityMenuController => Controller.AbilityMenuController;
    public StatPanelController StatPanelController => Controller.StatPanelController;
    public HitSuccessIndicator HitSuccessIndicator => Controller.HitSuccessIndicator;

    public TurnData Turn => Controller.Turn;
    public List<Unit> Units => Controller.Units;

    private IEnumerator Sequence()
    {
        Turn.Moved = true;
        var movement = Controller.Turn.Actor.GetComponent<IMovement>();
        yield return StartCoroutine(movement.Move(Controller.CurrentTile));
        Controller.StateMachine.ChangeState<CommandSelectionState>();
    }


    private void Awake() => Controller = GetComponent<BattleController>();

    public void Enter()
    {
        AddListeners();
        StartCoroutine(Sequence());
    }

    public void Exit() => RemoveListeners();

    public void OnDestroy() => RemoveListeners();

    public void AddListeners() => this.ToggleListeners(true);

    public void RemoveListeners() => this.ToggleListeners(false);

    public void OnMove(object sender, DataEventArgs<float2> e) { }

    public void OnFire(object sender, DataEventArgs<int> e) { }
}
