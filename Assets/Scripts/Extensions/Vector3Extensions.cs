using UnityEngine;

namespace Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3Int ToVector3Int(this Vector3 source)
        {
            return new Vector3Int(
                Mathf.RoundToInt(source.x),
                Mathf.RoundToInt(source.y),
                Mathf.RoundToInt(source.z)
                );
        }
    }
}