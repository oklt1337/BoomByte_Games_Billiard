using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Project.Scripts.UI.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button exitButton;

        internal Action OnSettings;

        private void Start()
        {
            startButton.onClick.AddListener(() => SceneManager.LoadScene("Game"));
            settingsButton.onClick.AddListener(() => OnSettings?.Invoke());
            exitButton.onClick.AddListener(Application.Quit);
        }
    }
}
