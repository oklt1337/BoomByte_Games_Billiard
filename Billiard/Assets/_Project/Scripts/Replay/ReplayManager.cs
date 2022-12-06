using UnityEngine;

namespace _Project.Scripts.Replay
{
    public class ReplayManager : MonoBehaviour
    {
        private int _replayDone;

        private void Awake()
        {
            var spawner = GameManager.Instance.Spawner;
            spawner.OnCueBallSpawnComplete += ball =>
            {
                var cueBall = ball.GetComponent<ActionReplay>();
                cueBall.OnReplayDone += CheckForReplayFinish;
            };
            spawner.OnYellowBallSpawnComplete += ball =>
            {
                var yellowBall = ball.GetComponent<ActionReplay>();
                yellowBall.OnReplayDone += CheckForReplayFinish;
            };
            spawner.OnRedBallSpawnComplete += ball =>
            {
                var redBall = ball.GetComponent<ActionReplay>();
                redBall.OnReplayDone += CheckForReplayFinish;
            };

            GameManager.Instance.OnGameStateChanged += state =>
            {
                if (state == GameState.Replay)
                    _replayDone = 0;
            };
        }

        private void CheckForReplayFinish()
        {
            _replayDone++;
            if (_replayDone == 3)
                GameManager.Instance.ReplayFinished();
        }
    }
}