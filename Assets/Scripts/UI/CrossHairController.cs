using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

//Fireball Games * * * PetrZavodny.com

namespace UI
{
    public class CrossHairController : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private Sprite freeFlightSprite;
        [SerializeField] private Sprite BuildSprite;
        [SerializeField] private Sprite DestroySprite;
        [SerializeField] private float freeFlightIconScale = 1f;
        [SerializeField] private float buildIconScale = 0.8f;
        [SerializeField] private float destroyIconScale = 0.8f;
        [SerializeField] private float fadeAppearStep = 0.05f;
        [SerializeField] private Image imagePivot;
        [SerializeField] private Animator animator;
        private GameMode gameMode;
        private bool isUIShown = true;
#pragma warning restore 649

        /// <summary>
        /// Run from GameController OnGameModeChanged
        /// </summary>
        /// <param name="newMode"></param>
        public void SetCrossHair(GameMode newMode)
        {
            gameMode = newMode;
        
            FadeAppearSymbol();
            
            switch (newMode)
            {
                case GameMode.FreeFlight:
                    SetForFreeFlightMode(); break;
                case GameMode.Build:
                    SetForBuildMode(); break;
                case GameMode.Beam:
                    SetDestroySprite(); break;
                default: SetForFreeFlightMode();
                    break;
            }
        }

        private void SetEmptySprite()
        {
            imagePivot.sprite = null;
        }

        /// <summary>
        /// Run from animator
        /// </summary>
        private void SetForFreeFlightMode()
        {
            animator.SetBool(Strings.AtTheTop, true);
        }

        private void SetFreeFlightSprite()
        {
            SetImagePivotScale(freeFlightIconScale);
            imagePivot.sprite = freeFlightSprite;
        }

        private void SetForBuildMode()
        {
            animator.SetBool(Strings.AtTheTop, false);
        }

        /// <summary>
        /// Run from animator
        /// </summary>
        private void SetBuildSprite()
        {
            if (gameMode == GameMode.Build)
            {
                SetImagePivotScale(buildIconScale);
                imagePivot.sprite = BuildSprite;
            }
        }

        private void SetDestroySprite()
        {
            SetImagePivotScale(destroyIconScale);
            imagePivot.sprite = DestroySprite;
        }

        private void SetImagePivotScale(float newScale)
        {
            imagePivot.transform.localScale = new Vector3(newScale, newScale, newScale);
        }

        public void SetCrosshairEnabled(bool isEnabled)
        {
            isUIShown = isEnabled;
            FadeAppearSymbol();
        }

        private void FadeAppearSymbol()
        {
            if (isUIShown && gameMode != GameMode.OffGameLoop)
            {
                StartCoroutine(AppearSymbol());
                return;
            }

            StartCoroutine(FadeSymbol());
        }

        private Color SetAlphaChannel(Color originalColor, int value)
        {
            value = Mathf.Clamp(value, 0, 255);
        
            var newColor = originalColor;
            newColor.a = value;
            return newColor;
        }

        IEnumerator FadeSymbol()
        {
            var newColor = imagePivot.color;
            while (imagePivot.color.a > 0)
            {
                yield return new WaitForFixedUpdate();
                newColor.a -= fadeAppearStep;
                imagePivot.color = newColor;
            }
        }

        IEnumerator AppearSymbol()
        {
            var newColor = imagePivot.color;
            while (imagePivot.color.a < 1f)
            {
                yield return new WaitForFixedUpdate();
                newColor.a += fadeAppearStep;
                imagePivot.color = newColor;
            }
        }
    }
}
