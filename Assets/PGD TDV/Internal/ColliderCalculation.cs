using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColliderCalculation
{
    public static int IsNonPairSize(Vector2 roomSize)
    {
        if (roomSize.x % 2 != 0 && roomSize.y % 2 != 0)
        {
            return 3; //is both non pair
        }
        else if (roomSize.x % 2 != 0)
        {
            return 2; //x is non pair
        }
        else if (roomSize.y % 2 != 0)
        {
            return 1; //y is non pair
        }
        else
        {
            return 0; //is pair
        }
    }

    public static Vector2 RoomColliderSize(Vector2 roomSize)
    {
        Vector2 size = new Vector2();

        size.x = roomSize.x;
        size.y = roomSize.y;

        return size;
    }

    public static Vector2 RoomColliderCenter(int isNonPairSize)
    {
        Vector2 center = new Vector2();

        if(isNonPairSize == 0)
        {
            center.x = -0.5f;
            center.y = -0.5f;
        }
        else if (isNonPairSize == 1)
        {
            center.x = -0.5f;
            center.y = 0.0f;
        }
        else if (isNonPairSize == 2)
        {
            center.x = 0.0f;
            center.y = -0.5f;
        }
        //Debug.Log("isNonPairSize: " + isNonPairSize);
        return center;
    }

    public static Vector2 HallwayColliderSize(int hallWidth, int hallLength, Vector2 direction)
    {
        Vector2 size = new Vector2();

        if (direction.x == 1)
        {
            size.x = hallLength;
            size.y = hallWidth + 2;
        }
        else if (direction.y == 1)
        {
            size.x = hallWidth + 2;
            size.y = hallLength;
        }

        return size;
    }

    public static Vector2 HallwayColliderCenter(int hallLength, Vector2 roomSize, Vector2 direction, bool isNonPairSize, int roomIsNonPairSize)
    {
        Vector2 center = new Vector2();
    
        if (direction.x != 0) // Horizontal
        {
            center.x = ((roomSize.x / 2) + (hallLength / 2.0f)) * direction.x;
            center.y = 0;

            if(roomIsNonPairSize == 0)
            {
                center.x -= 0.5f;
                center.y -= 0.5f;
            }
            else if(roomIsNonPairSize == 1)
            {
                center.x -= 0.5f;
                center.y -= 0.5f;
            }
            else
            {
                center.y -= 0.5f;
            }
        }
        else if (direction.y != 0) // Vertical
        {
            center.x = 0;
            center.y = ((roomSize.y / 2) + (hallLength / 2.0f)) * direction.y;

            if(roomIsNonPairSize == 0)
            {
                center.x -= 0.5f;
                center.y -= 0.5f;
            }
            else if(roomIsNonPairSize == 2)
            {
                center.x -= 0.5f;
                center.y -= 0.5f;
            }
            else
            {
                center.x -= 0.5f;
            }
        }

        return center;
    }
}
