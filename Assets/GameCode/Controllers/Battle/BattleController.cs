using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public StateMachine StateMachine;
    public CameraRig CameraRig;
    public Board Board;
    public LevelData LevelData;
    public Transform TileSelector;
    public float2 CurrentCoordinates;
    public IEnumerator Round;

    public AbilityMenuController AbilityMenuController;
    public StatPanelController StatPanelController;

    public TurnData Turn = new TurnData();
    public List<Unit> Units = new List<Unit>();

    // PROTOTYPE
    public GameObject HeroPrefab;
    public Tile CurrentTile { get => Board.GetTile(CurrentCoordinates); }

    private void Start()
    {
        StateMachine.ChangeState<InitBattleState>();
    }
}
