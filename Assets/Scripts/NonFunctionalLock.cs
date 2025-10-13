using UnityEngine;
using UnityEngine.SceneManagement;

public class NonFunctionalLock : MonoBehaviour
{
    public void VolverMenu() => SceneManager.LoadScene("MainMenu");
}
