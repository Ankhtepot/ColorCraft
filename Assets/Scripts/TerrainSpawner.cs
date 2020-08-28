using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Fireball Games * * * PetrZavodny.com

public class TerrainSpawner : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private int gridSize;
    [SerializeField] private int worldTileSideSize;
    [SerializeField] private WorldTile tile;
    [SerializeField] private TerrainElement element;
    [SerializeField] private float defaultYOffset;
    [Range(0f, 1f)] [SerializeField] private float sampleScale = 0.1f;
    private EnvironmentControler environment;
    private int ratio;
    private float[,] perlinMap;
#pragma warning restore 649

    void Start()
    {
        initialize();
        SetPerlinMap(environment.XOffset, environment.YOffset);
        SetWorldTile();
    }

    private void SetPerlinMap(float environmentXOffset, float environmentYOffset)
    {
        perlinMap = new float[ratio,ratio];

        for (int x = 0; x < ratio; x++)        
        {
            for (int y = 0; y < ratio; y++)
            {
                print($"Perlin Noise on position {x},{y} is: {Mathf.PerlinNoise(environmentXOffset + x * sampleScale,environmentYOffset + y * sampleScale)}");
                perlinMap[x, y] = Mathf.PerlinNoise(environmentXOffset + x * sampleScale, environmentYOffset + y * sampleScale);
            }
        }
    }

    private void SetWorldTile()
    {
        var tileTransform = tile.transform;
        // tileTransform.localScale = new Vector3(worldTileSideSize, worldTileSideSize, worldTileSideSize);
        SpawnTerrainElements();
    }

    private void SpawnTerrainElements()
    {
        var tilePosition = tile.transform.localPosition;

        for (int x = 0; x < ratio; x++)
        {
            for (int y = 0; y < ratio; y++)
            {
                var newElement = Instantiate(element, new Vector3(tilePosition.x + x * gridSize, perlinMap[x,y] * 10, tilePosition.x + y * gridSize), Quaternion.identity);
                newElement.transform.parent = tile.transform;
            }
        }
    }

    private void initialize()
    {
        environment = FindObjectOfType<EnvironmentControler>();
        gridSize = environment.GridSize;
        worldTileSideSize = environment.WorldTileSideSize;
        
        if (worldTileSideSize % gridSize != 0)
        {
            Debug.LogWarning($"World tile side size ({worldTileSideSize}) is not dividable by grid size ({gridSize}) without remainder.");
        }

        ratio = Mathf.RoundToInt(worldTileSideSize / gridSize);
    }
}
