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
    [SerializeField] private GameObject chestPickUpPrefab; 
        
    private List<Corridor> corridors = new List<Corridor>();
    
    List<BonusRoom> bonusRooms = new List<BonusRoom>();
    List<NormalRoom> normalRooms = new List<NormalRoom>();
    List<GameObject> bonusRoomObjects = new List<GameObject>();
    
    private int currentCorridor = 0;

    public int GetMaxRoom => generatedMaxBaseRooms;

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
        normalRooms.Clear();        

        foreach (GameObject bonusRoomObject in bonusRoomObjects)
        {
            DestroyImmediate(bonusRoomObject.gameObject);
        }
        bonusRoomObjects.Clear();
        currentCorridor = 0;
    }
    
    public void GenerateMap()
    {
        ClearAll();
        corridors.Clear();
        
        List<SO_RoomType> generatedRoomsType = new List<SO_RoomType>();
        List<SO_BonusRoomType> generatedBonusRoomsType = new List<SO_BonusRoomType>();
        
        BonusRoomPosition bonusRoomPosition = (BonusRoomPosition) Random.Range(0,2);
        
        SO_RoomType currentRoomType = hubRoom;
        
        
        //Spawn HUB, then spawn rooms
        for (int i = 0; i < generatedMaxBaseRooms; i++)
        {
            generatedRoomsType.Add(currentRoomType);
            currentRoomType = currentRoomType.NextRoom();
        }

        generatedRoomsType.Add(merchantRoom);
        generatedRoomsType.Add(endRoom);

        foreach (var room in generatedRoomsType)
        {
            generatedBonusRoomsType.Add(room.BonusRoom());
        }
        
        #region Drawing rooms
        int spaceBetweenRooms = 0;  //Space between the center of the last room to the center of the next one
        //loop
        for (int i = 0; i < generatedRoomsType.Count; i++)
        {
            #region Main Path Rooms
            //stock room size
            Vector2Int size = generatedRoomsType[i].GetSize();
            
            //stock room center
            BoundsInt roomBounds = new BoundsInt(new Vector3Int(spaceBetweenRooms,0,0),new Vector3Int(size.x,size.y,0)); 
            roomBounds.SetCenter(new Vector3Int(spaceBetweenRooms+size.x/2,0,0));
            NormalRoom newNormalRoom;
            newNormalRoom.bounds = roomBounds;
            newNormalRoom.RoomType = generatedRoomsType[i];
            normalRooms.Add(newNormalRoom);
            spaceBetweenRooms += size.x + CORRIDOR_LENGTH;                      
            
            //Drawing rooms
            DrawArea(floorMap,floorTile,roomBounds);
            DrawArea(wallsMap,wallsTile,roomBounds);
            #endregion

            #region Bonus Rooms
            //Check if room has a bonus room
            if (generatedBonusRoomsType[i])
            {
                //Stock the position of the room (Under or Above main path)
                generatedBonusRoomsType[i].position = bonusRoomPosition;
                
                //if true, draw the room
                Vector2Int bonusRoomSize = generatedBonusRoomsType[i].GetSize();
                BoundsInt bonusRoomBounds = new BoundsInt(normalRooms[i].bounds.position,new Vector3Int(bonusRoomSize.x,bonusRoomSize.y,0));;
                
                Vector3Int offset = new Vector3Int(0,bonusRoomSize.y+bonusRoomCorridorSize.y+normalRooms[i].bounds.size.y/2,0);
                
                //Check if the room needs to be drawn under or above the room
                switch (bonusRoomPosition)
                {
                    case BonusRoomPosition.Under:
                        bonusRoomBounds.SetCenter(Vector3Int.FloorToInt(normalRooms[i].bounds.center) - offset);
                        break;
                    case BonusRoomPosition.Above:
                    default:
                        bonusRoomBounds.SetCenter(Vector3Int.FloorToInt(normalRooms[i].bounds.center) + offset);
                        break;
                }
                BonusRoom bonus = new BonusRoom();
                bonus.bounds = bonusRoomBounds;
                bonus.connectedRoomId = i;
                bonus.BonusRoomType = generatedBonusRoomsType[i];
                
                bonusRooms.Add(bonus);
                
                bonusRoomPosition = (BonusRoomPosition) Random.Range(0,2);
                
                DrawArea(floorMap, floorTile, bonusRoomBounds);
                DrawArea(wallsMap, wallsTile, bonusRoomBounds); 
            }
            #endregion
        }


        #endregion
        
        RoomManager.roomsBounds = normalRooms;

        //Generating corridors
        for (int i = 0; i < normalRooms.Count-1; i++)
        {
            Corridor corridor = new Corridor();
            switch (generatedRoomsType[i+1].sizeType.name)
            {
                case RoomSizeName.Large:
                    corridor.width = largeCorridorSize.y;
                    corridor.start = new Vector3Int((int)normalRooms[i].bounds.xMax, (int)normalRooms[i].bounds.center.y) - new Vector3Int(0,Mathf.FloorToInt(largeCorridorSize.y/2),0);
                    corridor.end = new Vector3Int((int)normalRooms[i+1].bounds.xMin,(int)normalRooms[i+1].bounds.center.y,0) - new Vector3Int(0,Mathf.FloorToInt(largeCorridorSize.y/2),0);
                    break;
                case RoomSizeName.Small:
                case RoomSizeName.Medium:
                default:
                    corridor.width = smallCorridorWidth.y;
                    corridor.start = new Vector3Int((int)normalRooms[i].bounds.xMax,(int)normalRooms[i].bounds.center.y) - new Vector3Int(0,Mathf.FloorToInt(smallCorridorWidth.y/2),0);
                    corridor.end = new Vector3Int((int)normalRooms[i+1].bounds.xMin, (int)normalRooms[i+1].bounds.center.y,0) - new Vector3Int(0,Mathf.FloorToInt(smallCorridorWidth.y/2),0);
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
            corridor.end = Vector3Int.FloorToInt(normalRooms[i].bounds.center) + new Vector3Int(bonusRoomCorridorSize.x / 2, 0, 0);
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
                    bonusObject = Instantiate(chestPickUpPrefab, bonusRoom.bounds.center-new Vector3(0,0.5f,0), Quaternion.identity);
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
            if (currentCorridor < corridors.Count - 1 && corridors[currentCorridor].isBonus)
            {
                DrawNextCorridor();
            }
        }
    }

    public void DeleteLastCorridor()
    {
        if (currentCorridor <= corridors.Count)
        {
            //If bonus corridor, delete 2 before 
            Corridor corridor = corridors[currentCorridor-1].isBonus ? corridors[currentCorridor - 2] : corridors[currentCorridor - 1];

            DeleteCorridor(floorMap, corridor.start, corridor.end, corridor.width, corridor.type);
            DeleteCorridor(wallsMap, corridor.start, corridor.end, corridor.width, corridor.type);
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

    private void DeleteCorridor(Tilemap map, Vector3Int start, Vector3Int end, int width,
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
                        map.SetTile(new Vector3Int(start.x + x, start.y + y, 0), null);
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
                        map.SetTile(new Vector3Int(start.x + x, startYVert + y, 0), null);
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
        foreach (NormalRoom room in normalRooms)
        {
            Gizmos.DrawWireCube(room.bounds.center,room.bounds.size);
        }
    }

    private void Update()
    {
        
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
