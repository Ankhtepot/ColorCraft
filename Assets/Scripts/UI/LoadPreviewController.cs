using System;
using System.Diagnostics.CodeAnalysis;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Utilities.Enumerations;

//Fireball Games * * * PetrZavodny.com

namespace UI
{
    public class LoadPreviewController : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private ScreenShotService screenShotService;
        [SerializeField] private GameObject Background;
        [SerializeField] private GameObject QuickSaveIndicator;
        [SerializeField] private RawImage Image;
        [SerializeField] private TextMeshProUGUI TitleMesh;
        [SerializeField] private TextMeshProUGUI TimeStampMesh;
        [SerializeField] public CustomUnityEvents.EventSavedPosition OnPositionChosen;
        private SavedPosition savedPosition;
#pragma warning restore 649

        // public void Initialize(Texture2D image, string title, long timestampTicks, bool isQuickSave)
        public void Initialize(SavedPosition position)
        {
            savedPosition = position;
            var previewImage = screenShotService.LoadScreenshot(position.ScreenShotFileName);
            var title = FileServices.ExtractPositionTitleFromScreenshotPath(position.ScreenShotFileName);
            
            Image.texture = previewImage;
            TitleMesh.text = title;
            TimeStampMesh.text = new DateTime(position.DateTimeTicks).ToString("d MMMM yyyy hh:mm");
            QuickSaveIndicator.SetActive(position.positionType == SavedPositionType.QuickSave);
        }
        
        /// <summary>
        /// Run from Load Preview mouse over click
        /// </summary>
        public void OnClick()
        {
            OnPositionChosen?.Invoke(savedPosition);
        }
        
        /// <summary>
        /// Run From MouseOverlay
        /// </summary>
        /// <param name="isOver"></param>
        [SuppressMessage("ReSharper", "Unity.IncorrectMethodSignature")]
        public void OnMouseOver(bool isOver)
        {
            Background.SetActive(isOver);
        }
    }
}
