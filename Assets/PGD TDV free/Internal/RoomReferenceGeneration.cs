using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomReferenceGeneration : MonoBehaviour
{
    public void RoomsGeneration()
    {
        //? generates first room reference in initial position and add it to the list of rooms
        Grid grid = GetComponent<DungeonGenerator>().grid;

        GameObject roomRef = new GameObject("RoomReference");

        roomRef.transform.position = grid.GetCellCenterWorld(new Vector3Int
        (
            (int)GetComponent<DungeonGenerator>().initialPosition.x,
            (int)GetComponent<DungeonGenerator>().initialPosition.y,
            0
        ));
        
        roomRef.transform.SetParent(transform);

        roomRef.AddComponent<RoomPropeties>();

        GetComponent<DungeonGenerator>().roomsReferences.Add(roomRef.transform);

        LoopForRoomsReferences();
    }

    private void LoopForRoomsReferences() //The loop for generating the room references in the space
    {
        int numberOfRooms = GetComponent<DungeonGenerator>().numberOfRooms;
        Vector2 roomSize = GetComponent<DungeonGenerator>().roomsSize;
        int hallsLength = GetComponent<DungeonGenerator>().hallsLength;
        Vector2 newPosition = Vector2.zero;
        Transform roomReference;
        Vector2 direction;

        while(GetComponent<DungeonGenerator>().roomsReferences.Count < numberOfRooms)
        {
            roomReference = GetComponent<DungeonGenerator>().roomsReferences[UnityEngine.Random.Range(0, GetComponent<DungeonGenerator>().roomsReferences.Count)];
            direction = RandomDirection();
            
            if(GetComponent<DungeonGenerator>().hasHalls)
            {
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

                roomRef.AddComponent<RoomPropeties>();
                GetComponent<DungeonGenerator>().roomsReferences.Add(roomRef.transform);
            }
        };

        GetComponent<DungeonGenerator>().GetRoomsNeighbors();
        
    }

    #region RoomReferenceGenerationMethods

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
        return false;
    }

    #endregion
}
