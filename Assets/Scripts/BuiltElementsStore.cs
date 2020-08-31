using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEngine;

//Fireball Games * * * PetrZavodny.com

public class BuiltElementsStore : MonoBehaviour
{
#pragma warning disable 649
    private Dictionary<Vector3Int, GameObject> store = new Dictionary<Vector3Int, GameObject>();
#pragma warning restore 649

   public void AddElement(GameObject element)
   {
      store.Add(element.transform.position.ToVector3Int(), element);
   }
   
   public void RemoveElement(GameObject element)
   {
      RemoveElementAt(element.transform.position);
   }

   private void RemoveElementAt(Vector3 position)
   {
       var key = position.ToVector3Int();
       if (store.ContainsKey(key))
       {
           store.Remove(key);
       }
   }

   /// <summary>
   /// Run from TerrainSpawner OnCoordinate Shown
   /// </summary>
   /// <param name="coordinate"></param>
   public void OnCoordinateShown(Vector3Int coordinate)
   {
       setActiveForStoreItems(coordinate, true);
   }
   
   /// <summary>
   /// Run from TerrainSpawner OnCoordinate Hidden
   /// </summary>
   /// <param name="coordinate"></param>
   public void OnCoordinateHidden(Vector3Int coordinate)
   {
       setActiveForStoreItems(coordinate, false);
   }

   private void setActiveForStoreItems(Vector3Int coordinate, bool isActive)
   {
       store.Where(item => item.Key.x == coordinate.x && item.Key.z == coordinate.z)
           .ToList()
           .ForEach(foundItem => foundItem.Value.SetActive(isActive));
   }
}
