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
    [SerializeField] private Transform elementParent;
    [SerializeField] private EnvironmentControler environment;
    [SerializeField] private Position characterPosition;

    [Header("Observed properties")] 
    [SerializeField] private int XOffset;
    [SerializeField] private int YOffset;
    private int ratio;
    private float[,] perlinMap;
    public Dictionary<Vector2Int, TerrainElement> GridMap = new Dictionary<Vector2Int, TerrainElement>();
    public List<Vector2Int> GridMapCoords = new List<Vector2Int>();

    public UnityEvent SpawningFinished;
#pragma warning restore 649
    private readonly Vector2Int ForwardRight = new Vector2Int(1, 1);
    private readonly Vector2Int ForwardLeft = new Vector2Int(-1, 1);
    private readonly Vector2Int BackRight = new Vector2Int(1, -1);
    private readonly Vector2Int BackLeft = new Vector2Int(-1, -1);
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
                perlinMap[x, y] = GetPerlinNoiseValue(x, y);
            }
        }
    }

    private float GetPerlinNoiseValue(float relativeX, float relativeY)
    {
        var xCoordinate = (currentPerlinOffset.x + relativeX / gridSize) * sampleScale;
        var yCoordinate = (currentPerlinOffset.y + relativeY / gridSize) * sampleScale;
        // print($"Perlin Noise unscaled coordinates: x: {currentPerlinOffset.x + relativeX / gridSize}, y: {currentPerlinOffset.y + relativeY / gridSize}");
        // print($"PerlinNoise Coordinates: [ {xCoordinate} , {yCoordinate} ].");
                
        return Mathf.PerlinNoise(xCoordinate, yCoordinate);
    }

    private IEnumerable<float> Get1DPerlinNoiseValues(List<Vector2Int> positions)
    {
        return positions.Select(item => Mathf.PerlinNoise(item.x, item.y));
    }

    private void SetWorldTile()
    {
        var tileTransform = elementParent.transform;
        SpawnTerrainElements();
    }

    private void SpawnTerrainElements()
    {
        var tilePosition = elementParent.transform.localPosition;

        for (int x = 0; x < ratio; x++)
        {
            for (int y = 0; y < ratio; y++)
            {
                var newElement = Instantiate(element, new Vector3(
                    tilePosition.x + x * gridSize,
                    Mathf.RoundToInt(perlinMap[x, y] * heightMultiplier),
                    tilePosition.y + y * gridSize), Quaternion.identity);
                
                var newElementTransform = newElement.transform;
                newElementTransform.parent = elementParent.transform;

                GridMap.Add(new Vector2Int((int)newElementTransform.position.x, (int)newElementTransform.position.z), newElement);
                GridMapCoords.Add(new Vector2Int((int)newElementTransform.position.x, (int)newElementTransform.position.z));
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
    
    private void SpawnNewLine(Vector3Int newPosition)
    {
        if (newPosition.y == 1 || newPosition.y == -1)
        {
            return;
        }
        
        var moveVector3 = newPosition - characterPosition.OldGridPosition;
        var moveVector2Int = new Vector2Int(moveVector3.x, moveVector3.z);
        // print($"moveVector: {moveVector2Int}");

        var offsetsChange = new Vector2Int(moveVector3.x, moveVector3.z);
        var oldWorldOffset = worldOffset;
        
        
        if (moveVector2Int == Vector2Int.up)
        {
            worldOffset += offsetsChange;
            // print("Moving Front");
            MoveForward(oldWorldOffset);
        }
        else
        if (moveVector2Int == Vector2Int.down)
        {
            worldOffset += offsetsChange;
            // print("Moving Back");
            MoveBack(oldWorldOffset);
        }
        else
        if (moveVector2Int == Vector2Int.right)
        {
            worldOffset += offsetsChange;
            // print("Moving Right");
            MoveRight(oldWorldOffset);
        }
        else
        if (moveVector2Int == Vector2Int.left)
        {
            worldOffset += offsetsChange;
            // print("Moving Left");
            MoveLeft(oldWorldOffset);
        }
        else if (moveVector2Int == ForwardRight)
        {
            worldOffset += Vector2Int.up;
            // print("Moving Forward Right");
            MoveForward(oldWorldOffset);
            worldOffset += Vector2Int.right;
            MoveRight(oldWorldOffset + Vector2Int.up);
        }
        else if (moveVector2Int == ForwardLeft)
        {
            worldOffset += Vector2Int.up;
            // print("Moving Forward Left");
            MoveForward(oldWorldOffset);
            worldOffset += Vector2Int.left;
            MoveLeft(oldWorldOffset + Vector2Int.up);
        }
        else if (moveVector2Int == BackLeft)
        {
            worldOffset += Vector2Int.down;
            // print("Moving Back Left");
            MoveBack(oldWorldOffset);
            worldOffset += Vector2Int.left;
            MoveLeft(oldWorldOffset + Vector2Int.down);
        }
        else if (moveVector2Int == BackRight)
        {
            worldOffset += Vector2Int.down;
            // print("Moving Back Right");
            MoveBack(oldWorldOffset);
            worldOffset += Vector2Int.right;
            MoveRight(oldWorldOffset + Vector2Int.down);
        }
    }

    private void MoveForward(Vector2Int oldWorldOffset)
    {
        for (int i = 0; i < ratio; i++)
        {
            var oldGridPosition = new Vector2Int(i + oldWorldOffset.x, oldWorldOffset.y);
            // print($"Old Element key: {oldGridPosition}");
            var newGridPosition = new Vector2Int(i,ratio - 1) + worldOffset;
            // print($"New Element key: {newGridPosition}");
            MoveElement(oldGridPosition, newGridPosition);
        }
    }

    private void MoveLeft(Vector2Int oldWorldOffset)
    {
        for (int i = 0; i < ratio; i++)
        {
            var oldGridPosition = new Vector2Int(ratio + oldWorldOffset.x - 1, i + oldWorldOffset.y);
            // print($"Old Element key: {oldGridPosition}");
            var newGridPosition = new Vector2Int(0,i) + worldOffset;
            // print($"New Element key: {newGridPosition}");
            MoveElement(oldGridPosition, newGridPosition);
        }
    }

    private void MoveRight(Vector2Int oldWorldOffset)
    {
        for (int i = 0; i < ratio; i++)
        {
            var oldGridPosition = new Vector2Int(oldWorldOffset.x, i + oldWorldOffset.y);
            // print($"Old Element key: {oldGridPosition}");
            var newGridPosition = new Vector2Int(ratio - 1,i) + worldOffset;
            // print($"New Element key: {newGridPosition}");
            MoveElement(oldGridPosition, newGridPosition);
        }
    }

    private void MoveBack(Vector2Int oldWorldOffset)
    {
        for (int i = 0; i < ratio; i++)
        {
            var oldGridPosition = new Vector2Int(i + oldWorldOffset.x, ratio + oldWorldOffset.y - 1);
            // print($"Old Element key: {oldGridPosition}");
            var newGridPosition = new Vector2Int(i,0) + worldOffset;
            // print($"New Element key: {newGridPosition}");
            MoveElement(oldGridPosition, newGridPosition);
        }
    }

    private void MoveElement(Vector2Int oldElementKey, Vector2Int newGridPosition)
    {
        float newYHeightValue = GetPerlinNoiseValue(newGridPosition.x, newGridPosition.y);
        var elementToMove = GridMap[oldElementKey];
        
        elementToMove.transform.position = new Vector3(
            newGridPosition.x,
            Mathf.RoundToInt(newYHeightValue * heightMultiplier),
            newGridPosition.y);
        GridMap.Remove(oldElementKey);
        GridMapCoords.Remove(oldElementKey);
        GridMap.Add(newGridPosition, elementToMove);
        GridMapCoords.Add(newGridPosition);
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
