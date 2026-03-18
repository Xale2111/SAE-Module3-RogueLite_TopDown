using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rooms
{
    public enum RoomType
    {
        Default,
        GoldMaxxing,
        Merchant,
        StatUpgrade
    }
     
    [System.Serializable]
    public struct Link{
        public float Weight;
        public SO_RoomType soRoom;
    }

    [CreateAssetMenu(fileName = "RoomType", menuName = "Rooms/RoomType", order = 0)]
    public class SO_RoomType : ScriptableObject
    {
        public RoomType type;
        public SO_RoomSizeType sizeType;   
        public string name;
        
        public List<Link> Links;
        public List<Link> BonusRoomLinks;
        
        public SO_RoomType NextRoom()
        {
            if (Links.Count > 0)
            {
                float random = Random.value * Links.Sum(l => l.Weight);

                float rngSum = 0;
                foreach (Link link in Links)
                {
                
                    if (random <= rngSum + link.Weight) return link.soRoom;
                    rngSum += link.Weight;
                }
            }

            return null;
        }

        public SO_RoomType BonusRoom()
        {
            return null;
        }

        public Vector2Int GetSize()
        {
            return sizeType.GetSize();
        }
        
    }
    
    
}