using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;

public class DataConverter : AssetPostprocessor
{
    private static Dictionary<string, Action> _parsers;

    static DataConverter()
    {
        var jobsParser = CreateParser<JobData>("Jobs");
        _parsers = new Dictionary<string, Action>  {
            {
                jobsParser.Key,
                () => {
                    var jobs = jobsParser.Value.Invoke();

                    for (var i = 0; i < jobs.Length; i++) {
                        var jobData = jobs[i];
                        var obj = GetOrCreatePrefab<Job>(jobsParser.Key, jobData.Name);
                        var job = obj.GetComponent<Job>();

                        job.BaseStats = jobData.BaseStats;
                        job.GrowthStats = jobData.GrowthStats;

                        GetAttribute(obj, StatTypes.EVD).Modifier = jobData.EVD;
                        GetAttribute(obj, StatTypes.RES).Modifier = jobData.RES;
                        GetAttribute(obj, StatTypes.MOV).Modifier = jobData.MOV;
                        GetAttribute(obj, StatTypes.JMP).Modifier = jobData.JMP;
                    }
                }
            }
        };
    }

    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        for (var i = 0; i < importedAssets.Length; i++)
        {
            var fileName = Path.GetFileName(importedAssets[i]);
            var key = fileName.Split('.')[0];
            if (_parsers.ContainsKey(key)) _parsers[key]?.Invoke();
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static KeyValuePair<string, Func<T[]>> CreateParser<T>(string file) where T : ScriptableObject, IParseable
    {
        return new KeyValuePair<string, Func<T[]>>(file, () => Parse<T>(file));
    }

    private static T[] Parse<T>(string key) where T : ScriptableObject, IParseable
    {
        var filePath = $"{Application.dataPath}/Data/{key}.csv";

        if (!File.Exists(filePath))
        {
            Debug.LogError($"Missing Data: {filePath}");
            return null;
        }

        var readText = File.ReadAllLines($"Assets/Data/{key}.csv");
        filePath = $"Assets/Resources/{key}/";
        ResourceExtensions.CreateSaveDirectory(key);

        var returnArray = new T[readText.Length - 1];

        for (var i = 1; i < readText.Length; ++i)
        {
            var data = ScriptableObject.CreateInstance<T>();
            data.Load(readText[i]);
            returnArray[i - 1] = data;
            AssetDatabase.CreateAsset(data, $"{filePath}{data.Name}.asset");
        }

        return returnArray;
    }

    private static GameObject GetOrCreatePrefab<T>(string key, string name) where T : Component
    {
        string fullPath = $"Assets/Resources/{key}/{name}.prefab";
        var obj = AssetDatabase.LoadAssetAtPath<GameObject>(fullPath);
        if (obj == null) obj = Create<T>(fullPath);
        return obj;
    }

    private static GameObject Create<T>(string fullPath) where T : Component
    {
        var instance = new GameObject("temp");
        instance.AddComponent<T>();
        var prefab = PrefabUtility.CreatePrefab(fullPath, instance);
        UnityEngine.Object.DestroyImmediate(instance);
        return prefab;
    }

    static StatModifierAttribute GetAttribute(GameObject obj, StatTypes stat)
    {
        var attributes = obj.GetComponents<StatModifierAttribute>();

        for (int i = 0; i < attributes.Length; i++)
            if (attributes[i].Stat == stat)
                return attributes[i];

        var attribute = obj.AddComponent<StatModifierAttribute>();
        attribute.Stat = stat;
        return attribute;
    }
}