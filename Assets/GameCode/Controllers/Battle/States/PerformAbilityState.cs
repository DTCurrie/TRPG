using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using System;

public class PerformAbilityState : MonoBehaviour, IBattleState
{
    public BattleController Controller { get; private set; }

    public AbilityMenuController AbilityMenuController => Controller.AbilityMenuController;
    public StatPanelController StatPanelController => Controller.StatPanelController;

    public TurnData Turn => Controller.Turn;
    public List<Unit> Units => Controller.Units;

    private void Awake() => Controller = GetComponent<BattleController>();

    private IEnumerator Animate()
    {
        yield return null;
        PrototypeAttack();
        if (Turn.Moved)
            Controller.StateMachine.ChangeState<EndFacingState>();
        else
            Controller.StateMachine.ChangeState<CommandSelectionState>();
    }

    private void PrototypeAttack()
    {
        for (var i = 0; i < Turn.Targets.Count; i++)
        {
            var obj = Turn.Targets[i].Content;
            var stats = obj?.GetComponentInChildren<Stats>();
            if (stats != null)
            {
                stats[StatTypes.HP] -= 50;
                if (stats[StatTypes.HP] <= 0) Debug.Log("KO'd Uni!", obj);
            }
        }
    }

    public void Enter()
    {
        AddListeners();
        Turn.Acted = true;
        if (Turn.Moved) Turn.MoveLocked = true;
        StartCoroutine(Animate());
    }

    public void Exit() => RemoveListeners();
    public void OnDestroy() => RemoveListeners();
    public void AddListeners() => this.ToggleListeners(true);
    public void RemoveListeners() => this.ToggleListeners(false);

    public void OnMove(object sender, DataEventArgs<float2> e) { }
    public void OnFire(object sender, DataEventArgs<int> e) { }
}
