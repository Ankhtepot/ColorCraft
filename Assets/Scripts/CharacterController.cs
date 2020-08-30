using System;
using System.Linq;
using UnityEngine;

//Fireball Games * * * PetrZavodny.com

public class CharacterController : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private bool movementEnabled;
    
    [SerializeField] float lookSpeedH = 2f;
    [SerializeField] float lookSpeedV = 2f;
    [SerializeField] float zoomSpeed = 2f;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float fastSpeed = 2f;
    
    [SerializeField] private Rigidbody rigidBody;
     
    private float yaw = 1f;
    private float pitch = 1f;
    [SerializeField] Position position;
    [SerializeField] TerrainSpawner terrainSpawner;
    [SerializeField] EnvironmentControler environment;
#pragma warning restore 649

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

    public void EnableMovement(bool state)
    {
        movementEnabled = state;
    }

    private void FixCharacterPosition()
    {
        float initialGridDimension = environment.GridSize * environment.WorldTileSideSize;
        var spawnPosition = position.GetGridPosition(new Vector3(initialGridDimension / 2, 20, initialGridDimension / 2));
        spawnPosition = position.GetGridPosition(spawnPosition);
       
        var positionElement = terrainSpawner.GridMap
            .FirstOrDefault(element => element.Key.x == (int) spawnPosition.x && element.Key.y == (int) spawnPosition.z);
        
        transform.position = new Vector3(spawnPosition.x, positionElement.Value.transform.position.y + (1 * environment.GridSize), spawnPosition.z);
    }

    private void Move()
    {
        yaw += lookSpeedH * Input.GetAxis("Mouse X");
        pitch -= lookSpeedV * Input.GetAxis("Mouse Y");
 
        transform.eulerAngles = new Vector3(pitch, yaw, 0f);
     
        
        
        if (Input.GetKey(KeyCode.A))
        {
            MoveCharacter(Vector3.left);
        }

        if (Input.GetKey(KeyCode.D))
        {
            MoveCharacter(Vector3.right);
        }
        
        if (Input.GetKey(KeyCode.W))
        {
            MoveCharacter(Vector3.forward);
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            MoveCharacter(Vector3.back);
        }
        
        if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Space))
        {
            MoveCharacter(Vector3.up);
        }
        
        if (Input.GetKey(KeyCode.Q))
        {
            MoveCharacter(Vector3.down);
        }
     
        //Zoom in and out with Mouse Wheel
        transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, Space.Self);
        
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
    }

    private void MoveCharacter(Vector3 direction)
    {
        var moveMultiplier = Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? fastSpeed : moveSpeed);
        transform.Translate(direction * moveMultiplier);
    }
}
