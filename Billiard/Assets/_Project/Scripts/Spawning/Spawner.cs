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

        public event Action<CueBall> OnCueBallSpawnComplete;
        public event Action<Ball> OnYellowBallSpawnComplete;
        public event Action<Ball> OnRedBallSpawnComplete;
        public event Action<CueHandler> OnCueSpawnComplete;

        private void Awake()
        {
            GameManager.Instance.OnGameStateChanged += OnGameStateChange;
        }

        private void OnGameStateChange(GameState gameState)
        {
            if (gameState != GameState.Reset)
                return;
            
            if (_cue != null)
                Destroy(_cue);

            SpawnObjects();
        }

        private void SpawnObjects()
        {
            _whiteBall = Instantiate(whiteBallPrefab, whiteSpawnPos, Quaternion.identity);
            _yellowBall = Instantiate(yellowBallPrefab, yellowSpawnPos, Quaternion.identity);
            _redBall = Instantiate(redBallPrefab, redSpawnPos, Quaternion.identity);

            if (_cue == null)
            {
                _cue = Instantiate(cuePrefab, cueSpawnPos, cuePrefab.transform.rotation);
                OnCueSpawnComplete?.Invoke(_cue.GetComponent<CueHandler>());
            }
            
            OnCueBallSpawnComplete?.Invoke(_whiteBall.GetComponent<CueBall>());
            OnYellowBallSpawnComplete?.Invoke(_yellowBall.GetComponent<Ball>());
            OnRedBallSpawnComplete?.Invoke(_redBall.GetComponent<Ball>());
        }
    }
}
