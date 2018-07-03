using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{
    public Tile CurrentTile { get; protected set; }
    public Directions Direction;

    public void Place(Tile target)
    {
        if (CurrentTile != null && CurrentTile.Content == gameObject)
            CurrentTile.Content = null;

        CurrentTile = target;

        if (target != null) target.Content = gameObject;
    }

    public void Refresh()
    {
        transform.localPosition = CurrentTile.CenterTop;
        transform.localEulerAngles = Direction.ToEuler();
    }
}
