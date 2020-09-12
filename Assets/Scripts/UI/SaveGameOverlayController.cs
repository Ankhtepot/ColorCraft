using Controllers;
using TMPro;
using UnityEngine;
using Utilities.Enumerations;

//Fireball Games * * * PetrZavodny.com

namespace UI
{
    public class SaveGameOverlayController : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private SaveLoadController saveLoad;
        [SerializeField] private TMP_InputField input;
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
        }

        private void InitializeForm()
        {
            input.placeholder.GetComponentInChildren<TextMeshProUGUI>().text = saveLoad.GetUniqueSaveName(SavedPositionType.Regular);
        }
    }
}
