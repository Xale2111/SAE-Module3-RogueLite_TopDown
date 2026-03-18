using UnityEngine;

namespace Rooms
{
    public enum RoomSizeName
    {
        Small,Medium,Large
    }

    [CreateAssetMenu(fileName = "RoomSize", menuName = "Rooms/RoomSize", order = 1)]
    public class SO_RoomSizeType : ScriptableObject
    {
        [SerializeField] private Vector2Int minSize; 
        [SerializeField] private Vector2Int maxSize;
        public RoomSizeName name;

        public Vector2Int GetSize()
        {
            Vector2Int size = new Vector2Int(Random.Range(minSize.x, maxSize.x), Random.Range(minSize.y, maxSize.y));
            if (size.x % 2 == 0)
            {
                size.x--;
            }
            if (size.y % 2 == 0)
            {
                size.y--;
            }

            return size;
        }
    }
}