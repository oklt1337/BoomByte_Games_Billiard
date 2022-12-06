using System;
using UnityEngine;

namespace _Project.Scripts.Cue
{
    public class CueHandler : MonoBehaviour
    {
        [SerializeField] private GameObject model;
        [SerializeField] private float sensitivity = 50f;
        [SerializeField] private float intensityMultiplier = 25f;
        [SerializeField] private float maxIntensity = 150f;

        private Camera _camera;
        private Transform _cueBall;
        private float _time;
        private bool _isMovable;
        public event Action<float> OnForceScale;
        public event Action<Vector3> OnRotate;
        public event Action OnShot;

        private void Awake()
        {
            GameManager.Instance.Spawner.OnCueBallSpawnComplete += ball =>
            {
                _cueBall = ball.transform;
                Reposition();
            };
            GameManager.Instance.BallManager.OnBallsStopped += ball =>
            {
                _cueBall = ball.transform;
                if (GameManager.Instance.GameState == GameState.Won)
                    return;
                _isMovable = true;
                model.SetActive(true);
            };
            GameManager.Instance.BallManager.OnBallReposition += () =>
            {
                Reposition();
                if (GameManager.Instance.GameState == GameState.Won)
                    return;
                _isMovable = true;
                model.SetActive(true);
            };
            GameManager.Instance.OnGameStateChanged += OnOnGameStateChanged;
        }

        private void OnOnGameStateChanged(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Won:
                case GameState.Replay:
                    _isMovable = false;
                    model.SetActive(false);
                    break;
                case GameState.Play:
                    _isMovable = true;
                    model.SetActive(true);
                    break;
                case GameState.Init:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameState), gameState, null);
            }
        }

        private void Start()
        {
            _camera = Camera.main;
            _isMovable = false;
            model.SetActive(false);
        }

        private void Update()
        {
            if (!_isMovable)
                return;

            IntensityCheck();
            Shoot();

            if (ViewPortCheck())
                return;

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                Rotation(1f);
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                Rotation(-1f);
        }

        private void IntensityCheck()
        {
            if (!Input.GetKey(KeyCode.Space))
                return;

            _time += Time.deltaTime;

            if (_time * intensityMultiplier >= maxIntensity)
                OnForceScale?.Invoke(maxIntensity);
            else
                OnForceScale?.Invoke(_time * intensityMultiplier);
        }

        private void Shoot()
        {
            if (!Input.GetKeyUp(KeyCode.Space))
                return;

            var intensity = intensityMultiplier * _time;
            if (intensity > maxIntensity)
                intensity = maxIntensity;

            var cueBallPosition = _cueBall.position;
            var tipPosition = transform.position;
            var origin = new Vector3(tipPosition.x, cueBallPosition.y, tipPosition.z);
            var dir = new Vector3(cueBallPosition.x - tipPosition.x, 0,
                cueBallPosition.z - tipPosition.z);

            var ray = new Ray(origin, dir);
            if (Physics.Raycast(ray, out var hit, 1f))
            {
                if (!hit.collider.CompareTag("CueBall"))
                    return;

                var direction = hit.point - origin;
                hit.rigidbody.AddForceAtPosition(direction.normalized * intensity, hit.point);

                model.SetActive(false);
                _isMovable = false;
                OnShot?.Invoke();
            }

            _time = 0f;
        }


        private void Rotation(float direction)
        {
            var cueBallPosition = _cueBall.position;
            var myTransform = transform;
            myTransform.RotateAround(
                cueBallPosition, Vector3.up, Time.deltaTime * sensitivity * direction);

            var tipPosition = myTransform.position;
            var dir = new Vector3(cueBallPosition.x - tipPosition.x, 0,
                cueBallPosition.z - tipPosition.z);
            OnRotate?.Invoke(dir);
        }

        private bool ViewPortCheck()
        {
            var view = _camera.ScreenToViewportPoint(Input.mousePosition);
            return view.x is < 0 or > 1 || view.y is < 0 or > 1;
        }

        private void Reposition()
        {
            var myTransform = transform;
            var ballPosition = _cueBall.position;
            var rotation = Quaternion.Euler(0,0,0);

            myTransform.position = new Vector3(ballPosition.x, ballPosition.y, ballPosition.z - 0.05f);
            transform.rotation = rotation;
            
            var tipPosition = myTransform.position;
            var dir = new Vector3(ballPosition.x - tipPosition.x, 0,
                ballPosition.z - tipPosition.z);
            
            OnRotate?.Invoke(dir);
        }
    }
}