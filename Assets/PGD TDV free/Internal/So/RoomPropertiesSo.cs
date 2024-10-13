using UnityEngine;
using PGD_TDV;

[CreateAssetMenu(fileName = "RoomProperties", menuName = "PGD TDV/RoomProperties")]
public class RoomPropertiesSo : ScriptableObject
{
    public int fightRoomSpawnRate;
    public int chestRoomSpawnRate;
    public int emptyRoomSpawnRate;

    public bool fightRoomCanCloseDoors;
    public bool collidersForCameraBounds;
}
