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

        public int StoppedBalls
        {
            get => _stoppedBalls;
            set
            {
                _stoppedBalls = value;
                if (_stoppedBalls == 3)
                    OnBallsStopped?.Invoke(_cueBall);
            } 
        }

        private void Awake()
        {
            var spawner = GameManager.Instance.Spawner;
            spawner.OnCueSpawnComplete += handler => handler.OnShot += () => StoppedBalls = 0;
            spawner.OnCueBallSpawnComplete += ball =>
            {
                _cueBall = ball;
                _cueBall.OnStop += () => StoppedBalls++;
            };
            spawner.OnYellowBallSpawnComplete += ball =>
            {
                _yellowBall = ball;
                _yellowBall.OnStop += () => StoppedBalls++;
            };
            spawner.OnRedBallSpawnComplete += ball =>
            {
                _redBall = ball;
                _redBall.OnStop += () => StoppedBalls++;
            };
        }
    }
}