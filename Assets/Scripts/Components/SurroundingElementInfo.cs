using System;
using Controllers;
using Extensions;
using UnityEngine;

namespace Components
{
    public class SurroundingElementInfo : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private TerrainSpawnerController terrainSpawner;
        [SerializeField] private BuiltElementsStoreController builtStore;
#pragma warning restore 649

        public bool ElementBellowIsNotDetached(Vector3 transformPosition)
        {
            return terrainSpawner.GridMap.ContainsKey(transformPosition.ToVector2IntXZ()) 
                   || builtStore.ContainsKey(transformPosition);
        }
    }
}