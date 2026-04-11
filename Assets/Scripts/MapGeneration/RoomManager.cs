using System.Collections.Generic;
using Rooms;
using UnityEngine;

public static class RoomManager
{
    public static List<NormalRoom> roomsBounds;
    static int currentRoom = 1;

    public static BoundsInt GetCurrentRoomBounds()
    {
        return roomsBounds[currentRoom].bounds;
    }

    public static void EnteredNewRoom()
    {
        currentRoom++;
    }
}
