using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomGeneration : MonoBehaviour
{

    public void RoomsGeneration()
    {
        //? generates first room reference in initial position and add it to the list of rooms
        Grid grid = GetComponent<DungeonGenerator>().grid;

        GameObject roomRef = new GameObject("RoomReference");
        roomRef.transform.position = grid.CellToWorld(new Vector3Int
        (
            (int)GetComponent<DungeonGenerator>().initialPosition.x,
            (int)GetComponent<DungeonGenerator>().initialPosition.y,
            0
        ));
        roomRef.transform.SetParent(transform);

        GetComponent<DungeonGenerator>().roomsReferences.Add(roomRef.transform);

        LoopForRoomsReferences();
    }

    #region RoomsReferences

    private void LoopForRoomsReferences()
    {
        int numberOfRooms = GetComponent<DungeonGenerator>().numberOfRooms;
        Vector2 roomSize = GetComponent<DungeonGenerator>().roomsSize;
        Vector2 newPosition = Vector2.zero;
        Transform roomReference;
        Vector2 direction;

        // starts the while loop to generate the rooms references
            //Gets a random room reference from the list of rooms
            //Gets a random direction from that room reference
            //Checks if the room reference is already in that position
            //if is not instantiate and save the transform in the list
            //if is don't save it 
            //if the list is full break the loop
        // ends the while loop

        do
        {
            roomReference = GetComponent<DungeonGenerator>().roomsReferences[UnityEngine.Random.Range(0, GetComponent<DungeonGenerator>().roomsReferences.Count)];
            direction = RandomDirection();
            
            if(GetComponent<DungeonGenerator>().hasHalls)
            {
                int hallsLength = GetComponent<DungeonGenerator>().hallsLength;

                newPosition = new Vector2
                (
                    roomReference.position.x + 
                    DistanceBetweenRooms((int)direction.x, (int)roomSize.x, hallsLength),
                    
                    roomReference.position.y + 
                    DistanceBetweenRooms((int)direction.y, (int)roomSize.y, hallsLength)
                );
            }else
            {
                newPosition = new Vector2
                (
                    roomReference.position.x + 
                    DistanceBetweenRooms((int)direction.x, (int)roomSize.x, 0),

                    roomReference.position.y + 
                    DistanceBetweenRooms((int)direction.y, (int)roomSize.y, 0)
                );
            }

            if (!IsRoomReferenceInPosition(newPosition))
            {
                GameObject roomRef = new GameObject("RoomReference");
                roomRef.transform.position = new Vector3(newPosition.x, newPosition.y, 0);
                roomRef.transform.SetParent(transform);

                GetComponent<DungeonGenerator>().roomsReferences.Add(roomRef.transform);
            }
        } while(GetComponent<DungeonGenerator>().roomsReferences.Count < numberOfRooms);

        LoopForRoomsCreation();
    }

    private Vector2 RandomDirection()
    {
        // get a random direction

        switch (UnityEngine.Random.Range(0, 4))
        {
            case 0:
                return Vector2.up;
            case 1:
                return Vector2.down;
            case 2:
                return Vector2.left;
            case 3:
                return Vector2.right;
        }

        return Vector2.zero;
    }

    private int DistanceBetweenRooms(int direction, int roomSize, int hallsLength)
    {
        // calculate the distance between rooms
        int distance = 0;

        if(hallsLength > 0)
        {
            distance = (direction * roomSize) + (direction * hallsLength);
        }
        else
        {
            distance = (direction * roomSize);
        }

        return distance;
    }

    private bool IsRoomReferenceInPosition(Vector2 newPosition)
    {
        // check if the room reference is in that position
        foreach (Transform room in GetComponent<DungeonGenerator>().roomsReferences)
        {
            if (room.position == new Vector3(newPosition.x, newPosition.y, 0))
            {
                Debug.Log("Room reference in position");
                return true;
            }
        }
        Debug.Log("Room reference not in position");
        return false;
    }

    #endregion

    #region RoomsCreation

    public void LoopForRoomsCreation()
    {
        foreach (Transform roomReference in GetComponent<DungeonGenerator>().roomsReferences)
        {
            Vector2Int roomCenter = new Vector2Int
            (
                (int)roomReference.position.x - (int)GetComponent<DungeonGenerator>().roomsSize.x / 2,
                (int)roomReference.position.y - (int)GetComponent<DungeonGenerator>().roomsSize.y / 2
            );

            CreateRoom(roomReference, roomCenter);
        }
    }

    private void CreateRoom(Transform roomReference, Vector2Int roomCenter)
    {
        Vector2Int roomSize = GetComponent<DungeonGenerator>().roomsSize;
        int hallsLength = GetComponent<DungeonGenerator>().hallsLength;

        for (int x = 0; x < roomSize.x; x++)
        {
            for (int y = 0; y < roomSize.y; y++)
            {
                Vector3Int position = new Vector3Int
                (
                    roomCenter.x + x,
                    roomCenter.y + y,
                    0
                );

                if (x == 0 || x == GetComponent<DungeonGenerator>().roomsSize.x - 1 || y == 0 || y == GetComponent<DungeonGenerator>().roomsSize.y - 1)
                {
                    GetComponent<DungeonGenerator>().wallsTileMap.SetTile(position, GetComponent<DungeonGenerator>().wallTile);
                }
                else
                {
                    GetComponent<DungeonGenerator>().floorsTileMap.SetTile(position, GetComponent<DungeonGenerator>().floorTile);
                }
            }
        }
        
        Tilemap wallsMap = GetComponent<DungeonGenerator>().wallsTileMap;
        Tilemap floorsMap = GetComponent<DungeonGenerator>().floorsTileMap;
        Tilemap doorsMap = GetComponent<DungeonGenerator>().doorsMap;
        Tile doorTile = GetComponent<DungeonGenerator>().doorTile;
        Tile floorTile = GetComponent<DungeonGenerator>().floorTile;
        Tile wallTile = GetComponent<DungeonGenerator>().wallTile;

        for (int i = 0; i < hallsLength; i++)
        {
            // Bottom door
            Vector3Int bottomDoorPosition = new Vector3Int(roomCenter.x + roomSize.x / 2 - hallsLength / 2 + i, roomCenter.y, 0);
            floorsMap.SetTile(bottomDoorPosition, floorTile);
            wallsMap.SetTile(bottomDoorPosition, null);
            doorsMap.SetTile(bottomDoorPosition, doorTile);
            CreatePath(new Vector3Int(bottomDoorPosition.x, bottomDoorPosition.y - 1, 0), Vector3Int.down, hallsLength, floorsMap, wallsMap, floorTile);
            CreatePathWalls(new Vector3Int(bottomDoorPosition.x, bottomDoorPosition.y - 1, 0), Vector3Int.down, hallsLength, floorsMap, wallsMap, wallTile);

            // Top door
            Vector3Int topDoorPosition = new Vector3Int(roomCenter.x + roomSize.x / 2 - hallsLength / 2 + i, roomCenter.y + roomSize.y - 1, 0);
            floorsMap.SetTile(topDoorPosition, floorTile);
            wallsMap.SetTile(topDoorPosition, null);
            doorsMap.SetTile(topDoorPosition, doorTile);
            CreatePath(new Vector3Int(topDoorPosition.x, topDoorPosition.y + 1, 0), Vector3Int.up, hallsLength, floorsMap, wallsMap, floorTile);
            CreatePathWalls(new Vector3Int(topDoorPosition.x, topDoorPosition.y + 1, 0), Vector3Int.up, hallsLength, floorsMap, wallsMap, wallTile);

            // Left door
            Vector3Int leftDoorPosition = new Vector3Int(roomCenter.x, roomCenter.y + roomSize.y / 2 - hallsLength / 2 + i, 0);
            floorsMap.SetTile(leftDoorPosition, floorTile);
            wallsMap.SetTile(leftDoorPosition, null);
            doorsMap.SetTile(leftDoorPosition, doorTile);
            CreatePath(new Vector3Int(leftDoorPosition.x - 1, leftDoorPosition.y, 0), Vector3Int.left, hallsLength, floorsMap, wallsMap, floorTile);
            CreatePathWalls(new Vector3Int(leftDoorPosition.x - 1, leftDoorPosition.y, 0), Vector3Int.left, hallsLength, floorsMap, wallsMap, wallTile);

            // Right door
            Vector3Int rightDoorPosition = new Vector3Int(roomCenter.x + roomSize.x - 1, roomCenter.y + roomSize.y / 2 - hallsLength / 2 + i, 0);
            floorsMap.SetTile(rightDoorPosition, floorTile);
            wallsMap.SetTile(rightDoorPosition, null);
            doorsMap.SetTile(rightDoorPosition, doorTile);
            CreatePath(new Vector3Int(rightDoorPosition.x + 1, rightDoorPosition.y, 0), Vector3Int.right, hallsLength, floorsMap, wallsMap, floorTile);
            CreatePathWalls(new Vector3Int(rightDoorPosition.x + 1, rightDoorPosition.y + 1, 0), Vector3Int.right, hallsLength, floorsMap, wallsMap, wallTile);
        }

        
    }

    private void CreatePath(Vector3Int startPosition, Vector3Int direction, int pathLength, Tilemap floorsMap, Tilemap wallsMap, Tile floorTile)
    {
        for (int i = 0; i < pathLength; i++)
        {
            Vector3Int position = startPosition + direction * i;
            floorsMap.SetTile(position, floorTile);
            wallsMap.SetTile(position, null);
        }
    }

    private void CreatePathWalls(Vector3Int startPosition, Vector3Int direction, int pathLength, Tilemap floorsMap, Tilemap wallsMap, Tile wallTile)
    {
        for (int i = 0; i < pathLength; i++)
        {
            Vector3Int position = startPosition + direction * i;

            // Calculate the positions for the walls on both sides of the path

            Vector3Int leftWallPosition = position + new Vector3Int(-direction.y, direction.x, 0);
            Vector3Int rightWallPosition = position + new Vector3Int(direction.y, -direction.x, 0);

            // Place the wall tiles if there are no floor tiles
            if (floorsMap.GetTile(leftWallPosition) == null)
            {
                wallsMap.SetTile(leftWallPosition, wallTile);
            }
            if (floorsMap.GetTile(rightWallPosition) == null)
            {
                wallsMap.SetTile(rightWallPosition, wallTile);
            }
        }
    }

    #endregion
}
