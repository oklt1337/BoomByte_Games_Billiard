using System;
using _Project.Scripts.Balls;
using _Project.Scripts.Cue;
using UnityEngine;

namespace _Project.Scripts.Spawning
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject whiteBallPrefab;
        [SerializeField] private GameObject yellowBallPrefab;
        [SerializeField] private GameObject redBallPrefab;
        [SerializeField] private GameObject cuePrefab;
    
        [SerializeField] private Vector3 whiteSpawnPos;
        [SerializeField] private Vector3 yellowSpawnPos;
        [SerializeField] private Vector3 redSpawnPos;
        [SerializeField] private Vector3 cueSpawnPos;

        private GameObject _whiteBall;
        private GameObject _yellowBall;
        private GameObject _redBall;
        private GameObject _cue;

        public event Action<CueBall> OnBallSpawnComplete;
        public event Action<CueHandler> OnCueSpawnComplete;

        private void Awake()
        {
            GameManager.Instance.OnGameStateChanged += DeleteBalls;
            GameManager.Instance.OnGameStateChanged += SpawnBalls;
        }

        private void DeleteBalls(GameState gameState)
        {
            if (gameState != GameState.Reset)
                return;

            if (_whiteBall != null)
                Destroy(_whiteBall);
            if (_yellowBall != null)
                Destroy(_yellowBall);
            if (_redBall != null)
                Destroy(_redBall);
            if (_cue != null)
                Destroy(_cue);
        }

        private void SpawnBalls(GameState gameState)
        {
            if (gameState != GameState.Reset)
                return;
            
            _whiteBall = Instantiate(whiteBallPrefab, whiteSpawnPos, Quaternion.identity);
            _yellowBall = Instantiate(yellowBallPrefab, yellowSpawnPos, Quaternion.identity);
            _redBall = Instantiate(redBallPrefab, redSpawnPos, Quaternion.identity);
            _cue = Instantiate(cuePrefab, cueSpawnPos, cuePrefab.transform.rotation);
            
            OnBallSpawnComplete?.Invoke(_whiteBall.GetComponent<CueBall>());
            OnCueSpawnComplete?.Invoke(_cue.GetComponent<CueHandler>());
        }
    }
}
