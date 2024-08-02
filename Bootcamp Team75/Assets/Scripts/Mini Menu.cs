using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenuController : MonoBehaviour
{
    public GameObject settingsMenuUI;
    public GameObject howToPlayUI;

    public void OpenSettingsMenu()
    {
        settingsMenuUI.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    public void CloseSettingsMenu()
    {
        settingsMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }

    public void MuteSound()
    {
        AudioListener.volume = 0f;
    }

    public void UnmuteSound()
    {
        AudioListener.volume = 1f;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene("Menu"); // Replace with your main menu scene name
    }

    public void ShowHowToPlay()
    {
        howToPlayUI.SetActive(true);
    }

    public void HideHowToPlay()
    {
        howToPlayUI.SetActive(false);
    }


}
