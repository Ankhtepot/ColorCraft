using System;
using UnityEngine;
using Utilities;
using Utilities.Enumerations;

//Fireball Games * * * PetrZavodny.com

namespace Controllers
{
    public class VisibilityController : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private AcceptKey acceptKey = AcceptKey.Always;
        [SerializeField] private KeyCode ShortcutKey;
        [SerializeField] private KeyCode AlternativeShortcutKey;
        [SerializeField] private bool isUIPartShown;
        [SerializeField] private Animator animator;
        public bool isEnabled = true;
        [SerializeField] public CustomUnityEvents.EventBool OnVisibilityChanged;
#pragma warning restore 649

        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            if (!isEnabled || !IsShortcutKeyDown()) return;

            switch (acceptKey)
            {
                case AcceptKey.Always:
                    ChangeVisibility();
                    break;
                case AcceptKey.WhenVisible:
                    if (isUIPartShown)
                    {
                        ChangeVisibility();
                    }
                    break;
                case AcceptKey.WhenHidden:
                    if (!isUIPartShown)
                    {
                        ChangeVisibility();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool IsShortcutKeyDown()
        {
            return Input.GetKeyDown(ShortcutKey) || Input.GetKeyDown(AlternativeShortcutKey);
        }

        private void ChangeVisibility()
        {
            isUIPartShown = !isUIPartShown;
            animator.SetBool(Strings.Show, isUIPartShown);
            
            OnVisibilityChanged?.Invoke(isUIPartShown);
        }

        /// <summary>
        /// Run from outside trigger
        /// </summary>
        public void OnOutsideTrigger()
        {
            ChangeVisibility();
        }
    }
}
