﻿using System.Collections.Generic;
using System.Linq;
using Components;
using Controllers;
using UnityEngine;

//Fireball Games * * * PetrZavodny.com

namespace Utilities
{
    public class DetachedElementsChecker : MonoBehaviour
    {
#pragma warning disable 649
        private static List<Vector3Int> groundElementsPositions;
#pragma warning restore 649

        public static void CheckForDetachedElements(Vector3Int detachedPosition, List<Vector3Int> directions)
        {
            var store = BuiltElementsStoreController.GetStoreDictionary();
            var elementsToCheck = SurroundingElementInfo.GetSurroundingElementsPositions(detachedPosition, directions);
            
            var detachedElements = new List<Vector3Int>();
            
            foreach (var position in elementsToCheck)
            {
                //If is position in detachedElements, it means it was in the path of element on other already checked side.
                if (detachedElements.Contains(position))
                {
                    continue;
                }
                
                var newQueue = new Queue<Vector3Int>();
                newQueue.Enqueue(position);
                detachedElements.AddRange(FindGround(newQueue, detachedElements));
            }

            detachedElements = detachedElements.Distinct().ToList();

            var effectedElements = detachedElements.Select(position => store[position]).ToList();
            effectedElements.ForEach(element => element.GetComponent<BuildElementLifeCycle>().IsDetached = true);
        }

        private static IEnumerable<Vector3Int> FindGround(Queue<Vector3Int> queue, List<Vector3Int> detachedElements, List<Vector3Int> directions = null)
        {
            var positionsOnThePath = new List<Vector3Int>();
            var exploredPositions = new List<Vector3Int>();
            
            while (queue.Count > 0)
            {
                var exploredPosition = queue.Dequeue();
                positionsOnThePath.Add(exploredPosition);
                exploredPositions.Add(exploredPosition);

                //if is ground around, then the path is not detached, return unchanged detachedElements
                if (SurroundingElementInfo.IsAnyPositionAroundGround(exploredPosition))
                {
                    return detachedElements;
                }
                
                var surroundingPositions = SurroundingElementInfo.GetSurroundingElementsPositions(exploredPosition, directions ?? Vector3Directions.AllDirections);
            
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
    }
}
