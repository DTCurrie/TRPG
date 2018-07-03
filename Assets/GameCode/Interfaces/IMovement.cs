using System.Collections;

public interface IMovement
{
    Unit Unit { get; }
    int Range { get; }

    bool ExpandSearch(Tile from, Tile to);
    IEnumerator Move(Tile target);
}