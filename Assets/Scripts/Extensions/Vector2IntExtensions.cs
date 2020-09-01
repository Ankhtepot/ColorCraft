using UnityEngine;

namespace Extensions
{
    public static class Vector2IntExtensions
    {
        public static Vector3Int ToVector3Int(this Vector2Int vector)
        {
            return new Vector3Int(vector.x, 0, vector.y);
        }
        
        public static Vector3 ToVector3(this Vector2Int vector)
        {
            return new Vector3(vector.x, 0f, vector.y);
        }
    }
}