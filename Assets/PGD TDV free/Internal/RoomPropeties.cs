using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPropeties : MonoBehaviour
{
    public List<RoomNeighbors> neighbors;
    private bool isPlayerInRoom;

    private void GiveTheRoomCollider()
    {
        // Add a collider to the room
        BoxCollider2D roomCollider = this.gameObject.AddComponent<BoxCollider2D>();
        //get the room size from the DungeonGenerator
        roomCollider.size = new Vector2(FindObjectOfType<DungeonGenerator>().roomsSize.x, FindObjectOfType<DungeonGenerator>().roomsSize.y);
        roomCollider.offset = new Vector2(-0.5f, -0.5f);
    }
}