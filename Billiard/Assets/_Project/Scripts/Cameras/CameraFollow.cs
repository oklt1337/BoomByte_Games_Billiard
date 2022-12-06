using _Project.Scripts.Balls;
using UnityEngine;

namespace _Project.Scripts.Cameras
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Vector3 offset = new(0, 1.5f, -1.5f);
        private Transform _followTransform;

        private void Awake()
        {
            GameManager.Instance.Spawner.OnCueBallSpawnComplete += SetFollowTarget;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                FlipCamera(1);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                FlipCamera(0);
            }
        }

        private void LateUpdate()
        {
            Follow();
        }

        private void FlipCamera(int dir)
        {
            var eulerAngles = transform.eulerAngles;
            
            switch (dir)
            {
                case 0:
                    transform.rotation = Quaternion.Euler(eulerAngles.x, 0, eulerAngles.z);
                    if (offset.z < 0)
                        return;

                    offset = new Vector3(offset.x, offset.y, offset.z * -1f);
                    break;
                case 1:
                    offset = new Vector3(offset.x, offset.y, Mathf.Abs(offset.z));
                    transform.rotation = Quaternion.Euler(eulerAngles.x, 180, eulerAngles.z);
                    break;
            }
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