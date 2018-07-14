using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

public interface IBattleState : IState
{
    BattleController Controller { get; }

    AbilityMenuController AbilityMenuController { get; }
    StatPanelController StatPanelController { get; }
    HitSuccessIndicator HitSuccessIndicator { get; }

    TurnData Turn { get; }
    List<Unit> Units { get; }

    void OnMove(object sender, DataEventArgs<float2> e);
    void OnFire(object sender, DataEventArgs<int> e);
}
