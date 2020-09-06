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
        [SerializeField] public UnityEvent OnGameInitiated;
        [SerializeField] public UnityEvent OnGameLoopStarted;
        [SerializeField] public CustomUnityEvents.EventGameMode OnGameModeChanged;
        [SerializeField] public CustomUnityEvents.EventBool OnInputEnabledChanged;
        [SerializeField] public CustomUnityEvents.EventBool OnGameLoopStatusChanged;
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

        private void InitializeNewGame()
        {
            OnGameInitiated?.Invoke();
        }

        /// <summary>
        /// Run from SplashScreen OnStartGameButtonClick
        /// </summary>
        public void OnStartANewGameRequested()
        {
            SetMouseCursor(false);
        
            IsGameLoopOn = true;
            GameMode = GameMode.FreeFlight;
            InputEnabled = true;
        
            OnGameLoopStarted?.Invoke();
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
                            GameMode = GameMode.Build; break;
                        case GameMode.Build:
                            GameMode = GameMode.Beam; break;
                        case GameMode.Beam:
                            GameMode = GameMode.FreeFlight; break;
                    }
                }
            }
        
            if (Input.GetKeyDown(KeyCode.LeftShift) && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.X)))
            {
                Application.Quit();
            }
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

        private void SetMouseCursor(bool isShown)
        {
            Cursor.visible = isShown;
        }
    
        private void initialize()
        {
            GameMode = GameMode.OffGameLoop;
            InputEnabled = false;
            IsGameLoopOn = false;
        
            Cursor.lockState = CursorLockMode.Confined;
        
            InitializeNewGame();
        }
    }
}
