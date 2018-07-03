using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class InitBattleState : MonoBehaviour, IBattleState
{
    public BattleController Controller { get; private set; }

    public AbilityMenuController AbilityMenuController => Controller.AbilityMenuController;
    public StatPanelController StatPanelController => Controller.StatPanelController;

    public TurnData Turn => Controller.Turn;
    public List<Unit> Units => Controller.Units;

    private IEnumerator Init()
    {
        var levelData = Controller.LevelData;
        Controller.Board.Load(levelData);
        var coords = new float2((int)levelData.TileData[0].x, (int)levelData.TileData[0].z);
        this.SelectTile(coords);
        SpawnTestUnits();
        Controller.Round = Controller.gameObject.AddComponent<TurnOrderController>().Round();
        yield return null;
        Controller.StateMachine.ChangeState<CutSceneState>();
    }

    // PROTOTYPE
    private void SpawnTestUnits()
    {
        var jobs = new string[] { "Rogue", "Warrior", "Wizard" };

        for (int i = 0; i < jobs.Length; i++)
        {
            var instance = Instantiate(Controller.HeroPrefab);
            var stats = instance.AddComponent<Stats>();

            stats[StatTypes.LVL] = 1;

            var jobPrefab = Resources.Load<GameObject>($"Jobs/{jobs[i]}");
            var jobInstance = Instantiate(jobPrefab);
            var job = jobInstance.GetComponent<Job>();

            jobInstance.transform.SetParent(instance.transform);

            job.Employ();
            job.LoadDefaultStats();

            var attackInstance = new GameObject("Attack");

            switch (i)
            {
                case 0:
                    attackInstance.AddComponent<ConstantAbilityRange>().Horizontal = 3;
                    attackInstance.AddComponent<UnitAbilityArea>();
                    attackInstance.AddComponent<DefaultAbilityTarget>();
                    break;
                case 1:
                    attackInstance.AddComponent<ConeAbilityRange>().Horizontal = 3;
                    attackInstance.AddComponent<FillAreaAbility>();
                    attackInstance.AddComponent<DefaultAbilityTarget>();
                    break;
                case 2:
                    attackInstance.AddComponent<LineAbilityRange>();
                    attackInstance.AddComponent<SpecifiedAbilityArea>();
                    attackInstance.AddComponent<DefaultAbilityTarget>();
                    break;
            }

            attackInstance.transform.SetParent(instance.transform);

            var rank = instance.AddComponent<Rank>();
            rank.Init(10);

            var tile = Controller.LevelData.TileData[i];
            var coordinates = new float2((int)tile.x, (int)tile.z);
            var unit = instance.GetComponent<Unit>();

            unit.Place(Controller.Board.GetTile(coordinates));
            unit.Refresh();

            instance.AddComponent<WalkMovement>();
            Units.Add(unit);
        }
    }

    private void Awake() => Controller = GetComponent<BattleController>();

    public void Enter()
    {
        AddListeners();
        StartCoroutine(Init());
    }

    public void Exit() => RemoveListeners();

    public void OnDestroy() => RemoveListeners();

    public void AddListeners() => this.ToggleListeners(true);

    public void RemoveListeners() => this.ToggleListeners(false);

    public void OnMove(object sender, DataEventArgs<float2> e) { }

    public void OnFire(object sender, DataEventArgs<int> e) { }
}
