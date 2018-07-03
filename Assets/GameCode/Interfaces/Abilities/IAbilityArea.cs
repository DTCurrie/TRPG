using Unity.Mathematics;
using System.Collections.Generic;

public interface IAbilityArea
{
    List<Tile> GetTilesInArea(Board board, float2 coordinates);
}
