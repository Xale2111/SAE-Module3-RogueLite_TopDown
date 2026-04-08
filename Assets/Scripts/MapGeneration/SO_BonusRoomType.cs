using UnityEngine;

namespace Rooms
{
    public struct BonusRoom
    {
        public BoundsInt bounds;
        public int connectedRoomId;
        public SO_BonusRoomType BonusRoomType;
    }
    
    public enum BonusRoomPosition
    {
        Under,
        Above
    }
    
    [CreateAssetMenu(fileName = "BonusRoom", menuName = "Rooms/BonusRoomType", order = 0)]
    public class SO_BonusRoomType : SO_Room
    {
        public BonusRoomPosition position;
    }
}