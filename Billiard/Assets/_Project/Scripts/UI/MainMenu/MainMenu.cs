using System;
using _Project.Scripts.Save;
using TMPro;
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

        [SerializeField] private GameObject lastScore;
        [SerializeField] private TMP_Text shots;
        [SerializeField] private TMP_Text score;
        [SerializeField] private TMP_Text playTime;

        internal event Action OnSettings;

        private void Awake()
        {
            lastScore.SetActive(false);
        }

        private void Start()
        {
            startButton.onClick.AddListener(() => SceneManager.LoadScene("Game"));
            settingsButton.onClick.AddListener(() => OnSettings?.Invoke());
            exitButton.onClick.AddListener(Application.Quit);
            SaveManager.Instance.OnSaveFileLoaded += LoadLastGame;
        }

        private void LoadLastGame(SaveStat saveStat)
        {
            shots.text = saveStat.Shots.ToString();
            score.text = saveStat.Score.ToString();
            var timeSpan = TimeSpan.FromSeconds(saveStat.PlayTime);
            playTime.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
            
            lastScore.SetActive(true);
        }
    }
}
