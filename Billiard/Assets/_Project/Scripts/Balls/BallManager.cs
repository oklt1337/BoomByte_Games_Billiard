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
        public event Action OnBallRespawn;

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
                DestroyBalls();
                OnBallRespawn?.Invoke();
            };
        }

        private void CheckBalls()
        {
            if (!_yellowBall.Moving && !_redBall.Moving && !_cueBall.Moving)
                OnBallsStopped?.Invoke(_cueBall);
        }

        private void DestroyBalls()
        {
            if (_cueBall != null)
                Destroy(_cueBall.gameObject);
            if (_yellowBall != null)
                Destroy(_yellowBall.gameObject);
            if (_redBall != null)
                Destroy(_redBall);
            if (_redBall != null)
                Destroy(_redBall.gameObject);
        }
    }
}