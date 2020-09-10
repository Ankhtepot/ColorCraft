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
        [SerializeField] public CustomUnityEvents.EventVector3Int OnReplacePreviewPositionChanged;
        [SerializeField] public UnityEvent OnNoValidPreviewPosition;
        
        private Vector3Int previousPreviewPosition;
        private GameMode gameMode;
        private bool canPreview = true;
#pragma warning restore 649

        private void Update()
        {
            GetPreviewPosition();
        }

        private void GetPreviewPosition()
        {
            switch (gameMode)
            {
                case GameMode.Build : GetBuildModePreviewPosition(); break;
                case GameMode.Replace : GetReplaceModePreviewPosition(); break;
            }
        }

        private void GetBuildModePreviewPosition()
        {
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
            
                previewPosition = (objectHit.parent.position + hit.normal).ToVector3Int();

                if (previewPosition == previousPreviewPosition) return;
                
                previousPreviewPosition = previewPosition;
                OnPreviewPositionChanged?.Invoke(previewPosition, hit.normal.ToVector3Int());
            }
            else
            {
                OnNoValidPreviewPosition?.Invoke();
            }
        }
        
        private void GetReplaceModePreviewPosition()
        {
            if (Physics.Raycast(targetCamera.transform.position, targetCamera.transform.forward, out var hit))
            {
                var objectHit = hit.transform;

                previewPosition = (objectHit.parent.position).ToVector3Int();

                if (previewPosition == previousPreviewPosition || !objectHit.GetComponentInParent<BuildElement>())
                {
                    return;
                }
                
                previousPreviewPosition = previewPosition;
                OnReplacePreviewPositionChanged?.Invoke(previewPosition);
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

        private IEnumerator CanPreviewCooldown()
        {
            yield return new WaitForSeconds(canPreviewInvalidationPeriod);
            canPreview = true;
        }
        
        /// <summary>
        /// Run from GameController OnGameModeChanged
        /// </summary>
        /// <param name="newMode"></param>
        public void SetGameMode(GameMode newMode)
        {
            gameMode = newMode;
        }
    }
}
