using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Enumerations;

namespace Models
{
    [Serializable]
    public class SavedPosition
    {
        public int GridSize;
        public int WorldTileSide;
        public SavedPositionType positionType;
        public Vector2Int OriginalOffset;
        public Vector2Int TraversedOffset;
        public Vector3 CharacterPosition;
        public Quaternion CharacterRotation;
        public string ScreenShotFileName;
        public long DateTimeTicks;
        public List<BuiltElementDescription> BuiltElements;
    }
}