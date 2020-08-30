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
        List<float> result = new List<float>();
        
        for (int i = 0; i < positions.Count; i++)
        {
            result.Add(Mathf.PerlinNoise(positions[i].x, positions[i].y));
        }

        return result;
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
            print("Ignoring movement");
            return;
        }
        
        var moveVector3 = newPosition - characterPosition.OldGridPosition;
        var moveVector2Int = new Vector2Int(moveVector3.x, moveVector3.z);
        // print($"moveVector: {moveVector2Int}");

        var offsetsChange = new Vector2Int(moveVector3.x, moveVector3.z);
        var oldWorldOffset = worldOffset;
        worldOffset += offsetsChange;
        
        if (moveVector2Int == Vector2Int.up)
        {
            print("Moving Front");
            MoveForward(oldWorldOffset);
        }
        else
        if (moveVector2Int == Vector2Int.down)
        {
            print("Moving Back");
            MoveBack(oldWorldOffset);
        }
        else
        if (moveVector2Int == Vector2Int.right)
        {
            print("Moving Right");
            MoveRight(oldWorldOffset);
        }
        else
        if (moveVector2Int == Vector2Int.left)
        {
            print("Moving Left");
            MoveLeft(oldWorldOffset);
        }
        else if (moveVector2Int == ForwardRight)
        {
            print("Moving Forward Right");
            MoveForward(oldWorldOffset);
            MoveRight(oldWorldOffset + Vector2Int.up);
        }
        else if (moveVector2Int == ForwardLeft)
        {
            print("Moving Forward Left");
            MoveForward(oldWorldOffset);
            MoveLeft(oldWorldOffset + Vector2Int.up);
        }
        else if (moveVector2Int == BackLeft)
        {
            print("Moving Back Left");
            MoveBack(oldWorldOffset);
            MoveLeft(oldWorldOffset + Vector2Int.down);
        }
        else if (moveVector2Int == BackRight)
        {
            print("Moving Back Right");
            MoveBack(oldWorldOffset);
            MoveRight(oldWorldOffset + Vector2Int.down);
        }
    }

    private void MoveForward(Vector2Int oldWorldOffset)
    {
        for (int i = 0; i < ratio; i++)
        {
            var oldElementKey = new Vector2Int(i + oldWorldOffset.x, oldWorldOffset.y);
            if (i == 0) print($"Old Element key: {oldElementKey}");
            var updatedYPosition = ratio + oldWorldOffset.y + Vector2Int.up.y - 1;

            MoveElement(oldElementKey, i, updatedYPosition, Vector2Int.up);
        }
    }

    private void MoveLeft(Vector2Int oldWorldOffset)
    {
        for (int i = 0; i < ratio; i++)
        {
            var oldElementKey = new Vector2Int(ratio + oldWorldOffset.x - 1, i + oldWorldOffset.y);
            if (i == 0) print($"Old Element key: {oldElementKey}");
            var updatedXPosition = worldOffset.x;

            MoveElement(oldElementKey, updatedXPosition, i, Vector2Int.left);
        }
    }

    private void MoveRight(Vector2Int oldWorldOffset)
    {
        for (int i = 0; i < ratio; i++)
        {
            var oldElementKey = new Vector2Int(oldWorldOffset.x, i + oldWorldOffset.y);
            if (i == 0) print($"Old Element key: {oldElementKey}");
            var updatedXPosition = ratio + worldOffset.x - 1;

            MoveElement(oldElementKey, updatedXPosition, i, Vector2Int.right);
        }
    }

    private void MoveBack(Vector2Int oldWorldOffset)
    {
        for (int i = 0; i < ratio; i++)
        {
            var oldElementKey = new Vector2Int(i + oldWorldOffset.x, ratio + oldWorldOffset.y - 1);
            if (i == 0) print($"Old Element key: {oldElementKey}");
            var updatedYPosition = worldOffset.y;

            MoveElement(oldElementKey, i, updatedYPosition, Vector2Int.down);
        }
    }

    private void MoveElement(Vector2Int oldElementKey, int updatedXPosition, int updatedYPosition, Vector2Int movementDirection)
    {
        float newYValue = GetPerlinNoiseValue(updatedXPosition, updatedYPosition);
        var elementToMove = GridMap[oldElementKey];
        var newElement2DPosition = new Vector2Int(updatedXPosition, updatedYPosition) ;
        if (updatedXPosition == 0 || updatedYPosition == 0) 
            print($"New Element key: {newElement2DPosition}");
        elementToMove.transform.position = new Vector3(
            updatedXPosition,
            Mathf.RoundToInt(newYValue * heightMultiplier),
            updatedYPosition);
        GridMap.Remove(oldElementKey);
        GridMapCoords.Remove(oldElementKey);
        GridMap.Add(newElement2DPosition, elementToMove);
        GridMapCoords.Add(newElement2DPosition);
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
