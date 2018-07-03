using System.IO;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public static class LevelCreatorExtensions
{
    public static void SaveLevel(this LevelCreator levelCreator)
    {
        if (!Directory.Exists(Settings.ResourcesPath + "Levels")) 
            ResourceExtensions.CreateSaveDirectory("Levels");

        var level = ScriptableObject.CreateInstance<LevelData>();
        level.TileData = new List<float3>(levelCreator.Tiles.Count);

        foreach (var tile in levelCreator.Tiles.Values)
            level.TileData.Add(new float3(tile.Coordinates.x, tile.Height, tile.Coordinates.y));

        string fileName = string.Format("{0}/{1}.asset", Settings.ResourcesPath + "Levels", levelCreator.LevelName);
        AssetDatabase.CreateAsset(level, fileName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = level;
    }

    public static void LoadLevel(this LevelCreator levelCreator)
    {
        levelCreator.ClearTiles();
        if (levelCreator.LevelData == null) return;

        foreach (var data in levelCreator.LevelData.TileData)
        {
            var tile = levelCreator.CreateTile();
            tile.Place(data);
            levelCreator.Tiles.Add(tile.Coordinates, tile);
        }
    }
}
