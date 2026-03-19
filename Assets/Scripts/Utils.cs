using UnityEngine;

namespace Rooms
{
    public static class Utils
    {
        public static void SetCenter(ref this BoundsInt bounds, Vector3Int center)
        {
            bounds.position = center - Vector3Int.CeilToInt(bounds.size / 2);
        }
    }
}