using Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private float roomThreshold = .5f;

    public static List<NormalRoom> roomsBounds;
    static int currentRoom = 0;
    static int nextRoom = 1;

    private void Start()
    {
        if (playerTransform == null)
        {
            playerTransform = FindFirstObjectByType<PlayerController>().transform;
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
            //Spawn enemies
        }
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

    private void OnDrawGizmos()
    {        
        Gizmos.color = Color.darkSeaGreen;
        if(nextRoom < roomsBounds.Count)
            Gizmos.DrawLine(new Vector3(GetNextRoomBounds().position.x + roomThreshold, -100), new Vector3(GetNextRoomBounds().position.x + roomThreshold, 100));       
    }



}


