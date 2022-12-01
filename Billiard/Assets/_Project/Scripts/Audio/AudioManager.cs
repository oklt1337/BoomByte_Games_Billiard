using UnityEngine;
using UnityEngine.Audio;

namespace _Project.Scripts.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        
        [SerializeField] private AudioMixer masterMixer;
        
        private const int MinVolume = -80;
        private const int MaxVolume = 0;
        
        private void Awake()
        {
            // If there is an instance, and it's not me, delete myself.
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
        }

        public void SetMasterVolume(float value)
        {
            // make sure volume is in bound
            value = value switch
            {
                > MaxVolume => MaxVolume,
                < MinVolume => MinVolume,
                _ => value
            };

            masterMixer.SetFloat("MasterVolume", value);
        }
    }
}
