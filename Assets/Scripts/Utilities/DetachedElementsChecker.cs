using System.Collections.Generic;
using System.Linq;
using Components;
using UnityEngine;

//Fireball Games * * * PetrZavodny.com

namespace Utilities
{
    public class DetachedElementsChecker : MonoBehaviour
    {
#pragma warning disable 649
        private static List<Vector3Int> groundElementsPositions;
#pragma warning restore 649

        public static void CheckForDetachedElements(BuildElement origin, List<Vector3Int> directions)
        {
            // var store = BuiltElementsStoreController.GetStoreDictionary();
            var elementsToCheck = origin.ConnectedTo;//;SurroundingElementInfo.GetConnectedElementsPositions(detachedPosition, directions);
            
            var detachedElements = new List<BuildElement>();
            
            foreach (var element in elementsToCheck)
            {
                //If is element in detachedElements, it means it was in the path of element on other already checked side.
                if (detachedElements.Contains(element))
                {
                    continue;
                }
                
                var newQueue = new Queue<BuildElement>();
                newQueue.Enqueue(element);
                detachedElements.AddRange(FindDetachedGroupOfElements(newQueue, detachedElements));
            }

            detachedElements = detachedElements.Distinct().ToList();

            detachedElements.ForEach(element => element.SetDetached());
        }

        private static IEnumerable<BuildElement> FindDetachedGroupOfElements(Queue<BuildElement> queue, List<BuildElement> detachedElements)
        {
            var elementsOnThePath = new List<BuildElement>();
            var exploredElements = new List<BuildElement>();
            
            while (queue.Count > 0)
            {
                var exploredElement = queue.Dequeue();
                elementsOnThePath.Add(exploredElement);
                exploredElements.Add(exploredElement);

                //if is ground around, then the path is not detached, return unchanged detachedElements
                if (exploredElement.IsGrounded)
                {
                    return detachedElements;
                }
                
                exploredElement.ConnectedTo.ForEach(element =>
                {
                    //if is not position already in the list, add it
                    if (!elementsOnThePath.Contains(element))
                    {
                        elementsOnThePath.Add(element);
                    }
                    //found positions also needs to be added to process them from queue
                    if (!queue.Contains(element) && !exploredElements.Contains(element))
                    {
                        queue.Enqueue(element);
                    }
                });
            }
            
            //if code gets here, it means, that all elements on the path are no grounded,
            //return concatenated detachedElements list then
            detachedElements.AddRange(elementsOnThePath);
            return detachedElements;
        }
    }
}
