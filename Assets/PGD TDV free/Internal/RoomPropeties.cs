using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPropeties : MonoBehaviour
{
    public List<RoomNeighbors> neighbors;
    public List<RoomNeighbors> connectedRooms;
    public bool isConnectedToMainRoom;
    private bool isPlayerInRoom;
}