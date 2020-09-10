using System.Collections.Generic;
using System.Linq;
using Controllers;
using Extensions;
using UnityEngine;
using static Utilities.Vector3Directions;

namespace Utilities
{
    public class SurroundingElementInfo : MonoBehaviour
    {
#pragma warning disable 649
        private static TerrainSpawnerController terrainSpawner;
        private static List<Vector3Int> groundElementsPositions;
#pragma warning restore 649

        private void Start()
        {
            terrainSpawner = FindObjectOfType<TerrainSpawnerController>();
        }

        public static bool ElementBellowIsNotDetached(Vector3 transformPosition)
        {
            return terrainSpawner.GridMap.ContainsKey(transformPosition.ToVector2IntXZ()) 
                   || BuiltElementsStoreController.ContainsKey(transformPosition);
        }

        public static bool IsGroundOrBuiltElementBellow(Vector3 position)
        {
            var positionBellow = position + Vector3.down;
            
            return terrainSpawner.IsGroundAtPosition(positionBellow) ||
                   BuiltElementsStoreController.ContainsKey(positionBellow);
        }
        
        public static List<Vector3Int> GetSurroundingElementsPositions(Vector3Int center, List<Vector3Int> directions = null)
        {
            return GetSurroundingPositions(center, directions ?? AllDirections)
                .Where(checkedPosition => BuiltElementsStoreController.ContainsKey(checkedPosition))
                .ToList();
        }
        
        private static IEnumerable<Vector3Int> GetSurroundingPositions(Vector3Int center, List<Vector3Int> directions)
        {
            return directions.Select(direction => direction + center).ToList();
        }

        public static bool IsAnyPositionAroundGround(Vector3Int center)
        {
            if (groundElementsPositions == null)
            {
                groundElementsPositions = terrainSpawner.GetGroundElementsPositions();
            }
            
            return GetSurroundingPositions(center, AllDirections)
                .Any(position => groundElementsPositions.Contains(position));
        }
    }
}