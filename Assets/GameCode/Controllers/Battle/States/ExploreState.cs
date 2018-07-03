using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

public class ExploreState : MonoBehaviour, IBattleState
{
    public BattleController Controller { get; private set; }

    public AbilityMenuController AbilityMenuController => Controller.AbilityMenuController;
    public StatPanelController StatPanelController => Controller.StatPanelController;

    public TurnData Turn => Controller.Turn;
    public List<Unit> Units => Controller.Units;

    private void Awake() => Controller = GetComponent<BattleController>();
    private void Start()
    {
        Debug.Log("Explore");
    }

    public void Enter()
    {
        AddListeners();
        Controller.StatPanelController.HidePrimary();
    }

    public void Exit()
    {
        this.RefreshPrimaryStatPanel(Turn.Actor.CurrentTile.Coordinates);
        RemoveListeners();
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
                if (tile && Controller.CurrentTile != tile)
                {
                    this.SelectTile(tile.Coordinates);
                    if (tile.Content != null)
                        this.RefreshPrimaryStatPanel(Controller.CurrentCoordinates);
                    else
                        Controller.StatPanelController.HidePrimary();
                }
            }
        }
        else if (e.Data == 2)
            Controller.StateMachine.ChangeState<CommandSelectionState>();
    }
}
