using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Fireball Games * * * PetrZavodny.com

public class EnvironmentControler : MonoBehaviour
{
#pragma warning disable 649
    public int GridSize = 1;
    public int WorldTileSideSize = 100;
    [Header("Perlin Noise Setup")] 
    public float XOffset;
    public float YOffset;
#pragma warning restore 649

    void Start()
    {
        initialize();
    }

    void Update()
    {
        
    }
    
    private void initialize()
    {
        XOffset = 1f / Random.Range(0, 1000);
        YOffset = 1f / Random.Range(0, 1000);
    }
}
