using UnityEngine;
using Utilities;

//Fireball Games * * * PetrZavodny.com

public class GameController : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private bool inputEnabled;
    [SerializeField] private bool isGameLoopOn;
    public GameMode GameMode { get; private set; }
    [SerializeField] public CustomUnityEvents.EventGameMode OnGameModeChanged;
    [SerializeField] public CustomUnityEvents.EventBool OnInputEnabledChanged;
    [SerializeField] public CustomUnityEvents.EventBool OnGameLoopStatusChanged;
#pragma warning restore 649
    
    public bool InputEnabled
    {
        get => inputEnabled;
        set => SetInputEnabled(value);
    }

    public bool IsGameLoopOn 
    { 
        get => isGameLoopOn;
        set => SetGameLoopStatus(value);
    }

    void Start()
    {
        initialize();
    }

    private void Update()
    {
            HandleInput();
    }

    private void HandleInput()
    {
        if (InputEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetMouseButtonDown(1))
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
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.X)))
        {
            Application.Quit();
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
    
    private void SetGameLoopStatus(bool value)
    {
        isGameLoopOn = value;
        OnGameLoopStatusChanged?.Invoke(value);
    }
    
    private void initialize()
    {
        SetGameMode(GameMode.FreeFlight);
        InputEnabled = true;
        IsGameLoopOn = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
}
