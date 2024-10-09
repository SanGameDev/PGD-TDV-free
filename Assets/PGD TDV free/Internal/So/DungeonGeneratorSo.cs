using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using PGD_TDV;

[CreateAssetMenu(fileName = "DungeonGenerator", menuName = "PGD TDV/DungeonGenerator")]
public class DungeonGeneratorSo : ScriptableObject
{
    [Range(1, 500)]public int numberOfRooms;

    [Header("Rooms")]
    public Vector2Int initialPosition;
    public Vector2Int roomsSize;


    [Header("Halls & Doors")]
    public int hallsLength;
    public int hallsWidth;
    public int doorsWidth;
    [Range(0, 100)] public int roomConnectionChance;


    [Header("Tiles")]
    public Tile wallTile;
    public Tile floorTile;
    public Tile doorTile;

    private Grid grid;
    private Tilemap wallsTileMap;
    private Tilemap floorsTileMap;
    private Tilemap doorsMap;
}
