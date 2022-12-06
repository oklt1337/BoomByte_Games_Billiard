using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Game
{
    public class GameCanvasManager : MonoBehaviour
    {
        [Header("GamePlayPanel")]
        [SerializeField] private GameObject gamePlayPanel;
        [SerializeField] private Button startButton;
        [SerializeField] private Button replayButton;
        [SerializeField] private Slider forceSlider;
        [SerializeField] private TMP_Text shots;
        [SerializeField] private TMP_Text score;
        [SerializeField] private TMP_Text playtime;
        
        [Header("GameOverPanel")]
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button newGameButton;
        [SerializeField] private TMP_Text finalShots;
        [SerializeField] private TMP_Text finalScore;
        [SerializeField] private TMP_Text finalPlaytime;
        
        private Image _forceFillImage;

        public event Action OnClickStart;
        public event Action OnClickReplay;
        public event Action OnClickNewGame;
        public event Action OnClickMainMenu;

        private void Awake()
        {
            _forceFillImage = forceSlider.fillRect.GetComponent<Image>();
            GameManager.Instance.Spawner.OnCueSpawnComplete += handler =>
            {
                handler.OnForceScale += UpdateForce;
                handler.OnShot += () => replayButton.gameObject.SetActive(false);
            };
            GameManager.Instance.BallManager.OnBallReposition += () => replayButton.gameObject.SetActive(true);
            GameManager.Instance.BallManager.OnBallsStopped += _ => replayButton.gameObject.SetActive(true);
            GameManager.Instance.OnPointsChanged += i => score.text = i.ToString();
            GameManager.Instance.OnShotsChanged += i => shots.text = i.ToString();
            GameManager.Instance.OnPlaytimeChanged += f =>
            {
                var timeSpan = TimeSpan.FromSeconds(f);
                playtime.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
            };
            GameManager.Instance.OnGameStateChanged += GameStateChanged;
            
            startButton.onClick.AddListener(() => OnClickStart?.Invoke());
            replayButton.onClick.AddListener(() => OnClickReplay?.Invoke());
            mainMenuButton.onClick.AddListener(() => OnClickMainMenu?.Invoke());
            newGameButton.onClick.AddListener(() => OnClickNewGame?.Invoke());
            ResetForceSlider();
        }

        private void GameStateChanged(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Play:
                    replayButton.gameObject.SetActive(true);
                    startButton.gameObject.SetActive(false);
                    break;
                case GameState.Reset:
                    ResetForceSlider();
                    
                    startButton.gameObject.SetActive(true);
                    gamePlayPanel.SetActive(true);
                    gameOverPanel.SetActive(false);
                    
                    finalShots.text = string.Empty;
                    finalScore.text = string.Empty;
                    finalPlaytime.text = string.Empty;
                    break;
                case GameState.Won:
                    gamePlayPanel.SetActive(false);
                    gameOverPanel.SetActive(true);

                    finalShots.text = shots.text;
                    finalScore.text = score.text;
                    finalPlaytime.text = playtime.text;
                    break;
                case GameState.Replay:
                    replayButton.gameObject.SetActive(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameState), gameState, null);
            }
        }

        private void UpdateForce(float value)
        {
            forceSlider.value = value;

            if (value < forceSlider.maxValue * 0.5f)
                _forceFillImage.color = Color.green;
            else if (value < forceSlider.maxValue * 0.90f)
                _forceFillImage.color = Color.yellow;
            else
                _forceFillImage.color = Color.red;
        }

        private void ResetForceSlider()
        {
            forceSlider.value = 0;
            _forceFillImage.color = Color.green;
        }
    }
}