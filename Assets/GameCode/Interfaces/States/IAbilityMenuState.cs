using System;
using System.Collections.Generic;

public interface IAbilityMenuState : IBattleState
{
    string Title { get; }
    Dictionary<string, Action> MenuOptions { get; }

    void LoadMenu();
    void Cancel();
}
