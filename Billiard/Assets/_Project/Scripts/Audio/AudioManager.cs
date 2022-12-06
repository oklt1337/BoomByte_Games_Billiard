using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        
        [Header("Mixer")]
        [SerializeField] private AudioMixer masterMixer;
        [Header("Audio Clips")]
        [SerializeField] private List<AudioClip> hitAudioClips = new();
        [SerializeField] private AudioClip backgroundMusic;
        [SerializeField] private AudioClip victoryClip;
        
        [Header("Audio Source")]
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource musicSource;

        private const int MinVolume = -80;
        private const int MaxVolume = 0;

        private float _currentPos;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
        }

        private void Start()
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
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
            masterMixer.SetFloat("SFXVolume", value);
            masterMixer.SetFloat("MusicVolume", value);
        }

        public void PlayHitClip(float force)
        {
            var clip = Random.Range(0, hitAudioClips.Count);
            sfxSource.PlayOneShot(hitAudioClips[clip], force);
        }
        public void PlayVictory()
        {
            sfxSource.PlayOneShot(victoryClip);
        }
    }
}
