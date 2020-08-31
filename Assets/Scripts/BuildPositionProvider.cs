using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using HelperClasses;
using UnityEngine;
using UnityEngine.Events;

//Fireball Games * * * PetrZavodny.com

public class BuildPositionProvider : MonoBehaviour
{
#pragma warning disable 649
    public Camera targetCamera;
    // [SerializeField] private GameController gameController;
    public Vector3Int previewPosition;
    [SerializeField] public CustomUnityEvents.EventVector3Int OnPreviewPositionChanged;
    [SerializeField] public UnityEvent OnNoValidPreviewPosition;
    private Vector3Int previousPreviewPosition;
    private GameMode gameMode;
    
#pragma warning restore 649
    
    void Update()
    {
        GetPreviewPosition();
    }

    private void GetPreviewPosition()
    {
        if (gameMode != GameMode.Build) return;
        
        var ray = targetCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(targetCamera.transform.position, targetCamera.transform.forward, out var hit))
        {
            var objectHit = hit.transform;

            if (objectHit.CompareTag(Strings.BuildTopOnly) && hit.normal != Vector3.up)
            {
                return;
            }
            if (objectHit.CompareTag(Strings.BuildAllSides))
            {
                return;
            }
            // print($"Hit normal: {hit.normal}");
            previewPosition = (objectHit.position + hit.normal).ToVector3Int();

            if (previewPosition != previousPreviewPosition)
            {
                previousPreviewPosition = previewPosition;
                OnPreviewPositionChanged?.Invoke(previewPosition);
                Debug.DrawLine(targetCamera.transform.position, hit.point, Color.red);
            }
        }
        else
        {
            OnNoValidPreviewPosition?.Invoke();
        }
    }
    
    public void SetGameMode(GameMode newMode)
    {
        gameMode = newMode;
    }
}
