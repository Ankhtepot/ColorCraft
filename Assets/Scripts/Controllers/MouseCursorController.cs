using System.Collections;
using UnityEngine;

//Fireball Games * * * PetrZavodny.com

namespace Controllers
{
    public class MouseCursorController : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private Texture2D[] MainCursorImages;
        [SerializeField] private Vector2 MainCursorHotSpot = Vector2.zero;
        [SerializeField] private CursorMode MainCursorMode = CursorMode.Auto;
        [SerializeField] private float idleAnimationStepDelay = 0.3f;
        [SerializeField] private float onHoverAnimationStepDelay = 0.1f;
        [SerializeField] private bool isHoverOn;
#pragma warning restore 649

        void Start()
        {
            initialize();
        }

        public static void SetCursorVisibility(bool isShown)
        {
            Cursor.visible = isShown;
        }

        public static void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    
        private void initialize()
        {
            if (MainCursorImages.Length == 0) return;
            
            SetCursor(MainCursorImages[0]);

            if (MainCursorImages.Length > 1)
            {
                StartCoroutine(AnimateCursor());
            }
        }

        IEnumerator AnimateCursor()
        {
            var currentStep = 0;
            while (Cursor.visible != false)
            {
                yield return new WaitForSeconds(isHoverOn ? onHoverAnimationStepDelay : idleAnimationStepDelay);
                currentStep = currentStep < MainCursorImages.Length - 1 ? currentStep += 1 : 0;
                SetCursor(MainCursorImages[currentStep]);
            }
        }

        private void SetCursor(Texture2D image)
        {
            Cursor.SetCursor(image, MainCursorHotSpot, MainCursorMode);
        }

        public void OnHoverOn()
        {
            
            isHoverOn = true;
        }

        public void OnHoverOff()
        {
            isHoverOn = false;
        }
    }
}
