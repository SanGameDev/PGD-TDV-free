using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CheckNeighbors
{
    public static bool CheckFreePositions(Transform room, List<Transform> roomsReferences, Vector2 direction, Vector2 roomSize, int hallsLengt)
    {
        foreach(Transform roomReference in roomsReferences)
        {
            if(roomReference.position == new Vector3
            (
                room.position.x + DistanceToTheNeighbor((int)direction.x, (int)roomSize.x, hallsLengt),
                room.position.y + DistanceToTheNeighbor((int)direction.y, (int)roomSize.y, hallsLengt),
                0
            ))
            {
                return false;
            }
            else
            {
                return true;
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
}
