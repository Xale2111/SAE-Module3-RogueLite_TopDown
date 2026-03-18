using UnityEngine;

namespace Rooms
{
    [System.Serializable]
    public class RoomInstance
    {
        public SO_RoomType roomType;
        public Vector2Int center;
        public Vector2Int size;

        public RoomInstance(SO_RoomType type)
        {
            roomType = type;
            size = type.GetSize();
        }
    }
}
