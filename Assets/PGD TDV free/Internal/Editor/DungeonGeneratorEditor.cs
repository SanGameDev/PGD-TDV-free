using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(DungeonGenerator))]
public class DungeonGeneratorEditor : Editor
{
    bool showDungeonGeneratorSo = false;
    bool showRoomPropertiesSo = false;
    //add all the variables from the so
    DungeonGenerator dungeonGenerator;
    DungeonGeneratorSo dungeonGeneratorSo;
    RoomPropertiesSo roomPropertiesSo;
    
    public override void OnInspectorGUI()
    {
        dungeonGenerator = (DungeonGenerator)target;
        dungeonGeneratorSo = dungeonGenerator.dungeonGeneratorSo;

        #region Dungeon Generator Properties
        GUILayout.BeginHorizontal();
        dungeonGenerator.dungeonGeneratorSo = (DungeonGeneratorSo)EditorGUILayout.ObjectField("Dungeon Generator So", dungeonGenerator.dungeonGeneratorSo, typeof(DungeonGeneratorSo), false);
        showDungeonGeneratorSo = EditorGUILayout.Toggle("", showDungeonGeneratorSo);
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        ShowVars();

        #endregion

        #region Room Properties
        GUILayout.BeginHorizontal();
        dungeonGenerator.roomPropertiesSo = (RoomPropertiesSo)EditorGUILayout.ObjectField("Room Properties So", dungeonGenerator.roomPropertiesSo, typeof(RoomPropertiesSo), false);
        showRoomPropertiesSo = EditorGUILayout.Toggle("", showRoomPropertiesSo);
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        ShowRoomProperties();

        #endregion

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
            Separation("Rooms");
            dungeonGenerator.dungeonGeneratorSo.initialPosition = EditorGUILayout.Vector2IntField("Initial Position", dungeonGeneratorSo.initialPosition);
            dungeonGenerator.dungeonGeneratorSo.roomsSize = EditorGUILayout.Vector2IntField("Rooms Size", dungeonGeneratorSo.roomsSize);
            Separation("Halls & Doors");
            dungeonGenerator.dungeonGeneratorSo.hallsLength = EditorGUILayout.IntField("Halls Length", dungeonGeneratorSo.hallsLength);
            dungeonGenerator.dungeonGeneratorSo.hallsWidth = EditorGUILayout.IntField("Halls Width", dungeonGeneratorSo.hallsWidth);
            dungeonGenerator.dungeonGeneratorSo.doorsWidth = EditorGUILayout.IntField("Doors Width", dungeonGeneratorSo.doorsWidth);
            dungeonGenerator.dungeonGeneratorSo.roomConnectionChance = EditorGUILayout.IntSlider("Connection Chance", dungeonGeneratorSo.roomConnectionChance, 0, 100);
            Separation("Tiles");
            dungeonGenerator.dungeonGeneratorSo.wallTile = (Tile)EditorGUILayout.ObjectField("Wall Tile", dungeonGeneratorSo.wallTile, typeof(Tile), false);
            dungeonGenerator.dungeonGeneratorSo.floorTile = (Tile)EditorGUILayout.ObjectField("Floor Tile", dungeonGeneratorSo.floorTile, typeof(Tile), false);
            dungeonGenerator.dungeonGeneratorSo.doorTile = (Tile)EditorGUILayout.ObjectField("Door Tile", dungeonGeneratorSo.doorTile, typeof(Tile), false);

            GUILayout.Space(15);
        }
    }

    private void ShowRoomProperties()
    {
        if(dungeonGenerator.roomPropertiesSo != null && showRoomPropertiesSo)
        {
            Separation("Spawn Rates");
            dungeonGenerator.roomPropertiesSo.fightRoomSpawnRate = EditorGUILayout.IntField("Fight Room Spawn Rate", dungeonGenerator.roomPropertiesSo.fightRoomSpawnRate);
            dungeonGenerator.roomPropertiesSo.chestRoomSpawnRate = EditorGUILayout.IntField("Chest Room Spawn Rate", dungeonGenerator.roomPropertiesSo.chestRoomSpawnRate);
            dungeonGenerator.roomPropertiesSo.emptyRoomSpawnRate = EditorGUILayout.IntField("Empty Room Spawn Rate", dungeonGenerator.roomPropertiesSo.emptyRoomSpawnRate);
            Separation("other properties");
            dungeonGenerator.roomPropertiesSo.fightRoomCanCloseDoors = EditorGUILayout.Toggle("Fight Room Can Close Doors", dungeonGenerator.roomPropertiesSo.fightRoomCanCloseDoors);
            dungeonGenerator.roomPropertiesSo.collidersForCameraBounds = EditorGUILayout.Toggle("Colliders For Camera Bounds", dungeonGenerator.roomPropertiesSo.collidersForCameraBounds);
        }
    }

    private void Separation(string label)
    {
        GUILayout.Space(15);
        GUILayout.Label(label, EditorStyles.boldLabel);
        GUILayout.Space(5);
    }
}
