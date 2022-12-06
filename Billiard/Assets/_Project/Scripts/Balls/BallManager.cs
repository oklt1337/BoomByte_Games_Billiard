using System;
using UnityEngine;

namespace _Project.Scripts.Balls
{
    public class BallManager : MonoBehaviour
    {
        private CueBall _cueBall;
        private Ball _yellowBall;
        private Ball _redBall;

        private int _stoppedBalls;
        public event Action<CueBall> OnBallsStopped;
        public event Action OnBallReposition;

        private void Awake()
        {
            var spawner = GameManager.Instance.Spawner;
            spawner.OnCueBallSpawnComplete += ball =>
            {
                _cueBall = ball;
                OnBallsStopped?.Invoke(_cueBall);
                _cueBall.OnStop += CheckBalls;
            };
            spawner.OnYellowBallSpawnComplete += ball =>
            {
                _yellowBall = ball;
                _yellowBall.OnStop += CheckBalls;
            };
            spawner.OnRedBallSpawnComplete += ball =>
            {
                _redBall = ball;
                _redBall.OnStop += CheckBalls;
            };

            GameManager.Instance.OnPointsChanged += points =>
            {
                if (points <= 0)
                    return;
                
                ResetBallPositions();
                OnBallReposition?.Invoke();
            };
        }

        private void CheckBalls()
        {
            if (_yellowBall.Moving || _redBall.Moving || _cueBall.Moving)
                return;

            if (!_cueBall.CheckWin())
                OnBallsStopped?.Invoke(_cueBall);
        }

        private void ResetBallPositions()
        {
            _cueBall.ResetPosition();
            _yellowBall.ResetPosition();
            _redBall.ResetPosition();
        }
    }
}