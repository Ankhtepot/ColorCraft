using System.Collections.Generic;
using UnityEngine;

//Fireball Games * * * PetrZavodny.com

namespace Components
{
    public class TerrainElement : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] List<Material> HeightMaterials;
#pragma warning restore 649

        public void SetHeightMaterial()
        {
            var height = Mathf.Clamp(Mathf.RoundToInt(transform.position.y), 0, HeightMaterials.Count - 1);
        
            GetComponentInChildren<MeshRenderer>().material = HeightMaterials[height];
        }
    }
}
