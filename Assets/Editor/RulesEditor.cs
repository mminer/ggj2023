using UnityEditor;

[CustomEditor(typeof(Rules))]
public class RulesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var rules = (Rules)target;

        CustomEditorUtility.Header("Stats");
        EditorGUILayout.LabelField("Deck Size", rules.deckSize.ToString());
    }
}
