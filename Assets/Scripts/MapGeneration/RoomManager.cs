using Rooms;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private float roomThreshold = .5f;


    private Transform playerTransform;
    public static List<NormalRoom> roomsBounds;
    static int currentRoom = 0;
    static int nextRoom = 1;

    private bool inFight = false;

    private void Start()
    {
        if (playerTransform == null || playerController == null)
        {
            playerController = FindFirstObjectByType<PlayerController>();
            playerTransform = playerController.transform;
        }

        if (mapGenerator == null)
        {
            mapGenerator = FindFirstObjectByType<MapGenerator>();
        }
    }

    private void Update()
    {
        if (currentRoom <= mapGenerator.GetMaxRoom && playerTransform.position.x > GetNextRoomBounds().position.x+roomThreshold)
        {
            Debug.Log("Player in next room");
            //Delete last corridor
            mapGenerator.DeleteLastCorridor();           
            //Set New current room
            EnteredNewRoom();
            playerController.HealPlayerOfPercentage(5);
            //Spawn enemies
            enemyManager.SpawnEnemies(GetWeightForTheRoom());
            inFight = true;
            //EnemyManager -> Spawn enemies(GetWeight)            
        }

        if (inFight && enemyManager.AreAllEnemiesDead())
        {
            mapGenerator.DrawNextCorridor();
            inFight = false;
            if (currentRoom == roomsBounds.Count-1)
            {
                NewFloor();
            }
        }
        
    }

    private float GetWeightForTheRoom()
    {
        return Random.Range(roomsBounds[currentRoom].RoomType.MinEnemyWeight, roomsBounds[currentRoom].RoomType.MaxEnemyWeight);
    }

    public static BoundsInt GetCurrentRoomBounds()
    {
        return roomsBounds[currentRoom].bounds;
    }
    public static BoundsInt GetNextRoomBounds()
    {
        return roomsBounds[nextRoom].bounds;  
    }    

    public static void EnteredNewRoom()
    {        
        currentRoom++;               
        nextRoom++;
    }

    private void NewFloor()
    {
        playerController.SendPlayerToStartPosition();
        mapGenerator.GoToNewDungeon();
        playerController.HealPlayerOfPercentage(30);
        playerController.UpgradeMaxHP(25);
        currentRoom = 0;
        nextRoom = 1;
    }

}


