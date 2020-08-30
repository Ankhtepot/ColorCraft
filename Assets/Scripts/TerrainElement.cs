using System.Collections.Generic;
using UnityEngine;

//Fireball Games * * * PetrZavodny.com

public class TerrainElement : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] List<Material> HeightMaterials;
#pragma warning restore 649

    public void SetHeightMaterial()
    {
        var height = Mathf.RoundToInt(transform.position.y);
        
        if (height < HeightMaterials.Count)
        {
            GetComponentInChildren<MeshRenderer>().material = HeightMaterials[height];
        }
    }
}
