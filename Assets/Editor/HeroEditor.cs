using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Hero))]
public class HeroEditor : Editor
{
    Command command;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        using (new EditorGUILayout.HorizontalScope())
        {
            command = (Command)EditorGUILayout.EnumPopup(command);

            if (GUILayout.Button("Run Command"))
            {
                var hero = target as Hero;
                hero.RunCommand(command);
            }
        }
    }
}
