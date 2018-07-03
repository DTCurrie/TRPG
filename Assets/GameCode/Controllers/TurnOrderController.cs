using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnOrderController : MonoBehaviour
{
    private const int _turnActivation = 1000;
    private const int _turnCost = 500;
    private const int _moveCost = 300;
    private const int _actionCost = 200;

    public const string BeforeRoundMessage = "TurnOrderController.BeforeRound";
    public const string OnTurnCheckedMessage = "TurnOrderController.OnTurnChecked";
    public const string OnTurnCompletedMessage = "TurnOrderController.OnTurnCompleted";
    public const string OnRoundCompletedMessage = "TurnOrderController.OnRoundCompleted";

    private bool CanTakeTurn(Unit unit)
    {
        var flag = new FlagEffect(GetCounter(unit) >= _turnActivation);
        unit.PostMessage(OnTurnCheckedMessage, flag);
        return flag.Active;
    }

    private int GetCounter(Unit unit) => unit.GetComponent<Stats>()[StatTypes.CTR];

    public IEnumerator Round()
    {
        var battleController = GetComponent<BattleController>();
        while (true)
        {
            this.PostMessage(BeforeRoundMessage);

            var units = new List<Unit>(battleController.Units);

            for (var i = 0; i < units.Count; i++)
            {
                var stats = units[i].GetComponent<Stats>();
                stats[StatTypes.CTR] += stats[StatTypes.SPD];
            }

            units.Sort((a, b) => GetCounter(a).CompareTo(GetCounter(b)));

            for (var i = units.Count - 1; i >= 0; i--)
            {
                if (CanTakeTurn(units[i]))
                {
                    battleController.Turn.ChangeActor(units[i]);
                    yield return units[i];

                    var cost = _turnCost;

                    if (battleController.Turn.Moved) cost += _moveCost;
                    if (battleController.Turn.Acted) cost += _actionCost;

                    var stats = units[i].GetComponent<Stats>();
                    stats.SetValue(StatTypes.CTR, stats[StatTypes.CTR] - cost, false);

                    units[i].PostMessage(OnTurnCheckedMessage);
                }
            }

            this.PostMessage(OnRoundCompletedMessage);
        }
    }
}
