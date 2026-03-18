using UnityEngine;

namespace Rooms
{
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