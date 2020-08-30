using System.Collections;
using System.Collections.Generic;
using HelperClasses;
using UnityEngine;
using UnityEngine.Events;

//Fireball Games * * * PetrZavodny.com

public class GameController : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private bool buildModeEnabled;
    [SerializeField] public CustomUnityEvents.EventBool OnBuildModeEnabled;
#pragma warning restore 649

    void Start()
    {
        SetBuildMode(true);
    }

    private void SetBuildMode(bool isEnabled)
    {
        buildModeEnabled = isEnabled;
        OnBuildModeEnabled?.Invoke(isEnabled);
    }

    void Update()
    {
        
    }
    
    private void initialize()
    {
       
    }
}
