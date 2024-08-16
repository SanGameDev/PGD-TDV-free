using System.Collections;
using System.Collections.Generic;
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


    private void StartGeneration()
    {
        GetComponent<RoomReferenceGeneration>().RoomsGeneration();
    }

    public void GetRoomsNeighbors()
    {
        //roomsReferences[1].GetComponent<RoomPropeties>().CheckNeighbors(roomsSize, grid, hallsLength);
        foreach (var room in roomsReferences)
        {
            room.GetComponent<RoomPropeties>().CheckNeighbors(roomsSize, grid, hallsLength);
        }

        SetRoomsTiles();
    }

    public void SetRoomsTiles()
    {
        GetComponent<RoomTilePlacement>().StartLoopForTilePlacement();
    }

    private void Start() {
        StartGeneration();
    }
}

public enum RoomNeighbors
{
    North,
    East,
    South,
    West
}
