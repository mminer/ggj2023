using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Hero))]
public class HeroEditor : Editor
{
    Card card;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var hero = (Hero)target;

        using (new EditorGUILayout.HorizontalScope())
        {
            card = (Card)EditorGUILayout.EnumPopup(card);

            if (GUILayout.Button("Apply Card"))
            {
                hero.StartCoroutine(hero.ApplyCard(card));
            }
        }
    }
}
