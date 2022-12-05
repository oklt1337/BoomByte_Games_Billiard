using System;
using _Project.Scripts.Audio;
using UnityEngine;

namespace _Project.Scripts.Balls
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;

        private bool _moving;
        public event Action OnStop;

        private void Update()
        {
            if (rb.velocity != Vector3.zero)
                _moving = true;

            if (rb.velocity != Vector3.zero || !_moving)
                return;

            _moving = false;
            OnStop?.Invoke();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision == null)
                return;

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayHitClip(rb.velocity.magnitude);
        }
    }
}