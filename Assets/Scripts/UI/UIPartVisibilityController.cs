using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

//Fireball Games * * * PetrZavodny.com

public class HelpOverlayControler : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private bool helpViewShown;
    [SerializeField] private Animator animator;
#pragma warning restore 649

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            helpViewShown = !helpViewShown;
            animator.SetBool(Strings.Show, helpViewShown);
        }
        
    }
}
