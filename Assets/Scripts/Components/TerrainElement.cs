using System.Collections.Generic;
using UnityEngine;
using Utilities.Enumerations;

//Fireball Games * * * PetrZavodny.com

namespace Components
{
    public class TerrainElement : MonoBehaviour
    {
#pragma warning disable 649
        public BuildPosition BuildBaseOn = BuildPosition.AllSides;
        [SerializeField] private List<Material> HeightMaterials;
#pragma warning restore 649

        public void SetHeightMaterial()
        {
            var height = Mathf.Clamp(Mathf.RoundToInt(transform.position.y), 0, HeightMaterials.Count - 1);
        
            GetComponentInChildren<MeshRenderer>().material = HeightMaterials[height];
        }
    }
}
