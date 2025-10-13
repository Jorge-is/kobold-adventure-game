using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PasswordUnlock : MonoBehaviour
{
    public TMP_InputField inputField;
    public TMP_Text messageText;
    public string correctPassword = "unity123"; // cambiar desde inspector
    public string unlockSceneName = "Level3"; // escena que se desbloquea


    public void Validar()
    {
        if (inputField.text.Trim() == correctPassword)
        {
            string key = "unlocked_" + unlockSceneName;
            PlayerPrefs.SetInt(key, 1);
            PlayerPrefs.Save();
            messageText.text = "¡Acceso concedido!";
            // Opcional: cargar la escena desbloqueada
            SceneManager.LoadScene(unlockSceneName);
        }
        else
        {
            messageText.text = "Contraseña incorrecta.";
        }
    }


    public void VolverMenu() => SceneManager.LoadScene("MainMenu");
}
