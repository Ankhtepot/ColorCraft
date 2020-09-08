using System.Collections;
using Components;
using Extensions;
using UnityEngine;
using UnityEngine.Events;
using Utilities.Enumerations;

//Fireball Games * * * PetrZavodny.com

namespace Utilities
{
    public class BuildPositionProvider : MonoBehaviour
    {
#pragma warning disable 649
        public Camera targetCamera;
        public Vector3Int previewPosition;
        public float canPreviewInvalidationPeriod = 0.01f;
        [SerializeField] public CustomUnityEvents.EventVector3IntVector3Int OnPreviewPositionChanged;
        [SerializeField] public UnityEvent OnNoValidPreviewPosition;
        private Vector3Int previousPreviewPosition;
        private GameMode gameMode;
        private bool canPreview = true;
    
#pragma warning restore 649
    
        void Update()
        {
            GetPreviewPosition();
        }

        private void GetPreviewPosition()
        {
            if (gameMode != GameMode.Build) return;
        
            if (canPreview && Physics.Raycast(targetCamera.transform.position, targetCamera.transform.forward, out var hit))
            {
                var objectHit = hit.transform;
                
                var buildPosition = GetBuildPosition(objectHit, true);

                if (buildPosition == BuildPosition.None) return;
            
                if (buildPosition == BuildPosition.Top && hit.normal != Vector3.up)
                {
                    canPreview = false;
                    StartCoroutine(CanPreviewCooldown());
                    return;
                }
            
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

        public static BuildPosition GetBuildPosition(Transform objectHit, bool inParent = false)
        {
            var terrainElement = inParent 
                ? objectHit.parent.GetComponent<TerrainElement>() 
                : objectHit.GetComponent<TerrainElement>();
            
            var buildElement = inParent 
                ? objectHit.parent.GetComponent<BuildElement>() 
                : objectHit.GetComponent<BuildElement>();

            if (!terrainElement && !buildElement)
            {
                return BuildPosition.None;
            }
            
            return terrainElement ? terrainElement.BuildBaseOn : buildElement.BuildBaseOn;
        }
        
        public void SetGameMode(GameMode newMode)
        {
            gameMode = newMode;
        }

        IEnumerator CanPreviewCooldown()
        {
            yield return new WaitForSeconds(canPreviewInvalidationPeriod);
            canPreview = true;
        }
    }
}
