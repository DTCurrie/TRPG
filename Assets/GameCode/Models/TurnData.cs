using System.Collections.Generic;
using UnityEngine;

public class TurnData
{
    private Tile _startTile;
    private Directions _startDirection;

    public Unit Actor;

    public bool Moved;
    public bool Acted;
    public bool MoveLocked;

    public GameObject Ability;
    public List<Tile> Targets;

    public void ChangeActor(Unit actor)
    {
        Actor = actor;
        Moved = false;
        Acted = false;
        MoveLocked = false;
        _startTile = actor.CurrentTile;
        _startDirection = actor.Direction;
    }

    public void UndoMove()
    {
        Moved = false;
        Actor.Place(_startTile);
        Actor.Direction = _startDirection;
        Actor.Refresh();
    }
}
