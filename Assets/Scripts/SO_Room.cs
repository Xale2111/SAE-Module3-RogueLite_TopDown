using UnityEngine;

namespace Rooms
{
    [CreateAssetMenu(fileName = "Room", menuName = "Rooms/Room", order = 0)]
    public class SO_Room : ScriptableObject
    {
        public RoomType type;
        public SO_RoomSizeType sizeType;   
        public string name;
        
        public Vector2Int center;
        
        public Vector2Int GetSize()
        {
            return sizeType.GetSize();
        }

    }
}