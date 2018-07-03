using System.Collections.Generic;

public interface IAbilityRange
{
    bool DirectionOriented { get; }
    Unit Unit { get; }

    List<Tile> GetTilesInRange(Board board);
}
