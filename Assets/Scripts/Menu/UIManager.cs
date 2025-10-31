using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Menús")]
    public GameObject optionsMenu;

    [Header("Audio del juego")]
    private AudioSource gameMusic; // Música de fondo del juego

    void Start()
    {
        // Obtener la referencia al AudioSource de la cámara principal
        if (Camera.main != null)
        {
            gameMusic = Camera.main.GetComponent<AudioSource>();
        }
    }

    public void OptionsMenu()
    {
        Time.timeScale = 0f; // Pausa el juego
        optionsMenu.SetActive(true);

        // Pausar la música de fondo si es necesario
        if (gameMusic != null && gameMusic.isPlaying)
        {
            gameMusic.Pause();
        }
    }

    public void Return()
    {
        Time.timeScale = 1f; // Reanuda el juego
        optionsMenu.SetActive(false);

        // Reanudar música de fondo
        if (gameMusic != null)
        {
            gameMusic.UnPause();
        }
    }

    public void AnotherOptions()
    {
        //Sound
        //Graphics
    }

    public void GoMainMenu()
    {
        Time.timeScale = 1f; // Asegura que el tiempo esté normal al volver al menú
        if (gameMusic != null)
        {
            gameMusic.Stop(); // Detiene la música actual al cambiar de escena
        }
        SceneManager.LoadScene("MainMenu");

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
