using System;
using _Project.Scripts.Audio;
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
        Replay,
        Init,
        Won
    }
    [DefaultExecutionOrder(-100)]
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        [SerializeField] private Spawner spawner;
        [SerializeField] private BallManager ballManager;
        [SerializeField] private GameCanvasManager gameCanvasManager;

        private int _score;
        private int _shots;
        private float _playtime;
        
        public event Action<GameState> OnGameStateChanged;
        public event Action<int> OnPointsChanged;
        public event Action<int> OnShotsChanged;
        public event Action<float> OnPlaytimeChanged;
        public Spawner Spawner => spawner;
        public BallManager BallManager => ballManager;
        public GameState GameState { get; private set; }

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
            gameCanvasManager.OnClickReplay += () => ChangeGameState(GameState.Replay);
            gameCanvasManager.OnClickNewGame += () => SceneManager.LoadScene("Game");
            gameCanvasManager.OnClickMainMenu += () => SceneManager.LoadScene("MainMenu");
            
            ChangeGameState(GameState.Init);
        }

        private void Update()
        {
            if (GameState != GameState.Play) 
                return;
            
            _playtime += Time.deltaTime;
            OnPlaytimeChanged?.Invoke(_playtime);
        }

        private void ChangeGameState(GameState gameState)
        {
            GameState = gameState;
            OnGameStateChanged?.Invoke(GameState);
        }

        private void AddPoint()
        {
            if (GameState == GameState.Replay)
            {
                ChangeGameState(GameState.Play);
            }
            else
            {
                _score++;
                OnPointsChanged?.Invoke(_score);
                if (_score >= 3)
                    Won();
            }
        }
        
        private void IncreaseShots()
        {
            _shots++;
            OnShotsChanged?.Invoke(_shots);
        }

        private void Won()
        {
            if (GameState == GameState.Won)
                return;
            
            ChangeGameState(GameState.Won);
            
            AudioManager.Instance.PlayVictory();
            
            if (SaveManager.Instance != null)
                SaveManager.Instance.SaveStats(new SaveStat(_score, _shots, _playtime));
        }

        private void ResetAll(GameState gameState)
        {
            if (gameState != GameState.Init)
                return;
            
            _score = 0;
            _shots = 0;
            _playtime = 0;

            OnPointsChanged?.Invoke(_score);
            OnShotsChanged?.Invoke(_shots);
            OnPlaytimeChanged?.Invoke(_playtime);
        }

        public void ReplayFinished()
        {
            if (GameState != GameState.Replay)
                return;
            
            ChangeGameState(GameState.Play);
        }
    }
}
