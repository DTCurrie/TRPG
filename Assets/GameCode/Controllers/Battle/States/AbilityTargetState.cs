using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AbilityTargetState : MonoBehaviour, IBattleState
{
    private List<Tile> _tiles;
    private IAbilityRange _abilityRange;

    public BattleController Controller { get; private set; }

    public AbilityMenuController AbilityMenuController => Controller.AbilityMenuController;
    public StatPanelController StatPanelController => Controller.StatPanelController;
    public HitSuccessIndicator HitSuccessIndicator => Controller.HitSuccessIndicator;

    public TurnData Turn => Controller.Turn;
    public List<Unit> Units => Controller.Units;

    private void Awake() => Controller = GetComponent<BattleController>();

    private void SelectTiles()
    {
        _tiles = _abilityRange.GetTilesInRange(Controller.Board);
        Controller.Board.ToggleTileSelection(_tiles, true);
    }

    private void ChangeDirection(float2 coordinates)
    {
        var direction = coordinates.GetDirection();
        if (Turn.Actor.Direction != direction)
        {
            Controller.Board.ToggleTileSelection(_tiles, false);
            Turn.Actor.Direction = direction;
            Turn.Actor.Refresh();
            SelectTiles();
        }
    }

    public void Enter()
    {
        AddListeners();
        _abilityRange = Turn.Ability.GetComponent<IAbilityRange>();
        SelectTiles();
        StatPanelController.ShowPrimary(Turn.Actor.gameObject);
    }

    public void Exit()
    {
        RemoveListeners();
        Controller.Board.ToggleTileSelection(_tiles, false);
    }

    public void OnDestroy() => RemoveListeners();

    public void AddListeners() => this.ToggleListeners(true);
    public void RemoveListeners() => this.ToggleListeners(false);

    public void OnMove(object sender, DataEventArgs<float2> e)
    {
        if (_abilityRange.DirectionOriented) ChangeDirection(e.Data);
    }

    public void OnFire(object sender, DataEventArgs<int> e)
    {
        if (e.Data == 1)
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                var tile = hit.transform.GetComponent<Tile>();

                if (!tile) return;

                if (tile == Controller.CurrentTile)
                    Controller.StateMachine.ChangeState<ConfirmAbilityTargetState>();

                if (tile.Content != null && (_abilityRange.DirectionOriented || _tiles.Contains(tile)))
                {
                    this.SelectTile(tile.Coordinates);
                    this.RefreshSecondaryStatPanel(tile.Coordinates);
                }
            }

        }
        else if (e.Data == 2)
        {
            StatPanelController.HideSecondary();
            Controller.StateMachine.ChangeState<CategorySelectionState>();
        }
    }
}
