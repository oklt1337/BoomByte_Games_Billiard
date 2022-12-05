using _Project.Scripts.Audio;
using UnityEngine;

namespace _Project.Scripts.Balls
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision == null)
                return;
            
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayHitClip(rb.velocity.magnitude);
        }
    }
}