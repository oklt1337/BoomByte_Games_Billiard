using _Project.Scripts.Balls;
using UnityEngine;

namespace _Project.Scripts.Cameras
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Vector3 offset = new(0,1.5f,-1.5f);
        private Transform _followTransform;

        private void Awake()
        {
            GameManager.Instance.Spawner.OnBallSpawnComplete += SetFollowTarget;
        }

        private void LateUpdate()
        {
            Follow();
        }
        
        private void SetFollowTarget(CueBall ball)
        {
            _followTransform = ball.transform;
        }

        private void Follow()
        {
            if (_followTransform == null)
                return;
            
            transform.position = _followTransform.position + offset;
        }
    }
}