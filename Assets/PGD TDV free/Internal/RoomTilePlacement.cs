using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class RoomTilePlacement : MonoBehaviour
{
    private DungeonGenerator dungeonGenerator;
    
    public void StartLoopForTilePlacement()
    {
        dungeonGenerator = FindObjectOfType<DungeonGenerator>();

        foreach (var room in FindObjectOfType<DungeonGenerator>().roomsReferences)
        {
            SetTilesForRoom(room);
        }
    }

    private void SetTilesForRoom(Transform room)
    {
        Vector2Int position = (Vector2Int)dungeonGenerator.grid.WorldToCell(room.position);
    
        Vector2Int roomCenter = new Vector2Int(
            position.x - dungeonGenerator.roomsSize.x / 2, 
            position.y - dungeonGenerator.roomsSize.y / 2);
    
        for (int x = roomCenter.x; x < roomCenter.x + dungeonGenerator.roomsSize.x; x++)
        {
            for (int y = roomCenter.y; y < roomCenter.y + dungeonGenerator.roomsSize.y; y++)
            {
                if (x == roomCenter.x || x == roomCenter.x + dungeonGenerator.roomsSize.x - 1 || y == roomCenter.y || y == roomCenter.y + dungeonGenerator.roomsSize.y - 1)
                {
                    dungeonGenerator.wallsTileMap.SetTile(new Vector3Int(x, y, 0), dungeonGenerator.wallTile);
                }
                else
                {
                    dungeonGenerator.floorsTileMap.SetTile(new Vector3Int(x, y, 0), dungeonGenerator.floorTile);
                }
            }
        }
    
        for (int i = 0; i < dungeonGenerator.hallsWidth; i++)
        {
            if(room.GetComponent<RoomPropeties>().neighbors.Contains(RoomNeighbors.North))
            {
                Vector3Int topDoorPosition = new Vector3Int(roomCenter.x + dungeonGenerator.roomsSize.x / 2 - dungeonGenerator.hallsWidth / 2 + i, roomCenter.y + dungeonGenerator.roomsSize.y - 1, 0);
                dungeonGenerator.floorsTileMap.SetTile(topDoorPosition, dungeonGenerator.floorTile);
                dungeonGenerator.wallsTileMap.SetTile(topDoorPosition, null);
                dungeonGenerator.doorsMap.SetTile(topDoorPosition, dungeonGenerator.doorTile);
                if (!IsPathPlaced(new Vector3Int(topDoorPosition.x, topDoorPosition.y + 1, 0), Vector3Int.up))
                {
                    CreatePathBetweenRooms(new Vector3Int(topDoorPosition.x, topDoorPosition.y + 1, 0), Vector3Int.up);
                    CreatePathWalls(new Vector3Int(topDoorPosition.x, topDoorPosition.y + 1, 0), Vector3Int.up);
                }
            }
            if(room.GetComponent<RoomPropeties>().neighbors.Contains(RoomNeighbors.South))
            {
                Vector3Int bottomDoorPosition = new Vector3Int(roomCenter.x + dungeonGenerator.roomsSize.x / 2 - dungeonGenerator.hallsWidth / 2 + i, roomCenter.y, 0);
                dungeonGenerator.floorsTileMap.SetTile(bottomDoorPosition, dungeonGenerator.floorTile);
                dungeonGenerator.wallsTileMap.SetTile(bottomDoorPosition, null);
                dungeonGenerator.doorsMap.SetTile(bottomDoorPosition, dungeonGenerator.doorTile);
                if (!IsPathPlaced(new Vector3Int(bottomDoorPosition.x, bottomDoorPosition.y - 1, 0), Vector3Int.down))
                {
                    CreatePathBetweenRooms(new Vector3Int(bottomDoorPosition.x, bottomDoorPosition.y - 1, 0), Vector3Int.down);
                    CreatePathWalls(new Vector3Int(bottomDoorPosition.x, bottomDoorPosition.y - 1, 0), Vector3Int.down);
                }
            }
            if(room.GetComponent<RoomPropeties>().neighbors.Contains(RoomNeighbors.East))
            {
                Vector3Int rightDoorPosition = new Vector3Int(roomCenter.x + dungeonGenerator.roomsSize.x - 1, roomCenter.y + dungeonGenerator.roomsSize.y / 2 - dungeonGenerator.hallsWidth / 2 + i, 0);
                dungeonGenerator.floorsTileMap.SetTile(rightDoorPosition, dungeonGenerator.floorTile);
                dungeonGenerator.wallsTileMap.SetTile(rightDoorPosition, null);
                dungeonGenerator.doorsMap.SetTile(rightDoorPosition, dungeonGenerator.doorTile);
                if (!IsPathPlaced(new Vector3Int(rightDoorPosition.x + 1, rightDoorPosition.y, 0), Vector3Int.right))
                {
                    CreatePathBetweenRooms(new Vector3Int(rightDoorPosition.x + 1, rightDoorPosition.y, 0), Vector3Int.right);
                    CreatePathWalls(new Vector3Int(rightDoorPosition.x + 1, rightDoorPosition.y, 0), Vector3Int.right);
                }
            }
            if(room.GetComponent<RoomPropeties>().neighbors.Contains(RoomNeighbors.West))
            {
                Vector3Int leftDoorPosition = new Vector3Int(roomCenter.x, roomCenter.y + dungeonGenerator.roomsSize.y / 2 - dungeonGenerator.hallsWidth / 2 + i, 0);
                dungeonGenerator.floorsTileMap.SetTile(leftDoorPosition, dungeonGenerator.floorTile);
                dungeonGenerator.wallsTileMap.SetTile(leftDoorPosition, null);
                dungeonGenerator.doorsMap.SetTile(leftDoorPosition, dungeonGenerator.doorTile);
                if (!IsPathPlaced(new Vector3Int(leftDoorPosition.x - 1, leftDoorPosition.y, 0), Vector3Int.left))
                {
                    CreatePathBetweenRooms(new Vector3Int(leftDoorPosition.x - 1, leftDoorPosition.y, 0), Vector3Int.left);
                    CreatePathWalls(new Vector3Int(leftDoorPosition.x - 1, leftDoorPosition.y, 0), Vector3Int.left);
                }
            }
        }
    }
    
    private bool IsPathPlaced(Vector3Int startPosition, Vector3Int direction)
    {
        // Verificar si el camino ya está colocado
        for (int i = 0; i < dungeonGenerator.hallsLength; i++)
        {
            Vector3Int position = startPosition + direction * i;
            if (dungeonGenerator.floorsTileMap.GetTile(position) == null)
            {
                return false;
            }
        }
        return true;
    }

    private void CreatePathBetweenRooms(Vector3Int startPosition, Vector3Int direction)
    {
        for (int i = 0; i < dungeonGenerator.hallsLength; i++)
        {
            Vector3Int position = startPosition + direction * i;
            dungeonGenerator.floorsTileMap.SetTile(position, dungeonGenerator.floorTile);
            dungeonGenerator.wallsTileMap.SetTile(position, null);
        }
    }

    private void CreatePathWalls(Vector3Int startPosition, Vector3Int direction)
    {
        int halfWidth = dungeonGenerator.hallsWidth / 2;
        bool isOddWidth = dungeonGenerator.hallsWidth % 2 != 0;

        for (int i = 0; i < dungeonGenerator.hallsLength; i++)
        {
            Vector3Int position = startPosition + direction * i;

            if (direction == Vector3Int.up || direction == Vector3Int.down)
            {
                CreateVerticalWalls(position, halfWidth, isOddWidth);
            }
            else if (direction == Vector3Int.left || direction == Vector3Int.right)
            {
                CreateHorizontalWalls(position, halfWidth, isOddWidth);
            }
        }
    }

    private void CreateVerticalWalls(Vector3Int position, int halfWidth, bool isOddWidth)
    {
        for (int j = 1; j <= halfWidth; j++)
        {
            Vector3Int leftPosition = new Vector3Int(position.x - j, position.y, 0);
            Vector3Int rightPosition = new Vector3Int(position.x + j, position.y, 0);

            PlaceWallIfEmpty(leftPosition);
            PlaceWallIfEmpty(rightPosition);
        }

        if (isOddWidth)
        {
            Vector3Int centerPosition = new Vector3Int(position.x, position.y, 0);
            PlaceWallIfEmpty(centerPosition);
        }
    }

    private void CreateHorizontalWalls(Vector3Int position, int halfWidth, bool isOddWidth)
    {
        for (int j = 1; j <= halfWidth; j++)
        {
            Vector3Int topPosition = new Vector3Int(position.x, position.y + j, 0);
            Vector3Int bottomPosition = new Vector3Int(position.x, position.y - j, 0);

            PlaceWallIfEmpty(topPosition);
            PlaceWallIfEmpty(bottomPosition);
        }

        if (isOddWidth)
        {
            Vector3Int centerPosition = new Vector3Int(position.x, position.y, 0);
            PlaceWallIfEmpty(centerPosition);
        }
    }

    private void PlaceWallIfEmpty(Vector3Int position)
    {
        if (dungeonGenerator.floorsTileMap.GetTile(position) == null && dungeonGenerator.wallsTileMap.GetTile(position) == null)
        {
            dungeonGenerator.wallsTileMap.SetTile(position, dungeonGenerator.wallTile);
        }
    }
}
