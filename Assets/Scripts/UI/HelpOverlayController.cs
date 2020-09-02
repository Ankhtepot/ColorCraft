using UnityEngine;

//Fireball Games * * * PetrZavodny.com

namespace UI
{
    public class HelpOverlayController : MonoBehaviour
    {
    #pragma warning disable 649
        [SerializeField] private GameObject HelpInfo;
    #pragma warning restore 649

        /// <summary>
        /// Run from GameUI.VisibilityController OnVisibilityChanged
        /// </summary>
        /// <param name="isVisible"></param>
        public void OnGameUIVisibilityChanged(bool isVisible)
        {
            HelpInfo.SetActive(isVisible);
        }
    }
}
