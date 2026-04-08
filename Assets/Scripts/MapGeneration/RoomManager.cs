using System.Collections.Generic;
using Rooms;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    List<BoundsInt> _rooms = new List<BoundsInt>();

    public BoundsInt CurrentRoomBounds()
    {
        return _rooms[0];
    }
}
