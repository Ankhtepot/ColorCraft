using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

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
    /// Run from TerrainSpawner OnSpawningFinished
    /// </summary>
    public void OnSpawingTerrainFinished()
    {
        animator.SetTrigger(Strings.Swap); //TODO make animation
    }
    
    /// <summary>
    /// Run from GameController OnGameLoopStarted
    /// </summary>
    public void OnGameLoopStarted()
    {
        content.SetActive(false); //TODO add animation
    } 
}
