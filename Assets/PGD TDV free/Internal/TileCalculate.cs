using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGD_TDV
{
    public static class TileCalculate
    {
        public static Vector2Int CalculateRoomCenter(Vector2Int roomSize, Vector2Int roomPosition)
        {
            return new Vector2Int(roomPosition.x - roomSize.x / 2, roomPosition.y - roomSize.y / 2);
        }

        public static bool IsWallTIle(Vector2Int position, Vector2Int roomCenter, Vector2Int roomSize)
        {
            if(position.x == roomCenter.x || position.x == roomCenter.x + roomSize.x - 1 || position.y == roomCenter.y || position.y == roomCenter.y + roomSize.y - 1)
            {
                return true;
            }

            return false;
        }

        public static List<Vector2Int> DoorTilePosition(Vector2Int roomCenter, Vector2Int roomsSize, int hallsWidth, RoomNeighbors direction)
        {
            List<Vector2Int> doorsTilePosition = new List<Vector2Int>();

            for(int j = 0; j < hallsWidth; j++)
            {
                if(direction == RoomNeighbors.North)
                {
                    doorsTilePosition.Add(new Vector2Int(roomCenter.x + roomsSize.x / 2 - hallsWidth / 2 + j, roomCenter.y + roomsSize.y - 1));
                }
                else if(direction == RoomNeighbors.South)
                {
                    doorsTilePosition.Add(new Vector2Int(roomCenter.x + roomsSize.x / 2 - hallsWidth / 2 + j, roomCenter.y));
                }
                else if(direction == RoomNeighbors.East)
                {
                    doorsTilePosition.Add(new Vector2Int(roomCenter.x + roomsSize.x - 1, roomCenter.y + roomsSize.y / 2 - hallsWidth / 2 + j));
                }
                else if(direction == RoomNeighbors.West)
                {
                    doorsTilePosition.Add(new Vector2Int(roomCenter.x, roomCenter.y + roomsSize.y / 2 - hallsWidth / 2 + j));
                }
            }

            return doorsTilePosition;
        }

        public static List<Vector2Int> HallTilePosition(Vector2Int roomCenter, Vector2Int roomsSize, int hallsWidth, RoomNeighbors direction, int hallsLength)
        {
            List<Vector2Int> hallsTilePosition = new List<Vector2Int>();

            for(int i = 0; i < hallsLength; i++)
            {
                for(int j = 0; j < hallsWidth; j++)
                {
                    if(direction == RoomNeighbors.North)
                    {
                        hallsTilePosition.Add(new Vector2Int(roomCenter.x + roomsSize.x / 2 - hallsWidth / 2 + j, roomCenter.y + roomsSize.y + i));
                    }
                    else if(direction == RoomNeighbors.South)
                    {
                        hallsTilePosition.Add(new Vector2Int(roomCenter.x + roomsSize.x / 2 - hallsWidth / 2 + j, roomCenter.y - i - 1));
                    }
                    else if(direction == RoomNeighbors.East)
                    {
                        hallsTilePosition.Add(new Vector2Int(roomCenter.x + roomsSize.x + i, roomCenter.y + roomsSize.y / 2 - hallsWidth / 2 + j));
                    }
                    else if(direction == RoomNeighbors.West)
                    {
                        hallsTilePosition.Add(new Vector2Int(roomCenter.x - i - 1, roomCenter.y + roomsSize.y / 2 - hallsWidth / 2 + j));
                    }
                }
            }

            return hallsTilePosition;
        }

        public static List<Vector2Int> HallsWallsTilePosition(Vector2Int roomCenter, Vector2Int roomsSize, int hallsWidth, RoomNeighbors direction, int hallsLength)
        {
            List<Vector2Int> hallsWallsTilePosition = new List<Vector2Int>();
            bool isNonPair = hallsWidth % 2 != 0;

            for (int i = 0; i < hallsLength; i++)
            {
                for (int j = 0; j < hallsWidth; j++)
                {
                    if (direction == RoomNeighbors.North || direction == RoomNeighbors.South)
                    {
                        Vector2Int leftWallPosition = new Vector2Int(roomCenter.x + roomsSize.x / 2 - hallsWidth / 2 - 1, roomCenter.y + roomsSize.y + i);
                        Vector2Int rightWallPosition = new Vector2Int(roomCenter.x + roomsSize.x / 2 + hallsWidth / 2, roomCenter.y + roomsSize.y + i);
                        
                        if(isNonPair)
                        {
                            rightWallPosition.x += 1;
                        }

                        if (direction == RoomNeighbors.South)
                        {
                            leftWallPosition.y = roomCenter.y - i - 1;
                            rightWallPosition.y = roomCenter.y - i - 1;
                        }

                        hallsWallsTilePosition.Add(leftWallPosition);
                        hallsWallsTilePosition.Add(rightWallPosition);
                    }
                    else if (direction == RoomNeighbors.East || direction == RoomNeighbors.West)
                    {
                        Vector2Int topWallPosition = new Vector2Int(roomCenter.x + roomsSize.x + i, roomCenter.y + roomsSize.y / 2 + hallsWidth / 2);
                        Vector2Int bottomWallPosition = new Vector2Int(roomCenter.x + roomsSize.x + i, roomCenter.y + roomsSize.y / 2 - hallsWidth / 2 - 1);

                        if (isNonPair)
                        {
                            topWallPosition.y += 1;
                        }

                        if (direction == RoomNeighbors.West)
                        {
                            topWallPosition.x = roomCenter.x - i - 1;
                            bottomWallPosition.x = roomCenter.x - i - 1;
                        }

                        hallsWallsTilePosition.Add(topWallPosition);
                        hallsWallsTilePosition.Add(bottomWallPosition);
                    }
                }
            }

            return hallsWallsTilePosition;
        }
    }
}