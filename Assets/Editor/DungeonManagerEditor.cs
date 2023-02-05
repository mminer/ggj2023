using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DungeonManager))]
public class DungeonManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var dungeonManager = (DungeonManager)target;

        EditorGUILayout.Space();

        if (GUILayout.Button("Regenerate Dungeon"))
        {
            dungeonManager.GenerateDungeon();
        }
    }
}
