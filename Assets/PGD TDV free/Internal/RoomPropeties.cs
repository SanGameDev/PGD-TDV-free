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
            Vector2Int positionToCheck = 
            new Vector2Int(DistanceBetweenRooms(direction.x, roomSize.x, hallsLength), DistanceBetweenRooms(direction.y, roomSize.y, hallsLength));

            foreach (var room in FindObjectOfType<DungeonGenerator>().roomsReferences)
            {
                if (room.transform.position == grid.GetCellCenterWorld(new Vector3Int(positionToCheck.x, positionToCheck.y, 0)))
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
        
    }

    private void GiveTheRoomCollider(Transform roomReference)
    {
        // get the room size
        // get the room position
        // calculate the collider size
        // calculate the collider position
        // instantiate the collider in that position
    }
}

/*
DungeonGenerator dungeonGenerator = FindObjectOfType<DungeonGenerator>();
        Vector2Int thisPosition = new Vector2Int((int)transform.position.x, (int)transform.position.y);

        if (neighbors == null)
        {
            neighbors = new List<RoomNeighbors>();
        }

            Vector2Int north = thisPosition + distancesToNeighbors * new Vector2Int(0, 1);  // Up
            Vector2Int south = thisPosition + distancesToNeighbors * new Vector2Int(0, -1); // Down
            Vector2Int west = thisPosition + distancesToNeighbors * new Vector2Int(-1, 0); // Left
            Vector2Int east = thisPosition + distancesToNeighbors * new Vector2Int(1, 0);   // Right
        
        for (int i = 0; i < 4; i++)
        {
            //Vector2Int neighborPosition = thisPosition + distancesToNeighbors * direction;

            //Debug.Log("Checking neighbors" + neighborPosition);

            //Debug.Log("Rooms" + grid.GetComponentsInChildren<RoomPropeties>().Length);
            foreach (var room in dungeonGenerator.roomsReferences)
            {
                Debug.Log("Checking neighbors" + room.transform.position + " " + grid.GetCellCenterWorld(new Vector3Int(north.x, north.y, 0)));

                if (room.transform.position == grid.GetCellCenterWorld(new Vector3Int(north.x, north.y, 0)))
                {
                    Debug.Log("Neighbor found");
                    neighbors.Add(RoomNeighbors.North);
                }
                else if (room.transform.position == grid.GetCellCenterWorld(new Vector3Int(south.x, south.y, 0)))
                {
                    Debug.Log("Neighbor found");
                    neighbors.Add(RoomNeighbors.South);
                }
                else if (room.transform.position == grid.GetCellCenterWorld(new Vector3Int(west.x, west.y, 0)))
                {
                    Debug.Log("Neighbor found");
                    neighbors.Add(RoomNeighbors.West);
                }
                else if (room.transform.position == grid.GetCellCenterWorld(new Vector3Int(east.x, east.y, 0)))
                {
                    Debug.Log("Neighbor found");
                    neighbors.Add(RoomNeighbors.East);
                }
            }
        }
        */
