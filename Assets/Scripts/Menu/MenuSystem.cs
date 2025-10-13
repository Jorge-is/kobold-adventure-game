using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    public void Jugar() => SceneManager.LoadScene("LevelSelector");
    public void Historia() => SceneManager.LoadScene("Story");

    public void Salir()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
