using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

//Fireball Games * * * PetrZavodny.com

public class BuildPositionProvider : MonoBehaviour
{
#pragma warning disable 649
    public Camera targetCamera;
    // [SerializeField] private GameController gameController;
    public Vector3Int previewPosition;
    [SerializeField] public CustomUnityEvents.EventVector3IntVector3Int OnPreviewPositionChanged;
    [SerializeField] public UnityEvent OnNoValidPreviewPosition;
    private Vector3Int previousPreviewPosition;
    private GameMode gameMode;
    [SerializeField] private string[] buildTags = {Strings.BuildAllSides, Strings.BuildTopOnly,};
    
#pragma warning restore 649
    
    void Update()
    {
        GetPreviewPosition();
    }

    private void GetPreviewPosition()
    {
        if (gameMode != GameMode.Build) return;
        
        if (Physics.Raycast(targetCamera.transform.position, targetCamera.transform.forward, out var hit))
        {
            var objectHit = hit.transform;

            if (!buildTags.Contains(objectHit.parent.tag)) return;
            
            if (objectHit.parent.CompareTag(Strings.BuildTopOnly) && hit.normal != Vector3.up) return;
            
            // print($"Hit normal: {hit.normal}");
            previewPosition = (objectHit.parent.position + hit.normal).ToVector3Int();

            if (previewPosition != previousPreviewPosition)
            {
                previousPreviewPosition = previewPosition;
                OnPreviewPositionChanged?.Invoke(previewPosition, hit.normal.ToVector3Int());
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
