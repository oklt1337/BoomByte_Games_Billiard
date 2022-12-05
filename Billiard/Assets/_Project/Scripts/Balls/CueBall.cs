using System;
using _Project.Scripts.Audio;
using UnityEngine;

namespace _Project.Scripts.Balls
{
    public class CueBall : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;

        [SerializeField] private bool yellow;
        [SerializeField] private bool red;

        public event Action OnWon;

        private void Update()
        {
            if (Input.GetKey(KeyCode.Q))
            {
                OnWon?.Invoke();
            }
            
            if (rb.velocity == Vector3.zero)
            {
                yellow = false;
                red = false;
            }
            else
                CheckWin();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision == null)
                return;

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayHitClip(rb.velocity.magnitude);

            if (collision.gameObject.CompareTag("YellowBall"))
            {
                yellow = true;
            }
            else if (collision.gameObject.CompareTag("RedBall"))
            {
                red = true;
            }
        }

        private void CheckWin()
        {
            if (red && yellow)
                OnWon?.Invoke();
        }
    }
}