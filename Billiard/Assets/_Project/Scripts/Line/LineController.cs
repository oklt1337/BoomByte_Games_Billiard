using System;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace _Project.Scripts.Line
{
    public class LineController : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;

        private readonly Vector3[] _pointsOfInterest = new Vector3[2];
        
        private void Awake()
        {
            var gameManager = GameManager.Instance;
            gameManager.Spawner.OnCueSpawnComplete +=
                handler => handler.OnRotate += (direction) => { SetPoint(direction, 0); };
            gameManager.Spawner.OnCueSpawnComplete +=
                handler => handler.OnShot += () => lineRenderer.enabled = false;
            gameManager.OnGameStateChanged += OnGameStateChange;
            gameManager.BallManager.OnBallsStopped += ball =>
            {
                lineRenderer.enabled = true;
                SetPoint(ball.transform.position, 1);
            };
        }

        private void Update()
        {
            if (!lineRenderer.enabled)
                return;

            CalculatePoints();
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

        private void SetPoint(Vector3 point, int index)
        {
            if (index >= _pointsOfInterest.Length)
                return;
            _pointsOfInterest[index] = point;
        }

        private void CalculatePoints()
        {
            if (_pointsOfInterest == null)
                return;
            const float radius = 0.03514923f;
            
            if (!Physics.CapsuleCast(_pointsOfInterest[1], _pointsOfInterest[1], radius, _pointsOfInterest[0],
                    out var sphereHit)) return;
            var ray = new Ray(_pointsOfInterest[1], _pointsOfInterest[0]);
            if (sphereHit.collider.tag is "CueBall" or "RedBall" or "YellowBall")
            {
                if (!Physics.Raycast(ray, out var hit)) 
                    return;
                    
                lineRenderer.startColor = Color.red;
                lineRenderer.endColor = Color.red;

                lineRenderer.SetPosition(0, _pointsOfInterest[1]);
                lineRenderer.SetPosition(1, hit.point);
            }
            else
            {
                if (!Physics.Raycast(ray, out var hit)) 
                    return;
                lineRenderer.startColor = Color.blue;
                lineRenderer.endColor = Color.blue;

                lineRenderer.SetPosition(0, _pointsOfInterest[1]);
                lineRenderer.SetPosition(1, hit.point);
            }
        }
    }
}