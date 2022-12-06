using System;
using _Project.Scripts.Audio;
using UnityEngine;

namespace _Project.Scripts.Balls
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;

        public bool Moving { get; private set; }
        
        public event Action OnStop;
        
        private Vector3 _startPos;
        
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
            Moving = false;
            OnStop?.Invoke();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision == null)
                return;

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayHitClip(rb.velocity.magnitude);
        }
        
        public void ResetPosition()
        {
            rb.velocity = Vector3.zero;
            transform.position = _startPos;
        }
    }
}