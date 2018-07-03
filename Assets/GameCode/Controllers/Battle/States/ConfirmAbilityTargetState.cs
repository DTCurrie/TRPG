using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

public class ConfirmAbilityTargetState : MonoBehaviour, IBattleState
{
    private List<Tile> _tiles;
    private IAbilityArea _area;
    private int _index;

    public BattleController Controller { get; private set; }

    public AbilityMenuController AbilityMenuController => Controller.AbilityMenuController;
    public StatPanelController StatPanelController => Controller.StatPanelController;

    public TurnData Turn => Controller.Turn;
    public List<Unit> Units => Controller.Units;

    private void Awake() => Controller = GetComponent<BattleController>();

    private void FindTargets()
    {
        Turn.Targets = new List<Tile>();
        var targeters = Turn.Ability.GetComponentsInChildren<IAbilityTarget>();
        for (var i = 0; i < _tiles.Count; ++i)
            if (IsTarget(_tiles[i], targeters))
                Turn.Targets.Add(_tiles[i]);
    }

    private void SetTarget(int target)
    {
        _index = target;
        if (_index < 0) _index = Turn.Targets.Count - 1;
        if (_index >= Turn.Targets.Count) _index = 0;
        if (Turn.Targets.Count > 0)
            this.RefreshSecondaryStatPanel(Turn.Targets[_index].Coordinates);
    }

    private bool IsTarget(Tile tile, IAbilityTarget[] list)
    {
        for (int i = 0; i < list.Length; ++i)
            if (list[i].IsTarget(tile))
                return true;

        return false;
    }

    public void Enter()
    {
        AddListeners();
        _area = Turn.Ability.GetComponent<IAbilityArea>();
        _tiles = _area.GetTilesInArea(Controller.Board, Controller.CurrentCoordinates);
        Controller.Board.ToggleTileSelection(_tiles, true);
        FindTargets();
        this.RefreshPrimaryStatPanel(Turn.Actor.CurrentTile.Coordinates);
        SetTarget(0);
    }

    public void Exit()
    {
        RemoveListeners();
        Controller.Board.ToggleTileSelection(_tiles, false);
        StatPanelController.HidePrimary();
        StatPanelController.HideSecondary();
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
        Debug.Log($"confirming ability target {e.Data}");
        if (e.Data == 1)
        {
            Debug.Log($"attacking {Turn.Targets.Count} targets");
            if (Turn.Targets.Count > 0)
                Controller.StateMachine.ChangeState<PerformAbilityState>();

        }
        else if (e.Data == 2)
        {
            Controller.StateMachine.ChangeState<AbilityTargetState>();
        }

    }
}
