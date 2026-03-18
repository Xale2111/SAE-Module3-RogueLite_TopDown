using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rooms
{
    public enum RoomType
    {
        Default,
        Chest,
        Merchant,
        WeaponUpgrade
    }
     
    [System.Serializable]
    public struct Link{
        public float Weight;
        public SO_RoomType soRoom;
    }
    
    [System.Serializable]
    public struct BonusLink{
        public float Weight;
        public SO_BonusRoomType soRoom;
    }

    [CreateAssetMenu(fileName = "RoomType", menuName = "Rooms/RoomType", order = 0)]
    public class SO_RoomType : SO_Room
    {
        public List<Link> Links;
        public List<BonusLink> BonusRoomLinks;
        
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

        public SO_BonusRoomType BonusRoom()
        {
            //Pick a random number between 0 and 10
            //if number <= bonus room weight 1 -> bonus room 1
            //else if number <= bonus room weight 1+2 -> bonus room 2
            // else -> no bonus room

            if (BonusRoomLinks.Count > 0)
            {
                
                float random = Random.Range(0, 10);

                int i = 0;
                
                float cumultativeWeight = 0;
                foreach (var bonusRoomLink in BonusRoomLinks)
                {
                    if (random  < bonusRoomLink.Weight + cumultativeWeight)
                    {
                        return bonusRoomLink.soRoom;
                        i++;
                    }
                    cumultativeWeight += bonusRoomLink.Weight;
                }
            }
            
            return null;
        }

       
    }
    
    
}