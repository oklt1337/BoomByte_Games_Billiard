using UnityEngine;

namespace _Project.Scripts.UI.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private MainMenu mainMenu;
        [SerializeField] private SettingsMenu settingsMenu;

        private void Start()
        {
            mainMenu.OnSettings += OpenSettingsMenu;
            settingsMenu.OnBack += OpenMainMenu;
            OpenMainMenu();
        }

        private void OpenMainMenu()
        {
            ToggleMainMenu(true);
            ToggleSettingsMenu(false);
        }

        private void OpenSettingsMenu()
        {
            ToggleMainMenu(false);
            ToggleSettingsMenu(true);
        }
        
        private void ToggleMainMenu(bool state)
        {
            mainMenu.gameObject.SetActive(state);
        }
        
        private void ToggleSettingsMenu(bool state)
        {
            settingsMenu.gameObject.SetActive(state);
        }
    }
}
