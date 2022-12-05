using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Save
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }

        private SaveStat _lastSave;
        private bool _loadedFiles;

        private readonly string _path = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Billiard");

        private const string FileName = "saveFile.json";

        public event Action<SaveStat> OnSaveFileLoaded;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
        }

        private void Start()
        {
            OnSaveFileLoaded += _ => _loadedFiles = true;
            SceneManager.sceneLoaded += (scene, _) =>
            {
                if (scene.buildIndex == 0 && _loadedFiles)
                    OnSaveFileLoaded?.Invoke(_lastSave);
            };
            
            LoadSaveFile();
        }

        public void SaveStats(SaveStat saveStat)
        {
            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);

            File.WriteAllText(Path.Combine(_path, FileName),
                JsonConvert.SerializeObject(saveStat, Formatting.Indented));
            _lastSave = saveStat;
        }

        private void LoadSaveFile()
        {
            if (!CheckForSaveFile())
            {
                SceneManager.LoadScene("MainMenu");
                return;
            }
                

            var filePath = Path.Combine(_path, FileName);
            _lastSave = JsonConvert.DeserializeObject<SaveStat>(File.ReadAllText(filePath));

            SceneManager.LoadScene("MainMenu");
        }

        private bool CheckForSaveFile()
        {
            if (!Directory.Exists(_path))
                return false;

            var filePath = Path.Combine(_path, FileName);
            return File.Exists(filePath);
        }
    }

    [Serializable]
    public struct SaveStat
    {
        public SaveStat(int score, int shots, float playTime)
        {
            Score = score;
            Shots = shots;
            PlayTime = playTime;
        }

        public int Score { get; }
        public int Shots { get; }
        public float PlayTime { get; }
    }
}