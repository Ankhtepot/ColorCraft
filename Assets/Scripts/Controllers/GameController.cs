using System;
using UnityEngine;
using UnityEngine.Events;
using Utilities;
using Utilities.Enumerations;

//Fireball Games * * * PetrZavodny.com

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private bool inputEnabled;
        [SerializeField] private bool isGameLoopOn;
        [SerializeField] private GameMode gameMode;
        [SerializeField] private ScreenShotService screenshotService;
        [SerializeField] public UnityEvent OnGameInitiated;
        [SerializeField] public UnityEvent OnGameLoopStarted;
        [SerializeField] public UnityEvent OnMenuRequested;
        [SerializeField] public CustomUnityEvents.EventGameMode OnGameModeChanged;
        [SerializeField] public CustomUnityEvents.EventBool OnInputEnabledChanged;
        [SerializeField] public CustomUnityEvents.EventBool OnGameLoopStatusChanged;
        [HideInInspector] public string LastScreenshotPath;
#pragma warning restore 649

        public GameMode GameMode
        {
            get => gameMode;
            private set
            {
                gameMode = value;
                OnGameModeChanged?.Invoke(value);
            }
        }
        public bool InputEnabled
        {
            get => inputEnabled;
            set => SetInputEnabled(value);
        }

        private void Start()
        {
            initialize();
        }

        private void Update()
        {
            HandleInput();
        }

        private void InitializeNewGame()
        {
            OnGameInitiated?.Invoke();
        }

        private bool IsGameLoopOn 
        { 
            get => isGameLoopOn;
            set => SetGameLoopStatus(value);
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
                            GameMode = GameMode.Build; break;
                        case GameMode.Build:
                            GameMode = GameMode.Replace; break;
                        case GameMode.Beam:
                            GameMode = GameMode.FreeFlight; break;
                        case GameMode.OffGameLoop:
                            break;
                        case GameMode.Replace:
                            GameMode = GameMode.Beam; break;
                        case GameMode.Move:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        
            if (Input.GetKey(KeyCode.LeftShift) && (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.X)))
            {
                Application.Quit();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                LastScreenshotPath = screenshotService.TakeScreenShot("tempScreenshot");
                OnMenuRequested?.Invoke();
            }
        }

        public void SetInputEnabled(bool isEnabled)
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
            GameMode = GameMode.OffGameLoop;
            InputEnabled = false;
            IsGameLoopOn = false;
        
            MouseCursorController.LockCursor();
        }
        
        /// <summary>
        /// Run from SplashScreen OnStartGameButtonClick
        /// </summary>
        public void OnStartANewGameRequested()
        {
            InitializeNewGame();
            
            MouseCursorController.SetCursorVisibility(false);
        
            IsGameLoopOn = true;
            GameMode = GameMode.FreeFlight;
            InputEnabled = true;
        
            OnGameLoopStarted?.Invoke();
        }
    }
}
