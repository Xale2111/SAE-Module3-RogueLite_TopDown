using System;
using System.Collections.Generic;
using System.Linq;
using Rooms;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;


public class MapGenerator : MonoBehaviour
{
    private const int CORRIDOR_LENGTH = 9;
    private const int BONUS_CORRIDOR_LENGTH = 7;
    
    [SerializeField] Vector2Int grassSize;
    [SerializeField] private int generatedMaxBaseRooms = 4;
    
    [Header("Floor")] [Space(10)] 
    [SerializeField] private Tilemap floorMap;
    [SerializeField] private TileBase floorTile;

    [Header("Walls")] [Space(10)] 
    [SerializeField] private Tilemap wallsMap;
    [SerializeField] private TileBase wallsTile;
    
    [Header("Rooms")] [Space(15)] 
    [SerializeField] private SO_RoomType hubRoom;
    [SerializeField] private SO_RoomType endRoom;

    [Header("Corridors")] [Space(15)]
    [SerializeField] private Vector2Int smallCorridorWidth = new Vector2Int(CORRIDOR_LENGTH,3);
    [SerializeField] private Vector2Int largeCorridorSize = new Vector2Int(CORRIDOR_LENGTH,5);
    [SerializeField] private Vector2Int bonusRoomCorridorSize = new Vector2Int(3,BONUS_CORRIDOR_LENGTH);
    
    private RoomManager roomManager;
    private List<Corridor> corridors = new List<Corridor>();
    
    private int currentCorridor = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        GenerateMap();
    }
    
    public void ClearAll()
    {
        floorMap.ClearAllTiles();
        wallsMap.ClearAllTiles();
        currentCorridor = 0;
    }


    public void GenerateMap()
    {
        ClearAll();
        corridors.Clear();
        
        List<SO_RoomType> generatedRooms = new List<SO_RoomType>();
        List<BoundsInt> roomsPositions = new List<BoundsInt>();
        List<SO_BonusRoomType> generatedBonusRooms = new List<SO_BonusRoomType>();
        
        BonusRoomPosition bonusRoomPosition = (BonusRoomPosition) Random.Range(0,2);
        
        SO_RoomType currentRoom = hubRoom;
        
        
        //Spawn HUB, then spawn rooms
        for (int i = 0; i < generatedMaxBaseRooms; i++)
        {
            generatedRooms.Add(currentRoom);
            currentRoom = currentRoom.NextRoom();
        }

        generatedRooms.Add(endRoom);

        foreach (var room in generatedRooms)
        {
            generatedBonusRooms.Add(room.BonusRoom());
        }
        
        //Drawing rooms
        int spaceBetweenRooms = 0;  //Space between the center of the last room to the left border of the new one
        //loop
        for (int i = 0; i < generatedRooms.Count; i++)
        {
            //stock room size
            Vector2Int size = generatedRooms[i].GetSize();
            //stock room center
            Vector2Int roomCenter = new Vector2Int(spaceBetweenRooms+size.x/2,0);
            BoundsInt roomBounds = new BoundsInt(new Vector3Int(roomCenter.x,roomCenter.y,0),new Vector3Int(size.x,size.y,0)); 
            roomsPositions.Add(roomBounds);;
            spaceBetweenRooms += size.x + CORRIDOR_LENGTH;
            
            //Drawing rooms
            DrawArea(floorMap,floorTile,roomCenter,size);
            DrawArea(wallsMap,wallsTile,roomCenter,size);
            
            //Check if room has a bonus room
            if (generatedBonusRooms[i])
            {
                //if true, draw the room
                Vector2Int bonusRoomSize = generatedBonusRooms[i].GetSize();
                Vector2Int bonusRoomCenter;

                //Check if the room needs to be drawn under or above the room
                switch (bonusRoomPosition)
                {
                    case BonusRoomPosition.Under:
                        bonusRoomCenter = roomCenter - new Vector2Int(0,size.y/2+BONUS_CORRIDOR_LENGTH+bonusRoomSize.y/2);
                        break;
                    case BonusRoomPosition.Above:
                    default:
                        bonusRoomCenter = roomCenter + new Vector2Int(0,size.y/2+BONUS_CORRIDOR_LENGTH+bonusRoomSize.y/2);
                        break;
                }
                generatedBonusRooms[i].position = bonusRoomPosition;
                generatedBonusRooms[i].center = bonusRoomCenter;
                bonusRoomPosition = (BonusRoomPosition) Random.Range(0,2);
                
                DrawArea(floorMap, floorTile, bonusRoomCenter, bonusRoomSize);
                DrawArea(wallsMap, wallsTile, bonusRoomCenter, bonusRoomSize); 
            }
            
        }

        for (int i = 0; i < generatedRooms.Count - 1; i++)
        {
            Debug.Log($"Drawing Room. Center: {generatedRooms[i].center}");
        }

        for (int i = 0; i < roomsPositions.Count-1; i++)
        {
            Corridor corridor = new Corridor();
            switch (generatedRooms[i+1].sizeType.name)
            {
                case RoomSizeName.Large:
                    corridor.width = largeCorridorSize.y;
                    
                    corridor.start = Vector3Int.RoundToInt(roomsPositions[i].x) - new Vector3Int(0,largeCorridorSize.y,0);
                    corridor.end = Vector3Int.RoundToInt(roomsPositions[i+1].center) - new Vector3Int(0,largeCorridorSize.y,0);
                    break;
                case RoomSizeName.Small:
                case RoomSizeName.Medium:
                default:
                    corridor.width = smallCorridorWidth.y;
                    //Debug.Log($"Drawing Corridor. Start Center: {generatedRooms[i].center} / End Center: {generatedRooms[i+1].center}");
                    corridor.start = Vector3Int.RoundToInt(roomsPositions[i].center) - new Vector3Int(0,smallCorridorWidth.y,0);
                    corridor.end = Vector3Int.RoundToInt(roomsPositions[i+1].center) - new Vector3Int(0,smallCorridorWidth.y,0);
                    break;
            }
            corridor.type = CorridorType.Horizontal;
            corridors.Add(corridor);
        }
        
        foreach (var corridor in corridors)
        {
            DrawNextCorridor();
        }
    }

    public void DrawNextCorridor()
    {
        if (currentCorridor < corridors.Count)
        {
            Corridor corridor = corridors[currentCorridor];
            DrawCorridor(floorMap,floorTile,corridor.start,corridor.end,corridor.width,corridor.type);
            DrawCorridor(wallsMap,wallsTile,corridor.start,corridor.end,corridor.width,corridor.type);
            currentCorridor++;
        }
    }

    public void DrawNextCorridor(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
           DrawNextCorridor();
        }
    }

    /// <summary>
    /// Draw the rooms
    /// </summary>
    /// <param name="map">tilemap to draw on</param>
    /// <param name="tile">tile to draw</param>
    /// <param name="corner">Top left corner of the room, that's where the drawing of the room will begin</param>
    /// <param name="size">size of the room</param>
    private void DrawArea(Tilemap map, TileBase tile, Vector2Int center, Vector2Int size)
    {
        Vector2Int corner = center - (size / 2);
        
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                map.SetTile(new Vector3Int(corner.x + x, corner.y + y, 0), tile);
            }
        }
    }

    private void DrawCorridor(Tilemap map, TileBase tile, Vector3Int start, Vector3Int end, int width,
        CorridorType type)
    {
        int length = 0;
        switch (type)
        {
            case CorridorType.Horizontal:
                length = end.x - start.x;
                for (int x = 0; x < length; x++)
                {
                    for (int y = 0; y < width; y++)
                    {
                        map.SetTile(new Vector3Int(start.x + x, start.y + y, 0), tile);
                    }
                }

                break;
            case CorridorType.Vertical:
                length = end.y - start.y;
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < length; y++)
                    {
                        map.SetTile(new Vector3Int(start.x+x, start.y + y, 0), tile);
                    }
                }

                break;
        }
    }


    /*
    private void DrawMap(Tilemap map, TileBase tile, List<Vector2Int> generatedPositions)
    {
        map.ClearAllTiles();

        foreach (Vector2Int position in generatedPositions)
        {
            map.SetTile(new Vector3Int(position.x, position.y, 0), tile);
        }
    }

    private void DrawMap(Tilemap map, TileBase tile, BoundsInt positions)
    {
        foreach (Vector3Int position in positions.allPositionsWithin)
        {
            map.SetTile(position, tile);
        }
    }*/
    
}
