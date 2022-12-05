using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Line
{
    public class LineController : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        private readonly List<Transform> _linePoints = new(3);


        private void Start()
        {
            var gameManager = GameManager.Instance;
            gameManager.Spawner.OnCueSpawnComplete +=
                handler => handler.OnRotate += t => SetPoint(t, 0);
            gameManager.Spawner.OnCueSpawnComplete +=
                handler => handler.OnShot += () => lineRenderer.enabled = false;
            gameManager.OnGameStateChanged += OnGameStateChange;
            gameManager.BallManager.OnBallsStopped += ball =>
            {
                lineRenderer.enabled = true;
                SetPoint(ball.transform, 1);
            };
        }

        private void Update()
        {
            if (!lineRenderer.enabled)
                return;
            if (_linePoints == null)
                return;

            for (var i = 0; i < _linePoints.Count; i++)
            {
                lineRenderer.SetPosition(i, _linePoints[i].position);
            }
        }

        private void OnGameStateChange(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Play:
                    lineRenderer.enabled = true;
                    break;
                case GameState.Reset:
                case GameState.Won:
                    lineRenderer.enabled = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameState), gameState, null);
            }
        }

        private void SetPoint(Transform point, int index)
        {
            _linePoints[index] = point;
        }
    }
}