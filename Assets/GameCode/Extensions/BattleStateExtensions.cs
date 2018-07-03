using Unity.Mathematics;
using UnityEngine;

public static class BattleStateExtensions
{
    public static void ToggleListeners(this IBattleState state, bool entering)
    {
        if (entering)
        {
            InputController.MoveEvent += state.OnMove;
            InputController.FireEvent += state.OnFire;
        }
        else
        {
            InputController.MoveEvent -= state.OnMove;
            InputController.FireEvent -= state.OnFire;
        }
    }

    public static void SelectTile(this IBattleState state, float2 coordinates)
    {
        if (state.Controller.CurrentCoordinates.Equals(coordinates) ||
            !state.Controller.Board.Tiles.ContainsKey(coordinates))
            return;

        state.Controller.CurrentCoordinates = coordinates;
        state.Controller.TileSelector.localPosition = state.Controller.Board.Tiles[coordinates].CenterTop;
    }

    public static Unit GetUnit(this IBattleState state, float2 coordinates) =>
        state.Controller.Board.GetTile(coordinates)?.Content?.GetComponent<Unit>();

    public static void RefreshPrimaryStatPanel(this IBattleState state, float2 coordinates)
    {
        var target = state.GetUnit(coordinates);
        if (target != null)
            state.StatPanelController.ShowPrimary(target.gameObject);
        else
            state.StatPanelController.HidePrimary();
    }

    public static void RefreshSecondaryStatPanel(this IBattleState state, float2 coordinates)
    {
        var target = state.GetUnit(coordinates);
        if (target != null)
            state.StatPanelController.ShowSecondary(target.gameObject);
        else
            state.StatPanelController.HideSecondary();
    }

    public static void AbilityMenuStateEnter(this IAbilityMenuState state)
    {
        state.AddListeners();
        state.SelectTile(state.Turn.Actor.CurrentTile.Coordinates);
        state.LoadMenu();
    }

    public static void AbilityMenuStateExit(this IAbilityMenuState state)
    {
        state.RemoveListeners();
        state.AbilityMenuController.Hide();
    }

    public static void AbilityMenuStateOnFire(this IAbilityMenuState state, object sender, DataEventArgs<int> e)
    {
        if (e.Data == 2) state.Cancel();
    }

    public static void AbilityMenuStateOnMove(this IAbilityMenuState state, object sender, DataEventArgs<float2> e)
    {
        if (e.Data.x > 0 || e.Data.y < 0)
            state.AbilityMenuController.NextEntry();
        else
            state.AbilityMenuController.PreviousEntry();
    }
}
