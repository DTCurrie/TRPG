using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

public class ConfirmAbilityTargetState : MonoBehaviour, IBattleState
{
    private int _index;

    private List<Tile> _tiles;
    private IAbilityArea _area;
    private IAbilityTarget[] _targeters;

    public BattleController Controller { get; private set; }

    public AbilityMenuController AbilityMenuController => Controller.AbilityMenuController;
    public StatPanelController StatPanelController => Controller.StatPanelController;
    public HitSuccessIndicator HitSuccessIndicator => Controller.HitSuccessIndicator;

    public TurnData Turn => Controller.Turn;
    public List<Unit> Units => Controller.Units;

    private void Awake() => Controller = GetComponent<BattleController>();

    private void FindTargets()
    {
        Turn.Targets = new List<Tile>();
        _targeters = Turn.Ability.GetComponentsInChildren<IAbilityTarget>();
        for (var i = 0; i < _tiles.Count; ++i)
            if (IsTarget(_tiles[i], _targeters))
                Turn.Targets.Add(_tiles[i]);
    }

    private void SetTarget(int target)
    {
        _index = target;
        if (_index < 0) _index = Turn.Targets.Count - 1;
        if (_index >= Turn.Targets.Count) _index = 0;
        if (Turn.Targets.Count > 0)
        {
            this.RefreshSecondaryStatPanel(Turn.Targets[_index].Coordinates);
            UpdateHitSuccessIndicator();
        }
    }

    private bool IsTarget(Tile tile, IAbilityTarget[] list)
    {
        for (int i = 0; i < list.Length; ++i)
            if (list[i].IsTarget(tile))
                return true;

        return false;
    }

    private void UpdateHitSuccessIndicator()
    {
        var chance = CalculateHitRate();
        var amount = EstimateDamage();
        var target = Turn.Targets[_index];

        for (var i = 0; i < _targeters.Length; i++)
        {
            if (_targeters[i].IsTarget(target))
            {
                var hitRate = ((Component)_targeters[i]).GetComponent<IHitRate>();
                chance = hitRate.Calculate(target);

                var effect = ((Component)_targeters[i]).GetComponent<IAbilityEffect>();
                amount = effect.Predict(target);
                break;
            }
        }

        HitSuccessIndicator.SetStats(chance, amount);
    }

    private int CalculateHitRate()
    {
        var target = Turn.Targets[_index];
        var hitRate = Turn.Ability.GetComponentInChildren<IHitRate>();
        return hitRate.Calculate(target);
    }

    private int EstimateDamage() => 50;

    public void Enter()
    {
        AddListeners();
        _area = Turn.Ability.GetComponent<IAbilityArea>();
        _tiles = _area.GetTilesInArea(Controller.Board, Controller.CurrentCoordinates);
        Controller.Board.ToggleTileSelection(_tiles, true);
        FindTargets();
        this.RefreshPrimaryStatPanel(Turn.Actor.CurrentTile.Coordinates);

        if (Turn.Targets.Count > 0)
        {
            StartCoroutine(HitSuccessIndicator.Show());
            SetTarget(0);
        }
    }

    public void Exit()
    {
        RemoveListeners();
        Controller.Board.ToggleTileSelection(_tiles, false);
        StartCoroutine(HitSuccessIndicator.Hide());
    }

    public void OnDestroy() => RemoveListeners();
    public void AddListeners() => this.ToggleListeners(true);
    public void RemoveListeners() => this.ToggleListeners(false);

    public void OnMove(object sender, DataEventArgs<float2> e)
    {
        if (e.Data.y > 0 || e.Data.x > 0)
            SetTarget(_index + 1);
        else
            SetTarget(_index - 1);
    }

    public void OnFire(object sender, DataEventArgs<int> e)
    {
        if (e.Data == 1)
        {
            if (Turn.Targets.Count > 0)
            {
                StatPanelController.HidePrimary();
                StatPanelController.HideSecondary();
                Controller.StateMachine.ChangeState<PerformAbilityState>();
            }

        }
        else if (e.Data == 2)
        {
            StatPanelController.HideSecondary();
            Controller.StateMachine.ChangeState<AbilityTargetState>();
        }

    }
}
