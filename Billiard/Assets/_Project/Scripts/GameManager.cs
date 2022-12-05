using System;
using _Project.Scripts.Balls;
using _Project.Scripts.Save;
using _Project.Scripts.Spawning;
using _Project.Scripts.UI.Game;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts
{
    public enum GameState
    {
        Play,
        Reset,
        Won
    }
    [DefaultExecutionOrder(-100)]
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        [SerializeField] private Spawner spawner;
        [SerializeField] private BallManager ballManager;
        [SerializeField] private GameCanvasManager gameCanvasManager;
        
        private GameState _gameState;
        private int _score;
        private int _shots;
        private float _playtime;
        
        public event Action<GameState> OnGameStateChanged;
        public event Action<int> OnPointsChanged;
        public event Action<int> OnShotsChanged;
        public event Action<float> OnPlaytimeChanged;
        public Spawner Spawner => spawner;
        public BallManager BallManager => ballManager;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
        }

        private void Start()
        {
            OnGameStateChanged += ResetAll;
            spawner.OnCueBallSpawnComplete += ball => ball.OnWon += AddPoint;
            spawner.OnCueSpawnComplete += handler => handler.OnShot += IncreaseShots;
            gameCanvasManager.OnClickStart += () => ChangeGameState(GameState.Play);
            gameCanvasManager.OnClickNewGame += () => ChangeGameState(GameState.Reset);
            gameCanvasManager.OnClickMainMenu += () => SceneManager.LoadScene("MainMenu");
            
            ChangeGameState(GameState.Reset);
        }

        private void Update()
        {
            if (_gameState != GameState.Play) 
                return;
            
            _playtime += Time.deltaTime;
            OnPlaytimeChanged?.Invoke(_playtime);
        }

        private void ChangeGameState(GameState gameState)
        {
            _gameState = gameState;
            OnGameStateChanged?.Invoke(_gameState);
        }

        private void AddPoint()
        {
            _score++;
            OnPointsChanged?.Invoke(_score);
            if (_score >= 3)
                Won();
        }
        
        private void IncreaseShots()
        {
            _shots++;
            OnShotsChanged?.Invoke(_shots);
        }

        private void Won()
        {
            ChangeGameState(GameState.Won);
            SaveManager.Instance.SaveStats(new SaveStat(_score, _shots, _playtime));
        }

        private void ResetAll(GameState gameState)
        {
            if (gameState != GameState.Reset)
                return;
            
            _score = 0;
            _shots = 0;
            _playtime = 0;

            OnPointsChanged?.Invoke(_score);
            OnShotsChanged?.Invoke(_shots);
            OnPlaytimeChanged?.Invoke(_playtime);
        }
    }
}
