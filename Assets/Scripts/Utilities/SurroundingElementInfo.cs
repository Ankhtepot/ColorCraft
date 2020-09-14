using System;
using System.Collections.Generic;
using System.Linq;
using Components;
using Controllers;
using Extensions;
using UnityEngine;
using Utilities.Enumerations;
using static Utilities.Vector3Directions;

namespace Utilities
{
    public class SurroundingElementInfo : MonoBehaviour
    {
#pragma warning disable 649
        private static GameController gameController;
        private static TerrainSpawnerController terrainSpawner;
#pragma warning restore 649

        private void Start()
        {
            gameController = FindObjectOfType<GameController>();
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

        public static IEnumerable<BuildElement> GetConnectedBuildElements(Vector3Int center)
        {
            return GetConnectedElementsPositions(center)
                .Select(position => BuiltElementsStoreController.GetElementAtPosition(position).GetComponent<BuildElement>());
        }
        
        public static List<Vector3Int> GetConnectedElementsPositions(Vector3Int center, List<Vector3Int> directions = null)
        {
            var elements = new List<Vector3Int>();

            var validDirections = AllDirections;

            if (gameController.GameMode != GameMode.Replace && BuiltElementsStoreController.ContainsKey(center))
            {
                var buildPositionFor = BuiltElementsStoreController.GetElementAtPosition(center).GetComponent<BuildElement>().BuildBaseOn;
                validDirections = ResolveDirectionsValidForBuildBasePosition(buildPositionFor);
            }

            foreach (var direction in validDirections)
            {
                var position = direction + center;

                if (BuiltElementsStoreController.ContainsKey(position))
                {
                    var buildBaseFor = BuiltElementsStoreController.GetElementAtPosition(position).GetComponent<BuildElement>().BuildBaseOn;
                    
                    if (HorizontalDirections.Contains(direction) 
                        && buildBaseFor == BuildPosition.Top)
                    {
                        continue;
                    }
                    
                    elements.Add(position);
                }
            }

            return elements;
        }
        
        public static List<Vector3Int> ResolveDirectionsValidForBuildBasePosition(BuildPosition buildPosition)
        {
            switch (buildPosition)
            {
                case BuildPosition.AllSides:
                    return AllDirections;
                case BuildPosition.Top:
                    return VerticalDirections;
                case BuildPosition.Ceiling:
                    return new List<Vector3Int>() {Vector3Int.up};
                case BuildPosition.None:
                    return new List<Vector3Int>();
                default:
                    throw new ArgumentOutOfRangeException(nameof(buildPosition), buildPosition, null);
            }
        }

        private static IEnumerable<Vector3Int> GetSurroundingPositions(Vector3Int center, List<Vector3Int> directions)
        {
            return directions.Select(direction => direction + center).ToList();
        }

        public static bool IsAnyPositionAroundGround(Vector3Int center)
        {
            var groundElementsPositions = terrainSpawner.GetGroundElementsPositions();
            
            return GetSurroundingPositions(center, AllDirections)
                .Any(position => groundElementsPositions.Contains(position));
        }
    }
}