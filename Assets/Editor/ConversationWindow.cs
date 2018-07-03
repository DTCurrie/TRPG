using System.IO;
using UnityEngine;
using UnityEditor;

public class ConversationWindow : EditorWindow
{
    private static EditorWindow _window;
    private string _name = "NewConversation";

    [MenuItem("Window/Create/Conversation")]
    static void Init()
    {
        _window = GetWindow(typeof(ConversationWindow));
        _window.Show();
    }

    private void OnGUI()
    {
        _name = EditorGUILayout.TextField("Name", _name);
        if (GUILayout.Button("Create")) SaveConversationData(_name);
    }

    private void SaveConversationData(string name)
    {
        if (!Directory.Exists(Settings.ResourcesPath + "Conversations"))
            ResourceExtensions.CreateSaveDirectory("Conversations");

        var conversation = CreateInstance<ConversationData>();

        string fileName = string.Format("{0}/{1}.asset", Settings.ResourcesPath + "Conversations", name);
        AssetDatabase.CreateAsset(conversation, fileName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        _window.Close();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = conversation;
    }

}