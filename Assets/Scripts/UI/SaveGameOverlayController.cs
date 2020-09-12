using System.IO;
using Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Utilities.Enumerations;

//Fireball Games * * * PetrZavodny.com

namespace UI
{
    public class SaveGameOverlayController : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private SaveLoadController saveLoad;
        [SerializeField] private TMP_InputField input;
        [SerializeField] private RawImage screenshotPivot;
        [SerializeField] private ScreenShotService screenShotService;
        [SerializeField] private GameController gameController;
        private byte[] screenshotBytes;
#pragma warning restore 649

        /// <summary>
        /// Run from VisibilityController OnVisibilityChanged
        /// </summary>
        /// <param name="shown"></param>
        public void OnVisibilityChanged(bool shown)
        {
            if (shown)
            {
                InitializeForm();
            }
            else
            {
                gameController.SetInputEnabled(true);
            }
        }

        private void InitializeForm()
        {
            gameController.SetInputEnabled(false);
            screenshotBytes = screenShotService.GetCurrentScreenshotBytes();
            input.text = Path.GetFileNameWithoutExtension(saveLoad.GetUniqueSaveName(SavedPositionType.Regular));
            screenshotPivot.texture = screenShotService.GetScreenshotFromBytes(screenshotBytes);
        }

        /// <summary>
        /// Triggered from SaveGameOverlay SaveButton
        /// </summary>
        public void SaveRegularPosition()
        {
            saveLoad.RegularSave(input.text, screenshotBytes);
        }
    }
}
