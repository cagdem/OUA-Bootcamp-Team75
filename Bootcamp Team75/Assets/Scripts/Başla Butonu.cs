using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene"); // Replace "MainScene" with the name of your main game scene
    }

    public void OpenSettings()
    {
        // Implement settings functionality here
    }
}
