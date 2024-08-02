using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniMenuController : MonoBehaviour
{
    public GameObject miniMenuUI; // Mini menü UI referansý
    public GameObject hintsBookUI; // Ýpuçlarý kitabý UI referansý

    // Mini menüyü açan fonksiyon
    public void OpenMiniMenu()
    {
        miniMenuUI.SetActive(true);
        Time.timeScale = 0f; // Oyunu duraklat
    }

    // Mini menüyü kapatan fonksiyon
    public void CloseMiniMenu()
    {
        miniMenuUI.SetActive(false);
        Time.timeScale = 1f; // Oyunu devam ettir
    }

    // Ýpuçlarý kitabýný açan fonksiyon
    public void OpenHintsBook()
    {
        hintsBookUI.SetActive(true);
        Time.timeScale = 0f; // Oyunu duraklat
    }

    // Ýpuçlarý kitabýný kapatan fonksiyon
    public void CloseHintsBook()
    {
        hintsBookUI.SetActive(false);
        Time.timeScale = 1f; // Oyunu devam ettir
    }

    // Ana menüye dön fonksiyonu
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Oyunu devam ettir
        SceneManager.LoadScene("Menu"); // Ana menü sahnesine dön
    }
}
