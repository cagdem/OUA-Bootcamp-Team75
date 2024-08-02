using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public SettingsMenuController settingsMenuController; // Reference to SettingsMenuController

    public void StartGame()
    {
        SceneManager.LoadScene("MainScene"); // Replace "MainScene" with the name of your main game scene
    }

    public void OpenSettings()
    {
        settingsMenuController.OpenSettingsMenu(); // Open the settings menu
    }
}
