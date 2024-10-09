using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using PGD_TDV;

public class DungeonGenerator : MonoBehaviour
{
    public DungeonGeneratorSo dungeonGeneratorSo;

    
    [Range(1, 500)]public int numberOfRooms;

    [Header("Rooms")]
    public Vector2Int initialPosition;
    public Vector2Int roomsSize;


    [Header("Halls & Doors")]
    public int hallsLength;
    public int hallsWidth;
    public int doorsWidth;
    [Range(0, 100)] public int roomConnectionChance;


    [Header("Tiles")]
    public Tile wallTile;
    public Tile floorTile;
    public Tile doorTile;

    private Grid grid;
    private Tilemap wallsTileMap;
    private Tilemap floorsTileMap;
    private Tilemap doorsMap;

    [Space(5)][Header("Room Properties"), Header("Spawn Rates")]
    public int fightRoomSpawnRate;
    public int chestRoomSpawnRate;
    public int emptyRoomSpawnRate;
    [Header("other properties")]
    public bool fightRoomCanCloseDoors;

    [Space(5)][Header("Generation")]
    public List<Transform> roomsReferences;
    public float generationProgress;

    private void Start() {
        GenerateDungeon();
    }

    public void GenerateDungeon()
    {
        if(grid == null)
        {
            GenerateGrid();
        }

        InstRoomRef();
    }

    public void ClearDungeon()
    {
        if(grid == null)
        {
            return;
        }

        foreach (var room in roomsReferences)
        {
            DestroyImmediate(room.gameObject);
        }

        roomsReferences.Clear();

        wallsTileMap.ClearAllTiles();
        floorsTileMap.ClearAllTiles();
        doorsMap.ClearAllTiles();
        generationProgress = 0;
    }

    private void InstRoomRef()
    {
        //<summary>
        //  This method instantiate the first room reference and call the method to generate the dungeon
        //</summary>
        if(numberOfRooms == 0)
        {
            Debug.LogError("No rooms to generate");
            return;
        }

        GameObject roomRef = new GameObject("RoomReference");

        Vector3 roomPosition = grid.GetCellCenterWorld(new Vector3Int
        (
            (int)initialPosition.x,
            (int)initialPosition.y,
            0
        ));
        roomRef.transform.position = new Vector3(roomPosition.x, roomPosition.y, 0);

        roomRef.transform.SetParent(transform);
        roomsReferences.Add(roomRef.transform);

        //<summary>
        //  This method call the method to generate the rest of the rooms
        //</summary>
        
        Vector2 roomNewPosition = Vector2.zero;
        int laps = 0;
        
        while(roomsReferences.Count < numberOfRooms)
        {
            Transform roomReference = roomsReferences[Random.Range(0, roomsReferences.Count)];
            Vector2 direction = CheckNeighbors.RandomDirection();

            if(hallsLength > 0)
            {
                roomNewPosition = new Vector2
                (
                    roomReference.position.x + CheckNeighbors.DistanceToTheNeighbor((int)direction.x, roomsSize.x, hallsLength),
                    roomReference.position.y + CheckNeighbors.DistanceToTheNeighbor((int)direction.y, roomsSize.y, hallsLength)
                );
            }else
            {
                roomNewPosition = new Vector2
                (
                    roomReference.position.x + CheckNeighbors.DistanceToTheNeighbor((int)direction.x, roomsSize.x, 0),
                    roomReference.position.y + CheckNeighbors.DistanceToTheNeighbor((int)direction.y, roomsSize.y, 0)
                );
            }           

            if(CheckNeighbors.CheckFreePositions(roomNewPosition, roomsReferences))
            {
                GameObject newRoomRef = new GameObject("RoomReference");
                newRoomRef.transform.position = new Vector3(roomNewPosition.x, roomNewPosition.y, 0);
                newRoomRef.transform.SetParent(transform);
                roomsReferences.Add(newRoomRef.transform);
            }

            laps++;

            if(laps > 5000)
            {
                Debug.LogWarning("Wtf are you doing? to much rooms fo the amount of iterations");
                break;
            }
        }
        AddGenerationProgress(30);

        //<summary>
        //  This method add the RoomProperties script to the rooms and starts a new List of RoomNeighbors
        //</summary>
        
        foreach (var room in roomsReferences)
        {
            room.AddComponent<RoomPropeties>().neighbors = new List<RoomNeighbors>();
            room.GetComponent<RoomPropeties>().connectedRooms = new List<RoomNeighbors>();
        }
        SetRoomsNeighbors();
    }

    //<summary>
    //  This function adds the neighbors to the rooms
    //</summary>
    private void SetRoomsNeighbors()
    {
        foreach (var room in roomsReferences)
        {
            List<RoomNeighbors> neighbors = CheckNeighbors.CheckForNeighbors(room.transform.position, roomsReferences, roomsSize, hallsLength);
            room.GetComponent<RoomPropeties>().neighbors = neighbors;
        }

        AddGenerationProgress(20);
        SetRoomsTiles();
    }

    //<summary>
    //  This function sets the tiles for the rooms
    //</summary>
    private void SetRoomsTiles()
    {
        foreach (var room in roomsReferences)
        {
            Vector2Int roomPosition = (Vector2Int)grid.WorldToCell(room.position);
            Vector2Int roomCenter = TileCalculate.CalculateRoomCenter(roomsSize, roomPosition);

            for(int x = roomCenter.x; x < roomCenter.x + roomsSize.x; x++)
            {
                for(int y = roomCenter.y; y < roomCenter.y + roomsSize.y; y++)
                {
                    Vector3Int tilePosition = new Vector3Int(x, y, 0);

                    if(TileCalculate.IsWallTIle((Vector2Int)tilePosition, roomCenter, roomsSize))
                    {
                        wallsTileMap.SetTile(tilePosition, wallTile);
                    }else
                    {
                        floorsTileMap.SetTile(tilePosition, floorTile);
                    }
                }
            }
        }

        AddGenerationProgress(10);
        ChanceToConnectRooms();
    }

    private void ChanceToConnectRooms()
    {
        RoomNeighbors neighborChosen = RoomNeighbors.North;

        foreach (var room in roomsReferences)
        {
            List<RoomNeighbors> neighbors = room.GetComponent<RoomPropeties>().neighbors;

            if(neighbors.Count > 1)
            {
                foreach (var neighbor in neighbors)
                {
                    if(Random.Range(0, 100) < roomConnectionChance)
                    {
                        neighborChosen = neighbor;
                    }
                }
            }
            else
            {
                neighborChosen = neighbors[0];  
            }

            Transform neighborTransform = CheckNeighbors.GetNeighborTransform(room, roomsReferences, roomsSize, hallsLength, neighborChosen);
            if(roomsReferences.Contains(neighborTransform))
            {
                neighborTransform.GetComponent<RoomPropeties>().connectedRooms.Add(CheckNeighbors.GetOppositeDirection(neighborChosen));

                if(!CheckNeighbors.CheckIfNeighborIsOnList(neighborChosen, room.GetComponent<RoomPropeties>().connectedRooms))
                    room.GetComponent<RoomPropeties>().connectedRooms.Add(neighborChosen);

                if(!CheckNeighbors.CheckIfNeighborIsOnList(CheckNeighbors.GetOppositeDirection(neighborChosen), neighborTransform.GetComponent<RoomPropeties>().connectedRooms))
                    neighborTransform.GetComponent<RoomPropeties>().connectedRooms.Add(CheckNeighbors.GetOppositeDirection(neighborChosen));
            }else
            {
                Debug.LogWarning("Room not connected" + room.position + " " + neighborChosen);
            }
        }

        //SetDoors();
        CheckConnectedRoomsToSpawn();
    }

    private void CheckConnectedRoomsToSpawn()
    {
        List<Transform> roomsToCheck = new List<Transform>{roomsReferences[0]};
        HashSet<Transform> checkedRooms = new HashSet<Transform>();

        for (int i = 0; i < roomsToCheck.Count; i++)
        {
            var room = roomsToCheck[i];
            if (checkedRooms.Contains(room))
                continue;

            List<RoomNeighbors> connectedNeighbors = room.GetComponent<RoomPropeties>().connectedRooms;

            foreach (var neighbor in connectedNeighbors)
            {
                Transform neighborTransform = CheckNeighbors.GetNeighborTransform(room, roomsReferences, roomsSize, hallsLength, neighbor);
                if (neighborTransform != null && !checkedRooms.Contains(neighborTransform))
                {
                    roomsToCheck.Add(neighborTransform);
                }
            }

            room.tag = "ConnectedToSpawn";
            room.GetComponent<RoomPropeties>().isConnectedToMainRoom = true;
            checkedRooms.Add(room);
        }

        ConnectRooms();
        
    }

    private void ConnectRooms()
    {
        //checks if in the roomReferences list there is a room that is not connected to the main room
        List<Transform> roomsNotConnected = roomsReferences.FindAll(x => x.GetComponent<RoomPropeties>().isConnectedToMainRoom == false);

        if(roomsReferences.Exists(x => x.GetComponent<RoomPropeties>().isConnectedToMainRoom == false))
        {
            //if there is a room that is not connected to the main room, it will check if there is a room connected to the main room
            foreach (var room in roomsNotConnected)
            {
                List<RoomNeighbors> neighborsToCheck = room.GetComponent<RoomPropeties>().neighbors;
                //pick a random neighbor that is connected to the main room and connect the room to it
                RoomNeighbors neighborChosen = neighborsToCheck[Random.Range(0, neighborsToCheck.Count)];
                Transform neighborTransform = CheckNeighbors.GetNeighborTransform(room, roomsReferences, roomsSize, hallsLength, neighborChosen);

                if(neighborTransform.GetComponent<RoomPropeties>().isConnectedToMainRoom)
                {
                    neighborTransform.GetComponent<RoomPropeties>().connectedRooms.Add(CheckNeighbors.GetOppositeDirection(neighborChosen));
                    room.GetComponent<RoomPropeties>().connectedRooms.Add(neighborChosen);
                    room.GetComponent<RoomPropeties>().isConnectedToMainRoom = true;
                }
                
            }
        }
        
        if(roomsReferences.Exists(x => x.GetComponent<RoomPropeties>().isConnectedToMainRoom == false))
        {
            ConnectRooms();
        }
        else
        {
            SetDoors();
        }
    }

    //<summary>
    //  This function sets the doors for the rooms
    //</summary>
    private void SetDoors()
    {
        foreach (var room in roomsReferences)
        {
            Vector2Int roomPosition = (Vector2Int)grid.WorldToCell(room.position);
            Vector2Int roomCenter = TileCalculate.CalculateRoomCenter(roomsSize, roomPosition);

            foreach (var neighbor in room.GetComponent<RoomPropeties>().connectedRooms)
            {
                List<Vector2Int> doorsTilePosition = TileCalculate.DoorTilePosition(roomCenter, roomsSize, doorsWidth, neighbor);

                foreach (var doorPosition in doorsTilePosition)
                {
                    doorsMap.SetTile((Vector3Int)doorPosition, doorTile);
                    floorsTileMap.SetTile((Vector3Int)doorPosition, floorTile);
                    wallsTileMap.SetTile((Vector3Int)doorPosition, null);
                }
            }
        }

        AddGenerationProgress(10);
        if(hallsLength > 0)
        {
            SetHalls();
        }
    }

    //<summary>
    //  This function sets the halls for the rooms
    //</summary>
    private void SetHalls()
    {
        foreach (var room in roomsReferences)
        {
            Vector2Int roomPosition = (Vector2Int)grid.WorldToCell(room.position);
            Vector2Int roomCenter = TileCalculate.CalculateRoomCenter(roomsSize, roomPosition);

            foreach (var neighbor in room.GetComponent<RoomPropeties>().connectedRooms)
            {
                List<Vector2Int> hallsTilePosition = TileCalculate.HallTilePosition(roomCenter, roomsSize, hallsWidth, neighbor, hallsLength);
                List<Vector2Int> hallsWallTilePosition = TileCalculate.HallsWallsTilePosition(roomCenter, roomsSize, hallsWidth, neighbor, hallsLength);

                foreach (var hallPosition in hallsTilePosition)
                {
                    floorsTileMap.SetTile((Vector3Int)hallPosition, floorTile);
                }

                foreach (var wallsPosition in hallsWallTilePosition)
                {
                    wallsTileMap.SetTile((Vector3Int)wallsPosition, wallTile);
                }
            }
        }

        AddGenerationProgress(10);
        SetColliders();
    }

    //<summary>
    //  This function sets the colliders for the rooms
    //</summary>
    private void SetColliders()
    {
        int isNonPairSize = ColliderCalculation.IsNonPairSize(roomsSize);
        bool isNonPairHalls;

        if(hallsWidth % 2 != 0)
        {
            isNonPairHalls = true;
        }else
        {
            isNonPairHalls = false;
        }

        foreach (var room in roomsReferences)
        {
            room.transform.AddComponent<CompositeCollider2D>();
            room.transform.AddComponent<BoxCollider2D>();

            room.GetComponent<BoxCollider2D>().size = ColliderCalculation.RoomColliderSize(roomsSize);
            room.GetComponent<BoxCollider2D>().offset = ColliderCalculation.RoomColliderCenter(isNonPairSize);

            room.GetComponent<CompositeCollider2D>().geometryType = CompositeCollider2D.GeometryType.Polygons;
            room.GetComponent<CompositeCollider2D>().isTrigger = true;

            room.GetComponent<BoxCollider2D>().usedByComposite = true;

            room.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

            if(hallsLength > 0)
            {
                foreach(var neighbor in room.GetComponent<RoomPropeties>().connectedRooms)
                {
                    if(neighbor == RoomNeighbors.North)
                    {
                        BoxCollider2D bC2D = room.transform.AddComponent<BoxCollider2D>();
                        bC2D.usedByComposite = true;
                        bC2D.size = ColliderCalculation.HallwayColliderSize(hallsWidth, hallsLength, Vector2.up);
                        bC2D.offset = ColliderCalculation.HallwayColliderCenter(hallsLength, roomsSize, Vector2.up, isNonPairHalls, isNonPairSize);
                    }
                    if(neighbor == RoomNeighbors.East)
                    {
                        BoxCollider2D bC2D = room.transform.AddComponent<BoxCollider2D>();
                        bC2D.usedByComposite = true;
                        bC2D.size = ColliderCalculation.HallwayColliderSize(hallsWidth, hallsLength, Vector2.right);
                        bC2D.offset = ColliderCalculation.HallwayColliderCenter(hallsLength, roomsSize, Vector2.right, isNonPairHalls, isNonPairSize);
                    }
                    if(neighbor == RoomNeighbors.South)
                    {
                        BoxCollider2D bC2D = room.transform.AddComponent<BoxCollider2D>();
                        bC2D.usedByComposite = true;
                        bC2D.size = ColliderCalculation.HallwayColliderSize(hallsWidth, hallsLength, Vector2.up);
                        bC2D.offset = ColliderCalculation.HallwayColliderCenter(hallsLength, roomsSize, Vector2.down, isNonPairHalls, isNonPairSize);
                    }
                    if(neighbor == RoomNeighbors.West)
                    {
                        BoxCollider2D bC2D = room.transform.AddComponent<BoxCollider2D>();
                        bC2D.usedByComposite = true;
                        bC2D.size = ColliderCalculation.HallwayColliderSize(hallsWidth, hallsLength, Vector2.right);
                        bC2D.offset = ColliderCalculation.HallwayColliderCenter(hallsLength, roomsSize, Vector2.left, isNonPairHalls, isNonPairSize);
                    }
                }
            }
        }

        AddGenerationProgress(20);
    }

    private void SetRoomsProperties()
    {
        foreach (var room in roomsReferences)
        {
            
        }
    }

    //<summary>
    //  This function adds the progress to the generation
    //</summary>
    public void AddGenerationProgress(float progressToAdd)
    {
        StartCoroutine(IncrementProgress(progressToAdd));
    }

    private IEnumerator IncrementProgress(float progressToAdd)
    {
       generationProgress += progressToAdd;
        float targetProgress = Mathf.Clamp(generationProgress, 0, 100);
        while (generationProgress < targetProgress)
        {
            generationProgress = Mathf.Clamp(generationProgress + (progressToAdd * Time.deltaTime), 0, 100);
            yield return null;
        }
    }

    private void GenerateGrid()
    {
        grid = new GameObject("Grid").AddComponent<Grid>();
        wallsTileMap = new GameObject("Walls").AddComponent<Tilemap>();
        floorsTileMap = new GameObject("Floors").AddComponent<Tilemap>();
        doorsMap = new GameObject("Doors").AddComponent<Tilemap>();

        wallsTileMap.transform.SetParent(grid.transform);
        floorsTileMap.transform.SetParent(grid.transform);
        doorsMap.transform.SetParent(grid.transform);

        wallsTileMap.gameObject.AddComponent<TilemapRenderer>().sortingOrder = -2;
        floorsTileMap.gameObject.AddComponent<TilemapRenderer>().sortingOrder = -2;
        doorsMap.gameObject.AddComponent<TilemapRenderer>().sortingOrder = -1;
    }
}



