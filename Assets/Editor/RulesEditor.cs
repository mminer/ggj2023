using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Rules))]
public class RulesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();
        GUILayout.Label("Stats", EditorStyles.boldLabel);

        var rules = (Rules)target;
        var deckSize = rules.deck.Sum(cardConfig => cardConfig.count);
        EditorGUILayout.LabelField("Deck Size", deckSize.ToString());
    }
}
