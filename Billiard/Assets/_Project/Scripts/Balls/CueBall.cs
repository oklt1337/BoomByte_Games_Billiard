using System;
using _Project.Scripts.Audio;
using UnityEngine;

namespace _Project.Scripts.Balls
{
    public class CueBall : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        
        private Vector3 _startPos;
        private bool _yellow;
        private bool _red;

        public bool Moving { get; private set; }
        public event Action OnWon;
        public event Action OnStop;
        
        private void Awake()
        {
            _startPos = transform.position;
        }

        private void Update()
        {
            if (rb.velocity != Vector3.zero)
                Moving = true;

            if (rb.velocity != Vector3.zero || !Moving) 
                return;

            if (!(_red && _yellow))
            {
                _yellow = false;
                _red = false;
            }
            Moving = false;
            OnStop?.Invoke();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision == null)
                return;

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayHitClip(rb.velocity.magnitude);

            if (collision.gameObject.CompareTag("YellowBall"))
            {
                _yellow = true;
            }
            else if (collision.gameObject.CompareTag("RedBall"))
            {
                _red = true;
            }
        }

        public bool CheckWin()
        {
            if (_red && _yellow) 
                OnWon?.Invoke();

            return _red && _yellow;
        }

        public void ResetPosition()
        {
            _yellow = false;
            _red = false;
            rb.velocity = Vector3.zero;
            transform.position = _startPos;
        }
    }
}