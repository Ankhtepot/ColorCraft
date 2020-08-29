using System;
using System.Linq;
using UnityEngine;

//Fireball Games * * * PetrZavodny.com

public class CharacterController : MonoBehaviour
{
    [SerializeField] private bool movementEnabled;
    
    [SerializeField] float lookSpeedH = 2f;
    [SerializeField] float lookSpeedV = 2f;
    [SerializeField] float zoomSpeed = 2f;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] private Rigidbody rigidBody;
     
    private float yaw = 1f;
    private float pitch = 1f;
    [SerializeField] Position position;
    [SerializeField] TerrainSpawner terrainSpawner;
    [SerializeField] EnvironmentControler environment;

    private void Start()
    {
        terrainSpawner.SpawningFinished.AddListener(FixCharacterPosition);
    }

    void FixedUpdate ()
    {
        if (movementEnabled)
        {
            Move();
        }
    }

    private void FixCharacterPosition()
    {
        float initialGridDimension = environment.GridSize * environment.WorldTileSideSize;
        var spawnPosition = position.GetGridPosition(new Vector3(initialGridDimension / 2, 20, initialGridDimension / 2));
        spawnPosition = position.GetGridPosition(spawnPosition);
        // print($"Spawn Position is: {spawnPosition}");
        
        // foreach (var el in terrainSpawner.GridMap.Keys)
        // {
        //     print($"el = {terrainSpawner.GridMap[el]}");
        //     // if((int)el.x == 15) print($"Found element: {terrainSpawner.GridMap[el]}, it's y-position is {el.y}");;
        // }
        var positionElement = terrainSpawner.GridMap
            .FirstOrDefault(element => element.Key.x == (int) spawnPosition.x && element.Key.y == (int) spawnPosition.z);
        // print($"Found element: {positionElement}, it's y-position is {positionElement.Key.y}");
        
        transform.position = new Vector3(spawnPosition.x, positionElement.Value.transform.position.y + (1 * environment.GridSize), spawnPosition.z);
    }

    private void Move()
    {
        yaw += lookSpeedH * Input.GetAxis("Mouse X");
        pitch -= lookSpeedV * Input.GetAxis("Mouse Y");
 
        transform.eulerAngles = new Vector3(pitch, yaw, 0f);
     
        var moveMultiplier = Time.deltaTime * moveSpeed;
        
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * moveMultiplier);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * moveMultiplier);
        }
        
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * moveMultiplier);
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * moveMultiplier);
        }
        
        if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Space))
        {
            transform.Translate(Vector3.up * moveMultiplier);
        }
        
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Translate(Vector3.down * moveMultiplier);
        }
     
        //Zoom in and out with Mouse Wheel
        transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, Space.Self);
        
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
    }
}
