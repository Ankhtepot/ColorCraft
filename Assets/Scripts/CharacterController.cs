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
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float fastSpeed = 2f;
    [SerializeField] private float heightConstraint;
    
    [SerializeField] private Rigidbody rigidBody;
     
    private float yaw = 1f;
    private float pitch = 1f;
    [SerializeField] Position position;
    [SerializeField] TerrainSpawner terrainSpawner;
    [SerializeField] EnvironmentControler environment;
#pragma warning restore 649

    private void Awake()
    {
        terrainSpawner.SpawningFinished.AddListener(FixCharacterPosition);
    }

    void FixedUpdate ()
    {
            Move();
    }

    /// <summary>
    /// Run from GameController OnInputEnabledChanged
    /// </summary>
    /// <param name="state"></param>
    public void EnableMovement(bool state)
    {
        movementEnabled = state;
    }

    /// <summary>
    /// Run from TerrainSpawner OnTerrainSpawned.
    /// This method is needed only on new game world generation
    /// </summary>
    private void FixCharacterPosition()
    {
        float initialGridDimension = environment.GridSize * environment.WorldTileSideSize;
        var middleOfElementsVector = new Vector3(initialGridDimension / 2, 20, initialGridDimension / 2);
        var newPosition = position.GetGridPosition(middleOfElementsVector);
        newPosition = position.GetGridPosition(newPosition);
       
        var positionElement = terrainSpawner.GridMap
            .FirstOrDefault(element => element.Key.x == newPosition.x && element.Key.y == newPosition.z);
        
        transform.position = new Vector3(newPosition.x, positionElement.Value.transform.position.y + (1 * environment.GridSize), newPosition.z);
        
        terrainSpawner.SpawningFinished.RemoveListener(FixCharacterPosition); 
    }

    private void Move()
    {
        if (!movementEnabled) return;
        
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
        
        if (position.CurrentGridPosition.y < heightConstraint 
            && (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Space)))
        {
            MoveCharacter(Vector3.up);
        }
        
        if (Input.GetKey(KeyCode.Q))
        {
            MoveCharacter(Vector3.down);
        }
     
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
    }

    private void MoveCharacter(Vector3 direction)
    {
        var moveMultiplier = Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? fastSpeed : moveSpeed);
        transform.Translate(direction * moveMultiplier);
    }
}
