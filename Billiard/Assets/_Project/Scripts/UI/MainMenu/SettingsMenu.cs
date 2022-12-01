using System;
using _Project.Scripts.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.MainMenu
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] private Button backButton;
        [SerializeField] private Slider masterVolume;

        internal Action OnBack;

        private void Start()
        {
            backButton.onClick.AddListener(() => OnBack?.Invoke());
            masterVolume.onValueChanged.AddListener(value => AudioManager.Instance.SetMasterVolume(value));
        }
    }
}
