using UnityEngine;
using Utilities;

//Fireball Games * * * PetrZavodny.com

public class GameController : MonoBehaviour
{
#pragma warning disable 649
    public bool InputEnabled
    {
        get => inputEnabled;
        set => SetInputEnabled(value);
    }

    public GameMode GameMode { get; private set; }
    [SerializeField] public CustomUnityEvents.EventGameMode OnGameModeChanged;
    [SerializeField] public CustomUnityEvents.EventBool OnInputEnabledChanged;
    [SerializeField] private bool inputEnabled;
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
        inputEnabled = isEnabled;
        OnInputEnabledChanged?.Invoke(isEnabled);
    }
    
    private void initialize()
    {
        SetGameMode(GameMode.FreeFlight);
        InputEnabled = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
}
