using System.Collections.Generic;
using System.Linq;
using Components;
using Controllers;
using UnityEngine;
using static Utilities.Vector3Directions;

//Fireball Games * * * PetrZavodny.com

namespace Utilities
{
    public class DetachedElementsChecker : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private TerrainSpawnerController terrainSpawner;
        private List<Vector3Int> groundElementsPositions;
        private Dictionary<Vector3Int, GameObject> elementStore;
        private List<Vector3Int> elementStoreKeys;
#pragma warning restore 649

        public void CheckForDetachedElements(Vector3Int detachedPosition, Dictionary<Vector3Int, GameObject> store)
        {
            if (groundElementsPositions == null)
            {
                groundElementsPositions = terrainSpawner.GetGroundElementsPositions();
            }
            
            elementStore = store;
            elementStoreKeys = store.Select(item => item.Key).ToList();
            
            var elementsToCheck = GetSurroundingElementsPositions(detachedPosition);
            
            var detachedElements = new List<Vector3Int>();
            
            foreach (var position in elementsToCheck)
            {
                //If is position in detachedElements, it means it was in the path of element on other side.
                if (detachedElements.Contains(position))
                {
                    continue;
                }
                
                var newQueue = new Queue<Vector3Int>();
                newQueue.Enqueue(position);
                detachedElements.AddRange(FindGround(newQueue, detachedElements));
            }

            detachedElements = detachedElements.Distinct().ToList();

            var effectedElements = detachedElements.Select(position => elementStore[position]).ToList();
            effectedElements.ForEach(element => element.GetComponent<BuildElementLifeCycle>().IsDetached = true);
        }

        private IEnumerable<Vector3Int> FindGround(Queue<Vector3Int> queue, List<Vector3Int> detachedElements)
        {
            var positionsOnThePath = new List<Vector3Int>();
            var exploredPositions = new List<Vector3Int>();
            
            while (queue.Count > 0)
            {
                var exploredPosition = queue.Dequeue();
                positionsOnThePath.Add(exploredPosition);
                exploredPositions.Add(exploredPosition);

                //if is ground around, then the path is not detached, return unchanged detachedElements
                if (IsAnyPositionAroundGround(exploredPosition))
                {
                    return detachedElements;
                }
                
                var surroundingPositions = GetSurroundingElementsPositions(exploredPosition);
            
                surroundingPositions.ForEach(position =>
                {
                    //if is not position already in the list, add it
                    if (!positionsOnThePath.Contains(position))
                    {
                        positionsOnThePath.Add(position);
                    }
                    //found positions also needs to be added to process them from queue
                    if (!queue.Contains(position) && !exploredPositions.Contains(position))
                    {
                        queue.Enqueue(position);
                    }
                });
            }
            
            //if code gets here, it means, that all elements on the path are no grounded,
            //return concatenated detachedElements list then
            detachedElements.AddRange(positionsOnThePath);
            return detachedElements;
        }

        private List<Vector3Int> GetSurroundingElementsPositions(Vector3Int center)
        {
            return GetSurroundingPositions(center)
                .Where(checkedPosition => elementStoreKeys.Contains(checkedPosition))
                .ToList();
        }

        private IEnumerable<Vector3Int> GetSurroundingPositions(Vector3Int center)
        {
            return AllDirections.Select(direction => direction + center).ToList();
        }

        private bool IsAnyPositionAroundGround(Vector3Int center)
        {
            return GetSurroundingPositions(center)
                .Any(position => groundElementsPositions.Contains(position));
        }
    }
}
