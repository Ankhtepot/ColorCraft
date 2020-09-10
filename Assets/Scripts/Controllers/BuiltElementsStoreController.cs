using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEngine;
using Utilities;

//Fireball Games * * * PetrZavodny.com

namespace Controllers
{
    public class BuiltElementsStoreController : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private DetachedElementsChecker detachChecker;
        
        private static readonly Dictionary<Vector3Int, GameObject> store = new Dictionary<Vector3Int, GameObject>();
#pragma warning restore 649

        public static void AddElement(GameObject element)
        {
            var elementPosition = element.transform.position.ToVector3Int();
            
            if (!store.ContainsKey(elementPosition))
            {
                print($"ElementStore: adding element with ket: {elementPosition}");
                store.Add(elementPosition, element);
            }
            else
            {
                Debug.LogWarning($"ElementStore already had key {elementPosition} assigned");
            }
        }
   
        public static void RemoveElement(GameObject element)
        {
            RemoveElementWithPosition(element.transform.position);
        }

        public static bool ContainsKey(Vector3 key)
        {
            return store.ContainsKey(key.ToVector3Int());
        }

        public static List<Vector3Int> GetKeys()
        {
            return store.GetKeys();
        }

        public static GameObject GetElementAtPosition(Vector3Int position)
        {
            return store[position];
        }

        public static void RemoveElementWithPosition(Vector3 position)
        {
            var key = position.ToVector3Int();
            if (store.ContainsKey(key))
            {
                // print($"ElementStore: removing element with ket: {key}");
                store.Remove(key);
            }
        }
        
        public static void RemoveAndDestroyElementWithPosition(Vector3 position)
        {
            var key = position.ToVector3Int();
            if (store.ContainsKey(key))
            {
                Destroy(store[key].gameObject);
                // print($"ElementStore: removing element with ket: {key}");
                store.Remove(key);
            }
        }

        private static void SetActiveForStoreItems(Vector3Int coordinate, bool isActive)
        {
            store.Where(item => item.Key.x == coordinate.x && item.Key.z == coordinate.z)
                .ToList()
                .ForEach(foundItem => foundItem.Value.SetActive(isActive));
        }

        public static List<GameObject> GetBuiltElements()
        {
            return store.Select(item => item.Value).ToList();
        }

        public static Dictionary<Vector3Int, GameObject> GetStoreDictionary()
        {
            return store;
        }

        public static void CheckForDetachedElements(Vector3Int detachedPosition, List<Vector3Int> directions)
        {
            DetachedElementsChecker.CheckForDetachedElements(detachedPosition, directions);
        }
        
        /// <summary>
        /// Run from TerrainSpawner OnCoordinate Shown
        /// </summary>
        /// <param name="coordinate"></param>
        public void OnCoordinateShown(Vector3Int coordinate)
        {
            SetActiveForStoreItems(coordinate, true);
        }
   
        /// <summary>
        /// Run from TerrainSpawner OnCoordinate Hidden
        /// </summary>
        /// <param name="coordinate"></param>
        public void OnCoordinateHidden(Vector3Int coordinate)
        {
            SetActiveForStoreItems(coordinate, false);
        }
    }
}
