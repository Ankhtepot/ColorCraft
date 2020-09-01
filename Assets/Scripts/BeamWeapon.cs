using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

//Fireball Games * * * PetrZavodny.com

public class BeamWeapon : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private GameController gameController;
    [SerializeField] private ParticleSystem beamVFX;
#pragma warning restore 649

    private void Update()
    {
        HandleShooting();
    }

    private void HandleShooting()
    {
        if (gameController.InputEnabled && gameController.GameMode == GameMode.Destroy && Input.GetMouseButton(0))
        {
            beamVFX.Play();
        }
        else if (beamVFX.isPlaying)
        {
            beamVFX.Stop();
        }
    }

    // /// <summary>
    // /// Run from GameController OnInputEnabledChanged
    // /// </summary>
    // /// <param name="isEnabled"></param>
    // public void OnInputEnabledChange(bool isEnabled)
    // {
    //     isInputEnabled = isEnabled;
    // }
}
