using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Replay
{
    public class ActionReplay : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        
        private readonly List<ActionReplayRecord> _actionReplaysRecords = new();
        private bool _isInReplayMode;
        private bool _recordMode;
        private int _currenIndex;

        private Vector3 _startPos;

        public event Action OnReplayDone;

        private void Awake()
        {
            GameManager.Instance.OnGameStateChanged += OnOnGameStateChanged;
            GameManager.Instance.Spawner.OnCueSpawnComplete += handler => handler.OnShot += () =>
            {
                _actionReplaysRecords.Clear();
                _recordMode = true;
            };
            GameManager.Instance.BallManager.OnBallReposition += () =>
            {
                _recordMode = false;
            };
            GameManager.Instance.BallManager.OnBallsStopped += _ =>
            {
                _recordMode = false;
            };
            GameManager.Instance.BallManager.OnBallsStopped += _ =>
            {
                _startPos = transform.position;
            };
            GameManager.Instance.BallManager.OnBallReposition += () =>
            {
                _startPos = transform.position;
            };
        }

        private void OnOnGameStateChanged(GameState gameState)
        {
            _isInReplayMode = gameState == GameState.Replay;

            switch (gameState)
            {
                case GameState.Replay:
                    SetTransform(0);
                    rb.isKinematic = true;
                    break;
                case GameState.Play:
                    break;
                case GameState.Init:
                    break;
                case GameState.Won:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameState), gameState, null);
            }
        }

        private void FixedUpdate()
        {
            if (_recordMode)
            {
                var myTransform = transform;
                _actionReplaysRecords.Add(new ActionReplayRecord
                    { Position = myTransform.position, Rotation = myTransform.rotation });
            }
            else if (_isInReplayMode)
            {
                var nextIndex = _currenIndex + 1;
                if (nextIndex < _actionReplaysRecords.Count)
                {
                    SetTransform(nextIndex);
                }
            }
        }

        private void SetTransform(int index)
        {
            _currenIndex = index;
            
            var myTransform = transform;
            var actionReplayRecord = _actionReplaysRecords[index];
            myTransform.position = actionReplayRecord.Position;
            myTransform.rotation = actionReplayRecord.Rotation;

            if (_currenIndex != _actionReplaysRecords.Count - 1) 
                return;
            rb.isKinematic = false;
            transform.position = _startPos;
            OnReplayDone?.Invoke();
        }
    }
}