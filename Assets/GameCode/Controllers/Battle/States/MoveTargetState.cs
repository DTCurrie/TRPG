using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MoveTargetState : MonoBehaviour, IBattleState
{
    private List<Tile> _tiles;

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
        var movement = Controller.Turn.Actor.GetComponent<IMovement>();
        _tiles = movement.GetTilesInRange(Controller.Board);
        Controller.Board.ToggleTileSelection(_tiles, true);
    }

    public void Exit()
    {
        RemoveListeners();
        Controller.Board.ToggleTileSelection(_tiles, false);
        _tiles = null;
    }

    public void OnDestroy() => RemoveListeners();

    public void AddListeners() => this.ToggleListeners(true);

    public void RemoveListeners() => this.ToggleListeners(false);

    public void OnMove(object sender, DataEventArgs<float2> e) =>
        this.SelectTile(e.Data + Controller.CurrentCoordinates);

    public void OnFire(object sender, DataEventArgs<int> e)
    {
        if (e.Data == 1)
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                var tile = hit.transform.GetComponent<Tile>();
                if (tile)
                {
                    if (Controller.CurrentTile != tile)
                        this.SelectTile(tile.Coordinates);
                    else if (_tiles.Contains(Controller.CurrentTile))
                        Controller.StateMachine.ChangeState<MoveSequenceState>();
                }
            }
        }
        else
            Controller.StateMachine.ChangeState<CommandSelectionState>();
    }
}




