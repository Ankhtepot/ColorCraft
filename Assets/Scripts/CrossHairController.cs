using System.Collections;
using System.Collections.Generic;
using HelperClasses;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

//Fireball Games * * * PetrZavodny.com

public class CrossHairController : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private Sprite freeFlightSprite;
    [SerializeField] private Sprite BuildSprite;
    [SerializeField] private Sprite DestroySprite;
    [SerializeField] private Image imagePivot;
    [SerializeField] private Animator animator;
#pragma warning restore 649

    /// <summary>
    /// Used in GameController
    /// </summary>
    /// <param name="newMode"></param>
    public void SetCrossHair(GameMode newMode)
    {
        switch (newMode)
        {
            case GameMode.FreeFlight:
                SetForFreeFlightMode(); break;
            case GameMode.Build:
                SetForBuildMode(); break;
            case GameMode.Destroy:
                SetDestroySprite(); break;
            default: SetFreeFlightSprite();
                break;
        }
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
        imagePivot.sprite = BuildSprite;
    }

    private void SetDestroySprite()
    {
        imagePivot.sprite = DestroySprite;
    }
}
