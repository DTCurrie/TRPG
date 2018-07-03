using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelCreator))]
public class LevelCreatorInspector : Editor
{
    private int AdjustAmount = 1;
    public LevelCreator Current => (LevelCreator)target;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Clear"))
            Current.ClearTiles();
        
        if (GUILayout.Button("Adjustment +"))
            AdjustAmount++;
        
        if (GUILayout.Button("Adjustment -"))
            AdjustAmount--;
        
        if (GUILayout.Button(string.Format("Adjust Current ({0})", AdjustAmount)))
            Current.AdjustCurrentTile(AdjustAmount);
        
        if (GUILayout.Button(string.Format("Adjust Area ({0})", AdjustAmount)))
            Current.AdjustArea(AdjustAmount);
        
        if (GUILayout.Button("Save"))
            Current.SaveLevel();
        
        if (GUILayout.Button("Load"))
            Current.LoadLevel();
        
        if (GUI.changed)
            Current.UpdateSelector();
    }
}
