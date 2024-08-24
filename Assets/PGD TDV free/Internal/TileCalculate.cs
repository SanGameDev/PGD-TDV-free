using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileCalculate
{
    public static Vector2Int CalculateRoomCenter(Vector2Int roomSize, Vector2Int roomPosition)
    {
        return new Vector2Int(roomPosition.x - roomSize.x / 2, roomPosition.y - roomSize.y / 2);
    }

    public static List<Vector2Int> DoorTilePosition(Vector2Int roomCenter, Vector2Int roomsSize, int hallsWidth, int i, RoomNeighbors direction)
    {
        List<Vector2Int> doorsTilePosition = new List<Vector2Int>();

        for(int j = 0; j < hallsWidth; j++)
        {
            if(direction == RoomNeighbors.North)
            {
                doorsTilePosition.Add(new Vector2Int(roomCenter.x + i, roomCenter.y + roomsSize.y / 2 + j));
            }
            else if(direction == RoomNeighbors.South)
            {
                doorsTilePosition.Add(new Vector2Int(roomCenter.x + i, roomCenter.y - roomsSize.y / 2 - j));
            }
            else if(direction == RoomNeighbors.East)
            {
                doorsTilePosition.Add(new Vector2Int(roomCenter.x + roomsSize.x / 2 + j, roomCenter.y + i));
            }
            else if(direction == RoomNeighbors.West)
            {
                doorsTilePosition.Add(new Vector2Int(roomCenter.x - roomsSize.x / 2 - j, roomCenter.y + i));
            }
        }

        return doorsTilePosition;
    }

    public static bool IsWallTIle(Vector2Int position, Vector2Int roomPosition, Vector2Int roomSize)
    {
        if(position.x == roomPosition.x || position.x == roomPosition.x + roomSize.x - 1 || position.y == roomPosition.y || position.y == roomPosition.y + roomSize.y - 1)
        {
            return true;
        }

        return false;
    }
}
