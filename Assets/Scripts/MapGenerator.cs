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
    [SerializeField] private SO_RoomType merchantRoom;

    [Header("Corridors")] [Space(15)]
    [SerializeField] private Vector2Int smallCorridorWidth = new Vector2Int(CORRIDOR_LENGTH,3);
    [SerializeField] private Vector2Int largeCorridorSize = new Vector2Int(CORRIDOR_LENGTH,5);
    [SerializeField] private Vector2Int bonusRoomCorridorSize = new Vector2Int(3,BONUS_CORRIDOR_LENGTH);
    
    [Header("Bonus room objects")] [Space(15)]
    [SerializeField] private GameObject weaponPickUpPrefab;
    
    private RoomManager roomManager;
    private List<Corridor> corridors = new List<Corridor>();
    
    List<BonusRoom> bonusRooms = new List<BonusRoom>();
    List<BoundsInt> roomsPositions = new List<BoundsInt>();
    List<GameObject> bonusRoomObjects = new List<GameObject>();
    
    private int currentCorridor = 0;
    private int roomIdCounter = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        GenerateMap();
    }
    
    public void ClearAll()
    {
        floorMap.ClearAllTiles();
        wallsMap.ClearAllTiles();
        bonusRooms.Clear();
        roomsPositions.Clear();
        foreach (GameObject bonusRoomObject in bonusRoomObjects)
        {
            DestroyImmediate(bonusRoomObject);
        }
        bonusRoomObjects.Clear();
        currentCorridor = 0;
    }
    
    public void GenerateMap()
    {
        ClearAll();
        corridors.Clear();
        
        List<SO_RoomType> generatedRooms = new List<SO_RoomType>();
        List<SO_BonusRoomType> generatedBonusRooms = new List<SO_BonusRoomType>();
        
        BonusRoomPosition bonusRoomPosition = (BonusRoomPosition) Random.Range(0,2);
        
        SO_RoomType currentRoom = hubRoom;
        
        
        //Spawn HUB, then spawn rooms
        for (int i = 0; i < generatedMaxBaseRooms; i++)
        {
            generatedRooms.Add(currentRoom);
            currentRoom = currentRoom.NextRoom();
        }

        generatedRooms.Add(merchantRoom);
        generatedRooms.Add(endRoom);

        foreach (var room in generatedRooms)
        {
            generatedBonusRooms.Add(room.BonusRoom());
        }
        
        #region Drawing rooms
        int spaceBetweenRooms = 0;  //Space between the center of the last room to the center of the next one
        //loop
        for (int i = 0; i < generatedRooms.Count; i++)
        {
            #region Main Path Rooms
            //stock room size
            Vector2Int size = generatedRooms[i].GetSize();
            
            //stock room center
            BoundsInt roomBounds = new BoundsInt(new Vector3Int(spaceBetweenRooms,0,0),new Vector3Int(size.x,size.y,0)); 
            roomBounds.SetCenter(new Vector3Int(spaceBetweenRooms+size.x/2,0,0));
            roomsPositions.Add(roomBounds);
            spaceBetweenRooms += size.x + CORRIDOR_LENGTH;
            
            roomIdCounter++;
            
            //Drawing rooms
            DrawArea(floorMap,floorTile,roomBounds);
            DrawArea(wallsMap,wallsTile,roomBounds);
            #endregion

            #region Bonus Rooms
            //Check if room has a bonus room
            if (generatedBonusRooms[i])
            {
                //Stock the position of the room (Under or Above main path)
                generatedBonusRooms[i].position = bonusRoomPosition;
                
                //if true, draw the room
                Vector2Int bonusRoomSize = generatedBonusRooms[i].GetSize();
                BoundsInt bonusRoomBounds = new BoundsInt(roomsPositions[i].position,new Vector3Int(bonusRoomSize.x,bonusRoomSize.y,0));;
                
                Vector3Int offset = new Vector3Int(0,bonusRoomSize.y+bonusRoomCorridorSize.y+roomsPositions[i].size.y/2,0);
                
                //Check if the room needs to be drawn under or above the room
                switch (bonusRoomPosition)
                {
                    case BonusRoomPosition.Under:
                        bonusRoomBounds.SetCenter(Vector3Int.FloorToInt(roomsPositions[i].center) - offset);
                        break;
                    case BonusRoomPosition.Above:
                    default:
                        bonusRoomBounds.SetCenter(Vector3Int.FloorToInt(roomsPositions[i].center) + offset);
                        break;
                }
                BonusRoom bonus = new BonusRoom();
                bonus.bounds = bonusRoomBounds;
                bonus.connectedRoomId = i;
                bonus.BonusRoomType = generatedBonusRooms[i];
                
                bonusRooms.Add(bonus);
                
                bonusRoomPosition = (BonusRoomPosition) Random.Range(0,2);
                
                DrawArea(floorMap, floorTile, bonusRoomBounds);
                DrawArea(wallsMap, wallsTile, bonusRoomBounds); 
            }
            #endregion
        }
        #endregion
        
        //Generating corridors
        for (int i = 0; i < roomsPositions.Count-1; i++)
        {
            Corridor corridor = new Corridor();
            switch (generatedRooms[i+1].sizeType.name)
            {
                case RoomSizeName.Large:
                    corridor.width = largeCorridorSize.y;
                    corridor.start = new Vector3Int((int)roomsPositions[i].center.x,(int)roomsPositions[i].center.y) - new Vector3Int(0,Mathf.FloorToInt(largeCorridorSize.y/2),0);
                    corridor.end = new Vector3Int((int)roomsPositions[i+1].center.x,(int)roomsPositions[i+1].center.y,0) - new Vector3Int(0,Mathf.FloorToInt(largeCorridorSize.y/2),0);
                    break;
                case RoomSizeName.Small:
                case RoomSizeName.Medium:
                default:
                    corridor.width = smallCorridorWidth.y;
                    corridor.start = new Vector3Int((int)roomsPositions[i].center.x,(int)roomsPositions[i].center.y) - new Vector3Int(0,Mathf.FloorToInt(smallCorridorWidth.y/2),0);
                    corridor.end = new Vector3Int((int)roomsPositions[i+1].center.x,(int)roomsPositions[i+1].center.y,0) - new Vector3Int(0,Mathf.FloorToInt(smallCorridorWidth.y/2),0);
                    break;
            }
            corridor.roomId = i;
            corridor.type = CorridorType.Horizontal;
            corridors.Add(corridor);
        }

        for (int i = 0; i < bonusRooms.Count; i++)
        {
            
            Corridor corridor = new Corridor();
            corridor.width = bonusRoomCorridorSize.x;
            corridor.start = Vector3Int.FloorToInt(bonusRooms[i].bounds.center) - new Vector3Int(bonusRoomCorridorSize.x / 2, 0, 0);
            corridor.end = Vector3Int.FloorToInt(roomsPositions[i].center) + new Vector3Int(bonusRoomCorridorSize.x / 2, 0, 0);
            corridor.type = CorridorType.Vertical;
            corridor.roomId = bonusRooms[i].connectedRoomId;
            corridor.isBonus = true;
            corridors.Add(corridor);
        }

        corridors.Sort((a, b) => {
            if (a.roomId < b.roomId) return -1;
            if (a.roomId > b.roomId) return 1;
            return 0;
        });

        foreach (BonusRoom bonusRoom in bonusRooms)
        {
            GameObject bonusObject;
            switch (bonusRoom.BonusRoomType.type)
            {
                case RoomType.Chest :
                    bonusObject = Instantiate(weaponPickUpPrefab, bonusRoom.bounds.center-new Vector3(0,0.5f,0), Quaternion.identity);
                    break;
                case RoomType.WeaponUpgrade:
                    default:
                    bonusObject = Instantiate(weaponPickUpPrefab, bonusRoom.bounds.center-new Vector3(0,0.5f,0), Quaternion.identity);
                    break;
            }
            bonusRoomObjects.Add(bonusObject);
        }
        
        DrawNextCorridor();
        
    }

    public void DrawNextCorridor()
    {
        
        if (currentCorridor < corridors.Count)
        {
            Corridor corridor = corridors[currentCorridor];
            DrawCorridor(floorMap,floorTile,corridor.start,corridor.end,corridor.width,corridor.type);
            DrawCorridor(wallsMap,wallsTile,corridor.start,corridor.end,corridor.width,corridor.type);
            currentCorridor++;
            if (corridors[currentCorridor].isBonus)
            {
                DrawNextCorridor();
            }
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
    private void DrawArea(Tilemap map, TileBase tile, BoundsInt bounds)
    {
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                map.SetTile(new Vector3Int(x, y, 0), tile);
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
                int startYVert = Mathf.Min(start.y, end.y);
                int lengthY = Mathf.Abs(end.y - start.y);
                
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < lengthY; y++)
                    {
                        map.SetTile(new Vector3Int(start.x + x, startYVert + y, 0), tile);
                    }
                }
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (BonusRoom bonus in bonusRooms)
        {
            Gizmos.DrawWireCube(bonus.bounds.center, bonus.bounds.size);
        }
        
        Gizmos.color = Color.blue;
        foreach (BoundsInt room in roomsPositions)
        {
            Gizmos.DrawWireCube(room.center,room.size);
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
