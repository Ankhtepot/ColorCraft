using Controllers;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

//Fireball Games * * * PetrZavodny.com

namespace UI
{
    public class MenuController : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private GameObject content;
        [SerializeField] private Animator animator;
        [SerializeField] private GameController gameController;
        [SerializeField] private Button SaveButton;
        private bool isShown = false;
#pragma warning restore 649

        private void Awake()
        {
            content.SetActive(true);
            SaveButton.interactable = false;
        }

        private void ShowHideMenu(bool show)
        {
            isShown = show;
            animator.SetBool(Strings.Show, show);

            if (show)
            {
                gameController.SetInputEnabled(false);
                MouseCursorController.UnlockCursor();
                MouseCursorController.SetCursorVisibility(true);
            }
            else
            {
                gameController.SetInputEnabled(true);
                MouseCursorController.LockCursor();
                MouseCursorController.SetCursorVisibility(false);
            }
        }

        /// <summary>
        /// Run from GameController OnGameLoopStarted
        /// </summary>
        public void OnGameLoopStarted()
        {
            SaveButton.interactable = true;
            animator.SetTrigger(Strings.Hide);
        } 
        
        /// <summary>
        /// Run from GameController OnMenuRequested
        /// </summary>
        public void OnMenuRequested()
        {
            isShown = !isShown;
            ShowHideMenu(isShown);
        }
    }
}
