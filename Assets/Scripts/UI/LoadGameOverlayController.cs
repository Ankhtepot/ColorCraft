using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using Models;
using UnityEngine;
using Utilities;
using Utilities.Enumerations;

//Fireball Games * * * PetrZavodny.com

namespace UI
{
    public class LoadGameOverlayController : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private SaveLoadController saveLoad;
        [SerializeField] private GameObject NoSavesFoundMesh;
        [SerializeField] private LoadPreviewController preview1;
        [SerializeField] private LoadPreviewController preview2;
        [SerializeField] private LoadPreviewController preview3;
        private int currentStartIndex = 0;
        private List<SavedPosition> loadedPositions = new List<SavedPosition>();
#pragma warning restore 649

        private void Update()
        {
            HandleInput();
        }
        
        private void HandleInput()
        {
            var delta = Input.GetAxis(Strings.MouseScrollWheel);

            if (!(Math.Abs(delta) > float.Epsilon)) return;
            
            if (delta > 0f)
            {
                currentStartIndex -= 1;
                SetLoadPreviews(currentStartIndex);
            }
            else if (delta < 0f)
            {
                currentStartIndex += 1;
                SetLoadPreviews(currentStartIndex);
            }
        }

        /// <summary>
        /// Run from VisibilityController OnVisibilityChanged
        /// </summary>
        /// <param name="shown"></param>
        public void OnVisibilityChanged(bool shown)
        {
            if (shown)
            {
                LoadPositions();
            }
            else
            {
                HidePreviews();
            }
        }

         private void HidePreviews()
         {
             preview1.gameObject.SetActive(false);
             preview2.gameObject.SetActive(false);
             preview3.gameObject.SetActive(false);
         }

         private void LoadPositions()
        {
           loadedPositions = saveLoad.LoadAllPositions().ToList();

            NoSavesFoundMesh.SetActive(loadedPositions.Count <= 0);

            if (loadedPositions.Count <= 0) return;

            SetLoadPreviews(0);
        }

        private void SetLoadPreviews( int startIndex)
        {
            if (loadedPositions == null) return;
            
            startIndex = Mathf.Clamp(startIndex, 0, loadedPositions.Count);
            currentStartIndex = startIndex;
            
            for (int i = 0; i < Mathf.Min(loadedPositions.Count,3); i++)
            {
                InstantiatePreviewForAnchor(i, loadedPositions[i + startIndex]);
            }
        }

        private void InstantiatePreviewForAnchor(int anchorIndex, SavedPosition position)
        {
            LoadPreviewController preview;
            switch (anchorIndex)
            {
                case 0: 
                    preview = preview1; 
                    preview1.gameObject.SetActive(true); break;
                case 1: 
                    preview = preview2;
                    preview2.gameObject.SetActive(true); break;
                case 2: 
                    preview = preview3;
                    preview3.gameObject.SetActive(true);break;
                default: throw new ArgumentOutOfRangeException($"Anchor index \"{anchorIndex}\" out of range");
            }
            
            preview.Initialize(position);
        }
    }
}
