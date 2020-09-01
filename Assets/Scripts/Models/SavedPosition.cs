using System;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class SavedPosition
    {
        public int GridSize;
        public int WorldTileSide;
        public Vector2Int OriginalOffset;
        public Vector2Int CurrentOffset;
        public List<BuiltElementDescription> BuiltElements;
    }
}