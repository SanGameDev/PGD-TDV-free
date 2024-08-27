using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColliderCalculation
{
    public static bool IsNonPairSize(Vector2 roomSize)
    {
        if (roomSize.x % 2 == 0 && roomSize.y % 2 == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static Vector2 RoomColliderSize(Vector2 roomSize)
    {
        Vector2 size = new Vector2();

        size.x = roomSize.x;
        size.y = roomSize.y;

        return size;
    }

    public static Vector2 RoomColliderCenter(bool isNonPairSize)
    {
        Vector2 center = new Vector2();
        if (isNonPairSize)
        {
            center.x = 0;
            center.y = 0;
        }
        else
        {
            center.x = -0.5f;
            center.y = -0.5f;
        }
        return center;
    }

    public static Vector2 HallwayColliderSize(int hallWidth, int hallLength, Vector2 direction)
    {
        Vector2 size = new Vector2();

        if (direction.x == 1)
        {
            size.x = hallLength;
            size.y = hallWidth;
        }
        else if (direction.y == 1)
        {
            size.x = hallWidth;
            size.y = hallLength;
        }

        return size;
    }

    public static Vector2 HallwayColliderCenter(int hallLength, Vector2 roomSize, Vector2 direction)
    {
        Vector2 center = new Vector2();
    
        if (direction.x != 0) // Horizontal
        {
            center.x = ((roomSize.x / 2) + (hallLength / 2.0f)) * direction.x;
            center.y = 0;
        }
        else if (direction.y != 0) // Vertical
        {
            center.x = 0;
            center.y = ((roomSize.y / 2) + (hallLength / 2.0f)) * direction.y;
        }

        center.x -= 0.5f;
        center.y -= 0.5f;
    
        return center;
    }
}
