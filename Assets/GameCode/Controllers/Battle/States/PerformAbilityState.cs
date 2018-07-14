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
    public HitSuccessIndicator HitSuccessIndicator => Controller.HitSuccessIndicator;

    public TurnData Turn => Controller.Turn;
    public List<Unit> Units => Controller.Units;

    private void Awake() => Controller = GetComponent<BattleController>();

    private IEnumerator Animate()
    {
        ApplyAbility();
        yield return null;
        StatPanelController.ShowPrimary(Turn.Actor.gameObject);
        if (Turn.Moved)
            Controller.StateMachine.ChangeState<EndFacingState>();
        else
            Controller.StateMachine.ChangeState<CommandSelectionState>();
    }

    private void ApplyAbility()
    {
        var effects = Turn.Ability.GetComponentsInChildren<IAbilityEffect>();

        for (var i = 0; i < Turn.Targets.Count; i++)
        {
            var target = Turn.Targets[i];

            for (int j = 0; j < effects.Length; j++)
            {
                var effect = effects[j];
                var targeter = ((Component)effect).GetComponent<IAbilityTarget>();
                if (targeter.IsTarget(target))
                {
                    var hitRate = ((Component)effect).GetComponent<IHitRate>();
                    var chance = hitRate.Calculate(target);

                    if (UnityEngine.Random.Range(0, 101) > chance) continue;
                    effect.Apply(target);
                }
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
