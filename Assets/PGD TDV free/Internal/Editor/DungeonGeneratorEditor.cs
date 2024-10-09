using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(DungeonGenerator))]
public class DungeonGeneratorEditor : Editor
{
    bool showDungeonGeneratorSo = false;
    //add all the variables from the so
    DungeonGenerator dungeonGenerator;
    DungeonGeneratorSo dungeonGeneratorSo;
    
    public override void OnInspectorGUI()
    {
        dungeonGenerator = (DungeonGenerator)target;
        dungeonGeneratorSo = dungeonGenerator.dungeonGeneratorSo;

        GUILayout.BeginHorizontal();
        dungeonGenerator.dungeonGeneratorSo = (DungeonGeneratorSo)EditorGUILayout.ObjectField("Dungeon Generator So", dungeonGenerator.dungeonGeneratorSo, typeof(DungeonGeneratorSo), false);
        showDungeonGeneratorSo = EditorGUILayout.Toggle("", showDungeonGeneratorSo);
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        ShowVars();

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

    private void ShowVars() 
    {
        if(dungeonGeneratorSo != null && showDungeonGeneratorSo)
        {
            dungeonGenerator.dungeonGeneratorSo.numberOfRooms = EditorGUILayout.IntSlider("Number of Rooms", dungeonGeneratorSo.numberOfRooms, 1, 500);
            dungeonGenerator.dungeonGeneratorSo.initialPosition = EditorGUILayout.Vector2IntField("Initial Position", dungeonGeneratorSo.initialPosition);
            dungeonGenerator.dungeonGeneratorSo.roomsSize = EditorGUILayout.Vector2IntField("Rooms Size", dungeonGeneratorSo.roomsSize);
            dungeonGenerator.dungeonGeneratorSo.hallsLength = EditorGUILayout.IntField("Halls Length", dungeonGeneratorSo.hallsLength);
            dungeonGenerator.dungeonGeneratorSo.hallsWidth = EditorGUILayout.IntField("Halls Width", dungeonGeneratorSo.hallsWidth);
            dungeonGenerator.dungeonGeneratorSo.doorsWidth = EditorGUILayout.IntField("Doors Width", dungeonGeneratorSo.doorsWidth);
            dungeonGenerator.dungeonGeneratorSo.roomConnectionChance = EditorGUILayout.IntSlider("Room Connection Chance", dungeonGeneratorSo.roomConnectionChance, 0, 100);
            dungeonGenerator.dungeonGeneratorSo.wallTile = (Tile)EditorGUILayout.ObjectField("Wall Tile", dungeonGeneratorSo.wallTile, typeof(Tile), false);
            dungeonGenerator.dungeonGeneratorSo.floorTile = (Tile)EditorGUILayout.ObjectField("Floor Tile", dungeonGeneratorSo.floorTile, typeof(Tile), false);
            dungeonGenerator.dungeonGeneratorSo.doorTile = (Tile)EditorGUILayout.ObjectField("Door Tile", dungeonGeneratorSo.doorTile, typeof(Tile), false);
        }
    }
}
