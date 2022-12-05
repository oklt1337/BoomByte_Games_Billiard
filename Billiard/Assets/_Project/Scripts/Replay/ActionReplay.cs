using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Replay
{
    public class ActionReplay : MonoBehaviour
    {
        private readonly List<ActionReplayRecord> _actionReplaysRecords = new();
        private bool _isInReplayMode;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                _isInReplayMode = !_isInReplayMode;

                if (_isInReplayMode)
                {
                    SetTransform(0);
                }
                else
                {
                    SetTransform(1);
                }
            }
        }

        private void FixedUpdate()
        {
            if (_isInReplayMode)
                return;
            
            var myTransform = transform;
            _actionReplaysRecords.Add(new ActionReplayRecord
                { Position = myTransform.position, Rotation = myTransform.rotation });
        }

        private void SetTransform(int index)
        {
            var myTransform = transform;
            var actionReplayRecord = _actionReplaysRecords[index];
            myTransform.position = actionReplayRecord.Position;
            myTransform.rotation = actionReplayRecord.Rotation;
        }
    }
}