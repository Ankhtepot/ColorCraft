using UnityEngine;
using UnityEngine.Events;

//Fireball Games * * * PetrZavodny.com

public class Position : MonoBehaviour
{
#pragma warning disable 649
    public Vector3 currentGridPosition;
    public Vector3 oldGridPosition;
    [SerializeField] public EventVector3 OnPositionChanged;
    [SerializeField] EnvironmentControler environment;
#pragma warning restore 649

    [System.Serializable]
    public class EventVector3 : UnityEvent<Vector3> {};

    private void Start()
    {
        currentGridPosition = GetGridPosition();
        oldGridPosition = currentGridPosition;
    }

    void Update()
    {
        ManagePositionChange();
    }

    private void ManagePositionChange()
    {
        var newPosition = GetGridPosition();
        
        if (newPosition != currentGridPosition)
        {
            oldGridPosition = currentGridPosition;
            currentGridPosition = newPosition;
            OnPositionChanged?.Invoke(newPosition);
        }        
    }

    private Vector3 GetGridPosition()
    {
        return GetGridPosition(transform.position);
    }

    private float NormalizePositionVector(float vector)
    {
        var gridSize = environment.GridSize;
        return Mathf.RoundToInt(vector) * gridSize;
    }

    public Vector3 GetGridPosition(Vector3 worldPosition)
    {
        return new Vector3(
            NormalizePositionVector(worldPosition.x),
            NormalizePositionVector(worldPosition.y),
            NormalizePositionVector(worldPosition.z));
    }
}
