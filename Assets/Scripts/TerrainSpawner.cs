using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

//Fireball Games * * * PetrZavodny.com

public class TerrainSpawner : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private int gridSize;
    [SerializeField] private int worldTileSideSize;
    [SerializeField] private int heightMultiplier = 10;
    [Range(0f, 1f)] [SerializeField] private float sampleScale = 0.1f;
    [SerializeField] private Vector2Int currentPerlinOffset;
    [SerializeField] private Vector2Int worldOffset = new Vector2Int(0, 0);
    [Header("Assignables")]
    [SerializeField] private TerrainElement element;
    [SerializeField] private WorldTile tile;
    [SerializeField] private EnvironmentControler environment;
    [SerializeField] private Position characterPosition;

    [Header("Observed properties")] 
    [SerializeField] private int XOffset;
    [SerializeField] private int YOffset;
    private int ratio;
    private float[,] perlinMap;
    public Dictionary<Vector2Int, TerrainElement> GridMap = new Dictionary<Vector2Int, TerrainElement>();

    public UnityEvent SpawningFinished;
#pragma warning restore 649

    void Start()
    {
        
        initialize();
       
    }

    private void SetPerlinMap()
    {
        perlinMap = new float[ratio,ratio];

        for (int y = 0; y < ratio; y++)        
        {
            for (int x = 0; x < ratio; x++)
            {
                
                // var xCoordinate = (mapOffset.x + x / gridSize) * sampleScale;
                // var yCoordinate = (mapOffset.y + y / gridSize) * sampleScale;
                // print($"PerlinNoise Coordinates: [ {xCoordinate} , {yCoordinate} ].");
                
                // perlinMap[x, y] = Mathf.PerlinNoise(xCoordinate, yCoordinate);

                perlinMap[x, y] = GetPerlinNoiseValue(x, y);
            }
        }
    }

    private float GetPerlinNoiseValue(float relativeX, float relativeY)
    {
        var xCoordinate = (currentPerlinOffset.x + relativeX / gridSize) * sampleScale;
        var yCoordinate = (currentPerlinOffset.y + relativeY / gridSize) * sampleScale;
        // print($"PerlinNoise Coordinates: [ {xCoordinate} , {yCoordinate} ].");
                
        return Mathf.PerlinNoise(xCoordinate, yCoordinate);
    }

    private IEnumerable<float> Get1DPerlinNoiseValues(List<Vector2Int> positions)
    {
        List<float> result = new List<float>();
        
        for (int i = 0; i < positions.Count; i++)
        {
            result.Add(Mathf.PerlinNoise(positions[i].x, positions[i].y));
        }

        return result;
    }

    private void SetWorldTile()
    {
        var tileTransform = tile.transform;
        SpawnTerrainElements();
    }

    private async void SpawnTerrainElements()
    {
        var tilePosition = tile.transform.localPosition;

        for (int x = 0; x < ratio; x++)
        {
            for (int y = 0; y < ratio; y++)
            {
                var newElement = Instantiate(element, new Vector3(
                    tilePosition.x + x * gridSize,
                    Mathf.RoundToInt(perlinMap[x, y] * heightMultiplier),
                    tilePosition.y + y * gridSize), Quaternion.identity);
                
                var newElementTransform = newElement.transform;
                newElementTransform.parent = tile.transform;

                GridMap.Add(new Vector2Int((int)newElementTransform.position.x, (int)newElementTransform.position.z), newElement);
            }
        }
        
        SpawningFinished?.Invoke();
    }

    private void GetElementGrid()
    {
        var els = FindObjectsOfType<TerrainElement>();
            els.ToList()
            .ForEach(terrainElement => GridMap.Add(new Vector2Int((int)terrainElement.transform.position.x, (int)terrainElement.transform.position.z), terrainElement));
    }
    
    private void SpawnNewLine(Vector3 newPosition)
    {
        var moveVector = newPosition - characterPosition.oldGridPosition;
        // print($"moveVector: {moveVector}");

        if (moveVector == Vector3.forward)
        {
            // print($"newCoordinates: {currentPerlinOffset}");
            var offsetsChange = new Vector2Int((int) moveVector.x, (int) moveVector.z);
            var oldWorldOffset = worldOffset;
            worldOffset += offsetsChange;
            currentPerlinOffset -= offsetsChange;
            // print($"newPerlinOffset: {currentPerlinOffset}");
            int ratio = environment.WorldTileSideSize / gridSize;
            Dictionary<Vector2Int, TerrainElement> elementsToMove = new Dictionary<Vector2Int, TerrainElement>();
            for (int i = 0; i < ratio; i++)
            {
                var oldElementKey = new Vector2Int(i - oldWorldOffset.x,  0 + oldWorldOffset.y);
                
                print($"Old Element key: {oldElementKey}");
                var elementToMove = GridMap[oldElementKey];
                var newYValue = GetPerlinNoiseValue(i, ratio + worldOffset.y - 1);
                elementToMove.transform.position = new Vector3(i, Mathf.RoundToInt(newYValue * heightMultiplier), ratio + worldOffset.y - 1);
                GridMap.Remove(oldElementKey);
                var newElementPosition = new Vector2Int(i, ratio + worldOffset.y - 1);
                print($"New Element key: {newElementPosition}");
                GridMap.Add(newElementPosition, elementToMove);
            }
        }
    }

    private Vector2Int Generate2DOffset()
    {
        XOffset = Random.Range(0, 1000);
        YOffset = Random.Range(0, 1000);
        
        return new Vector2Int(XOffset, YOffset);
    }

    private void initialize()
    {
        characterPosition.OnPositionChanged.AddListener(SpawnNewLine);
        gridSize = environment.GridSize;
        worldTileSideSize = environment.WorldTileSideSize;
        
        if (worldTileSideSize % gridSize != 0)
        {
            Debug.LogWarning($"World tile side size ({worldTileSideSize}) is not dividable by grid size ({gridSize}) without remainder.");
        }

        ratio = Mathf.RoundToInt(worldTileSideSize / gridSize);

        currentPerlinOffset = Generate2DOffset();
        SetPerlinMap();
        SetWorldTile();
    }
}
