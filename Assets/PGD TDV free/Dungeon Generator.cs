using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    public List<Transform> roomsReferences;
    public int numberOfRooms;
    

    [Header("Rooms")]
    public Vector2Int initialPosition;
    public Vector2Int roomsSize;


    [Header("Halls")]
    public bool hasHalls;
    public int hallsLength;
    public int hallsWidth;


    [Header("Tilemaps & Tiles")]
    public Grid grid;

    public Tilemap wallsTileMap;
    public Tile wallTile;
    public Tilemap floorsTileMap;
    public Tile floorTile;
    public Tilemap doorsMap;
    public Tile doorTile;

    private void Start() {
        InstRoomRef();
        
    }

    private void InstRoomRef()
    {
        if(numberOfRooms == 0)
        {
            Debug.LogError("No rooms to generate");
            return;
        }

        GameObject roomRef = new GameObject("RoomReference");

        roomRef.transform.position = grid.GetCellCenterWorld(new Vector3Int
        (
            (int)initialPosition.x,
            (int)initialPosition.y,
            0
        ));

        roomRef.transform.SetParent(transform);
        roomsReferences.Add(roomRef.transform);

        ///----------------------------------------------
        
        Vector2 roomNewPosition = Vector2.zero;
        
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

            if(CheckNeighbors.CheckFreePositions(roomRef.transform, roomsReferences, direction, roomsSize, hallsLength))
            {
                GameObject newRoomRef = new GameObject("RoomReference");
                newRoomRef.transform.position = new Vector3(roomNewPosition.x, roomNewPosition.y, 0);
                newRoomRef.transform.SetParent(transform);
                roomsReferences.Add(newRoomRef.transform);
            }
        }

        
        foreach (var room in roomsReferences)
        {
            room.AddComponent<RoomPropeties>();   
        }

        SetRoomsNeighbors();
    }

    private void SetRoomsNeighbors()
    {
        foreach (var room in roomsReferences)
        {
            if(CheckNeighbors.CheckFreePositions(room, roomsReferences, Vector2.up, roomsSize, hallsLength))
            {
                if(room.GetComponent<RoomPropeties>().neighbors == null)
                {
                    room.GetComponent<RoomPropeties>().neighbors = new List<RoomNeighbors>();
                }
                room.GetComponent<RoomPropeties>().neighbors.Add(RoomNeighbors.North);
            }
            if(CheckNeighbors.CheckFreePositions(room, roomsReferences, Vector2.down, roomsSize, hallsLength))
            {
                if(room.GetComponent<RoomPropeties>().neighbors == null)
                {
                    room.GetComponent<RoomPropeties>().neighbors = new List<RoomNeighbors>();
                }
                room.GetComponent<RoomPropeties>().neighbors.Add(RoomNeighbors.South);
            }
            if(CheckNeighbors.CheckFreePositions(room, roomsReferences, Vector2.left, roomsSize, hallsLength))
            {
                if(room.GetComponent<RoomPropeties>().neighbors == null)
                {
                    room.GetComponent<RoomPropeties>().neighbors = new List<RoomNeighbors>();
                }
                room.GetComponent<RoomPropeties>().neighbors.Add(RoomNeighbors.West);
            }
            if(CheckNeighbors.CheckFreePositions(room, roomsReferences, Vector2.right, roomsSize, hallsLength))
            {
                if(room.GetComponent<RoomPropeties>().neighbors == null)
                {
                    room.GetComponent<RoomPropeties>().neighbors = new List<RoomNeighbors>();
                }
                room.GetComponent<RoomPropeties>().neighbors.Add(RoomNeighbors.East);
            }
        }

        //SetRoomsTiles();
    }

    private void SetRoomsTiles()
    {
        foreach (var room in roomsReferences)
        {
            Vector2Int roomPosition = new Vector2Int((int)room.transform.position.x, (int)room.transform.position.y);
            Vector2Int roomCenter = TileCalculate.CalculateRoomCenter(roomPosition, roomsSize);

            for (int x = roomPosition.x; x < roomPosition.x + roomsSize.x; x++)
            {
                for (int y = roomPosition.y; y < roomPosition.y + roomsSize.y; y++)
                {
                    if (TileCalculate.IsWallTIle(new Vector2Int(x, y), roomCenter, roomsSize))
                    {
                        wallsTileMap.SetTile(new Vector3Int(x, y, 0), wallTile);
                    }
                    else
                    {
                        floorsTileMap.SetTile(new Vector3Int(x, y, 0), floorTile);
                    }
                }
            }
        }
    }


}

public enum RoomNeighbors
{
    North,
    East,
    South,
    West
}
