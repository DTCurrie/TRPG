using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    private Transform _selector;

    public LevelData LevelData;
    public string LevelName;

    public GameObject TilePrefab;
    public GameObject TileSelectorPrefab;

    public Dictionary<float2, Tile> Tiles = new Dictionary<float2, Tile>();

    public int BoardWidth = 10;
    public int BoardLength = 10;
    public int BoardHeight = 8;

    public float2 CurrentCoordinates;

    public Transform Selector
    {
        get
        {
            if (_selector == null)
                _selector = Instantiate(TileSelectorPrefab).transform;
            return _selector;
        }
    }

    private Tile GetOrCreateTile(float2 coordinates)
    {
        if (Tiles.ContainsKey(coordinates)) return Tiles[coordinates];

        var tile = CreateTile();
        tile.Place(coordinates, 0);
        Tiles.Add(coordinates, tile);
        return tile;
    }

    private Rect RandomRect()
    {
        var x = UnityEngine.Random.Range(0, BoardWidth);
        var y = UnityEngine.Random.Range(0, BoardLength);
        var w = UnityEngine.Random.Range(1, BoardWidth - x + 1);
        var h = UnityEngine.Random.Range(1, BoardLength - y + 1);
        return new Rect(x, y, w, h);
    }

    private void AdjustSingle(float2 coordinates, int height)
    {
        if (height > 0)
        {
            var tile = GetOrCreateTile(coordinates);
            if (tile.Height < BoardHeight)
            {
                if (tile.Height + height > BoardHeight)
                    height = BoardHeight - tile.Height;
                tile.Adjust(height);
            }
        }
        else
        {
            if (!Tiles.ContainsKey(coordinates)) return;

            var tile = Tiles[coordinates];
            tile.Adjust(height);

            if (tile.Height <= 0)
            {
                Tiles.Remove(coordinates);
                DestroyImmediate(tile.gameObject);
            }

        }
    }

    public void AdjustCurrentTile(int height)
    {
        AdjustSingle(CurrentCoordinates, height);
    }

    public void AdjustRect(Rect rect, int height)
    {
        for (int y = (int)rect.yMin; y < (int)rect.yMax; y++)
            for (var x = (int)rect.xMin; x < (int)rect.xMax; x++)
                AdjustSingle(new float2(x, y), height);
    }

    public void AdjustArea(int height)
    {
        var rect = RandomRect();
        AdjustRect(rect, height);
    }

    public Tile CreateTile()
    {
        var tile = Instantiate(TilePrefab);
        tile.transform.parent = transform;
        return tile.GetComponent<Tile>();
    }

    public void ClearTiles()
    {
        for (var i = transform.childCount - 1; i >= 0; i--) 
            DestroyImmediate(transform.GetChild(i).gameObject);
        Tiles.Clear();
    }

    public void UpdateSelector()
    {
        var tile = Tiles.ContainsKey(CurrentCoordinates)
                        ? Tiles[CurrentCoordinates]
                        : null;
        Selector.localPosition = tile != null
            ? tile.CenterTop
            : new float3(CurrentCoordinates.x, 0, CurrentCoordinates.y);
    }
}
