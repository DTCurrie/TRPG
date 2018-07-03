using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Board : MonoBehaviour
{
    private float2[] _directions = {
        new float2(0,1),
        new float2(0,-1),
        new float2(1,0),
        new float2(-1,0)
    };

    private float2 _min;
    private float2 _max;

    public float2 Min => _min;
    public float2 Max => _max;

    public GameObject TilePrefab;
    public Dictionary<float2, Tile> Tiles = new Dictionary<float2, Tile>();

    private void ResetPathfinding()
    {
        foreach (var tile in Tiles.Values) tile.ResetPathfindingData();
    }

    public void Load(LevelData levelData)
    {
        _min = new float2(int.MaxValue, int.MaxValue);
        _max = new float2(int.MinValue, int.MinValue);

        for (var i = 0; i < levelData.TileData.Count; i++)
        {
            var tile = Instantiate(TilePrefab).GetComponent<Tile>();
            tile.transform.parent = transform;
            tile.Place(levelData.TileData[i]);
            Tiles.Add(tile.Coordinates, tile);

            _min.x = math.min(_min.x, tile.Coordinates.x);
            _min.y = math.min(_min.y, tile.Coordinates.y);

            _max.x = math.min(_max.x, tile.Coordinates.x);
            _max.y = math.min(_max.y, tile.Coordinates.y);
        }
    }

    public Tile GetTile(float2 coordinates) => Tiles.ContainsKey(coordinates) ? Tiles[coordinates] : null;

    public List<Tile> RangeSearch(Tile start, Func<Tile, Tile, bool> checkCost)
    {
        var visited = new List<Tile> { start };
        var frontier = new Queue<Tile>();

        ResetPathfinding();
        start.PathfindingCost = 0;
        frontier.Enqueue(start);

        while (frontier.Count > 0)
        {
            // Get current tile in queue
            var tile = frontier.Dequeue();

            for (int i = 0; i < _directions.Length; i++)
            {
                // Get neighbor
                var neighbor = GetTile(tile.Coordinates + _directions[i]);

                // Is neighbot not null and is it cost less than or equal to the current tile
                if (neighbor == null || neighbor.PathfindingCost <= tile.PathfindingCost + 1)
                    continue;

                // Check the tile cost with passed function from movement component
                if (checkCost(tile, neighbor))
                {
                    // Set neighbor pathfinding data and add it to frontier queue
                    neighbor.PathfindingCost = tile.PathfindingCost + 1;
                    neighbor.PathFindingPrevious = tile;

                    // If we haven't visited the tile, queue it to search for 
                    // neighbors and add it to visited
                    if (!visited.Contains(neighbor))
                    {
                        frontier.Enqueue(neighbor);
                        visited.Add(neighbor);
                    }
                }
            }

        }

        return visited;
    }

    public void ToggleTileSelection(List<Tile> tiles, bool selecting)
    {
        for (var i = 0; i < tiles.Count; i++) tiles[i].Select(selecting);
    }
}
