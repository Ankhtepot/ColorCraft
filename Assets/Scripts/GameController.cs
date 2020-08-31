using System.Collections;
using System.Collections.Generic;
using HelperClasses;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

//Fireball Games * * * PetrZavodny.com

public class GameController : MonoBehaviour
{
#pragma warning disable 649
    public bool InputEnabled = true;
    public GameMode GameMode;
    [SerializeField] public CustomUnityEvents.EventGameMode OnGameModeChanged;
    [SerializeField] public CustomUnityEvents.EventBool OnInputEnabledChanged;
#pragma warning restore 649

    void Start()
    {
        initialize();
    }

    private void Update()
    {
        if (InputEnabled)
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            switch (GameMode)
            {
                case GameMode.FreeFlight:
                    SetGameMode(GameMode.Build); break;
                case GameMode.Build:
                    SetGameMode(GameMode.Destroy); break;
                case GameMode.Destroy:
                    SetGameMode(GameMode.FreeFlight); break;
            }
            
            
        }
    }

    private void SetGameMode(GameMode newMode)
    {
        GameMode = newMode;
        OnGameModeChanged?.Invoke(newMode);
    }

    private void SetInputEnabled(bool isEnabled)
    {
        InputEnabled = isEnabled;
        OnInputEnabledChanged?.Invoke(isEnabled);
    }
    
    private void initialize()
    {
        SetGameMode(GameMode.FreeFlight);
        SetInputEnabled(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
}
