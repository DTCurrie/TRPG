using System.IO;
using UnityEngine;
using UnityEditor;

public static class ResourceExtensions
{
    private static void CheckResourcesPath()
    {
        if (!Directory.Exists(Settings.ResourcesPath))
            AssetDatabase.CreateFolder("Assets", "Resources");
    }

    public static void CreateSaveDirectory(string name)
    {
        CheckResourcesPath();

        if (!Directory.Exists($"{Settings.ResourcesPath}/{name}"))
            AssetDatabase.CreateFolder("Assets/Resources", name);

        AssetDatabase.Refresh();
    }

    public static void CreateSaveDirectory(string[] names)
    {
        var path = Settings.ResourcesPath;
        CheckResourcesPath();

        for (int i = 0; i < names.Length; i++)
        {
            Debug.Log($"checking {path}/{names[i]}");

            if (!Directory.Exists($"{path}/{names[i]}"))
                AssetDatabase.CreateFolder(path, names[i]);

            path += $"/{names[i]}";
        }

        AssetDatabase.Refresh();
    }
}