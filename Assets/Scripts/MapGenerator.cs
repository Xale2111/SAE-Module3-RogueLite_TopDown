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
    private const int CORRIDOR_WIDTH = 9;
    
    [SerializeField] Vector2Int grassSize;
    [SerializeField] private int generatedMaxBaseRooms = 4;
    
    [Header("Grass")] [Space(10)] 
    [SerializeField] private Tilemap grassMap;
    [SerializeField] private TileBase grassTile;

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
    [SerializeField] private Vector2Int smallCorridorSize = new Vector2Int(CORRIDOR_WIDTH,3);
    [SerializeField] private Vector2Int largeCorridorSize = new Vector2Int(CORRIDOR_WIDTH,5);

    
    private RoomManager roomManager;
    private List<Corridor> corridors = new List<Corridor>();
    private int currentCorridor = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        GenerateMap();
    }

    void GenerateGrass()
    {
        for (int x = -20; x < grassSize.x; x++)
        {
            for (int y = -(grassSize.y/2); y < grassSize.y/2; y++)
            {
                grassMap.SetTile(new Vector3Int(x, y, 0), grassTile);
            }
        }
    }

    public void ClearAll()
    {
        grassMap.ClearAllTiles();
        floorMap.ClearAllTiles();
        wallsMap.ClearAllTiles();
    }


    public void GenerateMap()
    {
        ClearAll();
        GenerateGrass();
        corridors.Clear();
        
        List<SO_RoomType> generatedRooms = new List<SO_RoomType>();
        
        SO_RoomType currentRoom = hubRoom;
        
        
        //Spawn HUB, then spawn rooms
        for (int i = 0; i < generatedMaxBaseRooms; i++)
        {
            generatedRooms.Add(currentRoom);
            currentRoom = currentRoom.NextRoom();
        }

        generatedRooms.Add(endRoom);
        
        //Drawing rooms
        
        int lastBorderXPosition = 0;
        
        for (int i = 0; i < generatedRooms.Count; i++)
        {
            SO_RoomType room = generatedRooms[i];
            Vector2Int size = room.GetSize();
            
            Vector2Int roomCenter = new Vector2Int(lastBorderXPosition+size.x/2,0);
            
            Debug.Log("Room order: " + room.name);
            //Drawing rooms
            DrawArea(floorMap,floorTile,roomCenter,size);
            DrawArea(wallsMap,wallsTile,roomCenter,size);
            
            lastBorderXPosition += size.x;
            if (i + 1 < generatedRooms.Count)
            {
                Corridor corridor = new Corridor();
                
                switch (generatedRooms[i+1].sizeType.name)
                {
                    case RoomSizeName.Small:
                    case RoomSizeName.Medium:
                        corridor.size = CorridorSize.Small;
                        corridor.start = new Vector2Int(lastBorderXPosition,0+smallCorridorSize.y/2);
                        break;
                    
                    case RoomSizeName.Large:
                        corridor.size = CorridorSize.Large;
                        corridor.start = new Vector2Int(lastBorderXPosition,0+largeCorridorSize.y/2);
                        break;
                }
                corridor.type = CorridorType.Horizontal;
    
                corridors.Add(corridor);
            }

            lastBorderXPosition += CORRIDOR_WIDTH;
        }
        
        DrawNextCorridor();
    }

    public void DrawNextCorridor()
    {
        Corridor corridor = corridors[currentCorridor];
        switch (corridor.size)
        {
            case CorridorSize.Small:
                DrawCorridor(floorMap, floorTile, corridor.start, smallCorridorSize);
                DrawCorridor(wallsMap, wallsTile, corridor.start, smallCorridorSize);
                break;
            case CorridorSize.Large:
                DrawCorridor(floorMap, floorTile, corridor.start, largeCorridorSize);
                DrawCorridor(wallsMap, wallsTile, corridor.start, largeCorridorSize);
                break;
        }

        currentCorridor++;
    }

    public void DrawNextCorridor(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            Corridor corridor = corridors[currentCorridor];
            switch (corridor.size)
            {
                case CorridorSize.Small:
                    DrawCorridor(floorMap, floorTile, corridor.start, smallCorridorSize);
                    DrawCorridor(wallsMap, wallsTile, corridor.start, smallCorridorSize);
                    break;
                case CorridorSize.Large:
                    DrawCorridor(floorMap, floorTile, corridor.start, largeCorridorSize);
                    DrawCorridor(wallsMap, wallsTile, corridor.start, largeCorridorSize);
                    break;
            }

            currentCorridor++;
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
    
    private void DrawCorridor(Tilemap map, TileBase tile, Vector2Int start, Vector2Int size)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                map.SetTile(new Vector3Int(start.x + x, start.y - y, 0), tile);
            }
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
