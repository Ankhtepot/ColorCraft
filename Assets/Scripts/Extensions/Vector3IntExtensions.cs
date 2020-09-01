﻿using UnityEngine;

namespace Extensions
{
    public static class Vector3IntExtensions
    {
        public static Vector2Int ToVector3Int(this Vector3Int source)
        {
            return new Vector2Int(source.x, source.z);
        }
    }
}