using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Fireball Games * * * PetrZavodny.com

public class SplashScreenOverlayController : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private GameObject content;
    [SerializeField] private Animator animator;
#pragma warning restore 649

    private void Awake()
    {
        content.SetActive(true);
    }

    /// <summary>
    /// Run from GameController OnGameLoopStarted
    /// </summary>
    public void OnGameLoopStarted()
    {
        content.SetActive(false);
    } 
}
