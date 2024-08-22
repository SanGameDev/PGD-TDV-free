using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPropeties : MonoBehaviour
{
    public List<RoomNeighbors> neighbors;

    public void CheckNeighbors(Vector2Int roomSize, Grid grid, int hallsLength)
    {
        if (neighbors == null)
        {
            neighbors = new List<RoomNeighbors>();
        }

        Vector2Int[] VecDirections = new Vector2Int[]
        {
            new Vector2Int(0, 1),  // Up
            new Vector2Int(0, -1), // Down
            new Vector2Int(-1, 0), // Left
            new Vector2Int(1, 0)   // Right
        };
        
        foreach (var direction in VecDirections)
        {
            Vector2 positionToCheck = (Vector2)transform.position + 
            new Vector2(DistanceBetweenRooms(direction.x, roomSize.x, hallsLength), DistanceBetweenRooms(direction.y, roomSize.y, hallsLength));
            //Debug.Log("Checking neighbors" + positionToCheck);

            foreach (var room in FindObjectOfType<DungeonGenerator>().roomsReferences)
            {
                //Debug.Log("Checking neighbors" + room.transform.position + " " + grid.WorldToCell(positionToCheck));
                if ((Vector2)room.transform.position == positionToCheck)
                {
                    if (direction == VecDirections[0])
                    {
                        neighbors.Add(RoomNeighbors.North);
                    }
                    else if (direction == VecDirections[1])
                    {
                        neighbors.Add(RoomNeighbors.South);
                    }
                    else if (direction == VecDirections[2])
                    {
                        neighbors.Add(RoomNeighbors.West);
                    }
                    else if (direction == VecDirections[3])
                    {
                        neighbors.Add(RoomNeighbors.East);
                    }
                }
            }
        }
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
            distance = direction * roomSize;
        }

        return distance;
    }

    public void SetRoomProperties()
    {
        // Add a collider to the room
        GiveTheRoomCollider();
    }

    private void GiveTheRoomCollider()
    {
        // Add a collider to the room
        BoxCollider2D roomCollider = this.gameObject.AddComponent<BoxCollider2D>();
        //get the room size from the DungeonGenerator
        roomCollider.size = new Vector2(FindObjectOfType<DungeonGenerator>().roomsSize.x, FindObjectOfType<DungeonGenerator>().roomsSize.y);
        roomCollider.offset = new Vector2(-0.5f, -0.5f);
    }
}