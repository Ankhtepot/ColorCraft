using System.Collections.Generic;
using Extensions;
using UnityEngine;

namespace Utilities
{
    public static class Vector3Directions
    {
        public static readonly List<Vector3Int> AllDirections;
        public static readonly List<Vector3Int> HorizontalDirections;
        public static readonly List<Vector3Int> VerticalDirections;

        static Vector3Directions()
        {
            AllDirections = new List<Vector3Int>()
            {
                Vector3.up.ToVector3Int(),
                Vector3.left.ToVector3Int(),
                Vector3.right.ToVector3Int(),
                Vector3.forward.ToVector3Int(),
                Vector3.back.ToVector3Int(),
                Vector3.down.ToVector3Int(),
            };
            
            HorizontalDirections = new List<Vector3Int>()
            {
                Vector3.left.ToVector3Int(),
                Vector3.right.ToVector3Int(),
                Vector3.forward.ToVector3Int(),
                Vector3.back.ToVector3Int(),
            };
            
            VerticalDirections = new List<Vector3Int>()
            {
                Vector3.up.ToVector3Int(),
                Vector3.down.ToVector3Int(),
            };
        }
    }
}