using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PGD_TDV;

public static class CheckNeighbors
{
    public static Vector2 RandomDirection()
    {
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

    public static bool CheckFreePositions(Vector3 room, List<Transform> roomsReferences)
    {
        foreach(Transform roomReference in roomsReferences)
        {
            if(roomReference.position == room)
            {
                return false;
            }
        }
        return true;
    }

    public static int DistanceToTheNeighbor(int direction, int roomSize, int hallsLength)
    {
        if(hallsLength == 0)
        {
            return direction * roomSize;
        }
        else
        {
            return (direction * roomSize) + (direction * hallsLength);
        }
    }

    public static List<RoomNeighbors> CheckForNeighbors(Vector3 room, List<Transform> roomsReferences, Vector2 roomSize, int hallsLength)
    {
        List<RoomNeighbors> neighbors = new List<RoomNeighbors>();

        if(!CheckFreePositions(new Vector3(room.x + DistanceToTheNeighbor(1, (int)roomSize.x, hallsLength), room.y, 0), roomsReferences))
            neighbors.Add(RoomNeighbors.East);

        if(!CheckFreePositions(new Vector3(room.x + DistanceToTheNeighbor(-1, (int)roomSize.x, hallsLength), room.y, 0), roomsReferences))
            neighbors.Add(RoomNeighbors.West);

        if(!CheckFreePositions(new Vector3(room.x, room.y + DistanceToTheNeighbor(1, (int)roomSize.y, hallsLength), 0), roomsReferences))
            neighbors.Add(RoomNeighbors.North);

        if(!CheckFreePositions(new Vector3(room.x, room.y + DistanceToTheNeighbor(-1, (int)roomSize.y, hallsLength), 0), roomsReferences))
            neighbors.Add(RoomNeighbors.South);

        return neighbors;
    }

    public static Transform GetNeighborTransform(Transform room, List<Transform> roomsReferences, Vector2 roomSize, int hallsLength, RoomNeighbors direction)
    {
        if(direction == RoomNeighbors.North)
        {
            return roomsReferences.Find(x => x.position == new Vector3(room.position.x, room.position.y + DistanceToTheNeighbor(1, (int)roomSize.y, hallsLength), 0));
        }
        else if(direction == RoomNeighbors.South)
        {
            return roomsReferences.Find(x => x.position == new Vector3(room.position.x, room.position.y + DistanceToTheNeighbor(-1, (int)roomSize.y, hallsLength), 0));
        }
        else if(direction == RoomNeighbors.East)
        {
            return roomsReferences.Find(x => x.position == new Vector3(room.position.x + DistanceToTheNeighbor(1, (int)roomSize.x, hallsLength), room.position.y, 0));
        }
        else if(direction == RoomNeighbors.West)
        {
            return roomsReferences.Find(x => x.position == new Vector3(room.position.x + DistanceToTheNeighbor(-1, (int)roomSize.x, hallsLength), room.position.y, 0));
        }

        return null;
    }

    public static RoomNeighbors GetOppositeDirection(RoomNeighbors direction)
    {
        if(direction == RoomNeighbors.North)
        {
            return RoomNeighbors.South;
        }
        else if(direction == RoomNeighbors.South)
        {
            return RoomNeighbors.North;
        }
        else if(direction == RoomNeighbors.East)
        {
            return RoomNeighbors.West;
        }
        else
        {
            return RoomNeighbors.East;
        }
    }

    public static bool CheckIfNeighborIsOnList(RoomNeighbors direction, List<RoomNeighbors> neighbors)
    {
        return neighbors.Contains(direction);
    }
}
