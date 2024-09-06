using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DungeonGenerator))]
public class DungeonGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DungeonGenerator dungeonGenerator = (DungeonGenerator)target;

        GUILayout.Space(25);

        //make this two buttons in the same line

        GUILayout.Label("Dungeon Generator Buttons", EditorStyles.boldLabel, GUILayout.ExpandWidth(true));

        GUILayout.BeginHorizontal();

        if(GUILayout.Button("Generate Dungeon"))
        {
            dungeonGenerator.ClearDungeon();
            dungeonGenerator.GenerateDungeon();
        }

        if(GUILayout.Button("Clear Dungeon"))
        {
            dungeonGenerator.ClearDungeon();
        }

        GUILayout.EndHorizontal();
    }
}
