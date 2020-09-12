using Controllers;
using UnityEngine;

//Fireball Games * * * PetrZavodny.com

namespace UI
{
    public class HelpOverlayController : MonoBehaviour
    {
    #pragma warning disable 649
        [SerializeField] private GameObject HelpInfo;
        [SerializeField] private VisibilityController visibilityController;
    #pragma warning restore 649

        /// <summary>
        /// Run from GameUI.VisibilityController OnVisibilityChanged
        /// </summary>
        /// <param name="isVisible"></param>
        public void OnGameUIVisibilityChanged(bool isVisible)
        {
            HelpInfo.SetActive(isVisible);
        }

        public void OnInputEnabledChanged(bool isEnabled)
        {
            visibilityController.isEnabled = isEnabled;
        }
    }
}
