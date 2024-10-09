using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PGD_TDV;

public class RoomPropeties : MonoBehaviour
{
    public List<RoomNeighbors> neighbors;
    public List<RoomNeighbors> connectedRooms;
    public bool isConnectedToMainRoom;

    public RoomType roomType;

    //private List<Vector3Int> doorTiles = new List<Vector3Int>();
    private bool isPlayerInRoom;

    private void Start() 
    {
        switch(roomType) // TODO checar la logica que agregara los eventos y funciones
        {
            case RoomType.start:
                //Do nothing
                break;
            case RoomType.empty:

                break;
            case RoomType.fight:

                break;
            case RoomType.chest:

                break;
            case RoomType.boss:

                break;
        }
    }
}