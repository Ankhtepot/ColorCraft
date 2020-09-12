using Controllers;
using UnityEngine;
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
        private bool isShown = false;
#pragma warning restore 649

        private void Awake()
        {
            content.SetActive(true);
        }

        /// <summary>
        /// Run from GameController OnMenuRequested
        /// </summary>
        public void OnMenuRequested()
        {
            isShown = !isShown;
            ShowHideMenu(isShown);
        }

        public void ShowHideMenu(bool show)
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

        // /// <summary>
        // /// Run from TerrainSpawner OnSpawningFinished
        // /// </summary>
        // public void OnSpawningTerrainFinished()
        // {
        //     animator.SetTrigger(Strings.Swap);
        // }
    
        /// <summary>
        /// Run from GameController OnGameLoopStarted
        /// </summary>
        public void OnGameLoopStarted()
        {
            animator.SetTrigger(Strings.Hide);
        } 
    }
}
