using UnityEditor;
using UnityEngine;

public static class CustomEditorUtility
{
    public static void Header(string text)
    {
        EditorGUILayout.Space();
        GUILayout.Label(text, EditorStyles.boldLabel);
    }
}
