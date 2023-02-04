using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Hero))]
public class HeroEditor : Editor
{
    Card card;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        using (new EditorGUILayout.HorizontalScope())
        {
            card = (Card)EditorGUILayout.EnumPopup(card);

            if (GUILayout.Button("Play Card"))
            {
                var hero = target as Hero;
                hero.PlayCard(card);
            }
        }
    }
}
