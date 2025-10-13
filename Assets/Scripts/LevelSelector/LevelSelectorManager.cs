using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelSelectorManager : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject levelButtonPrefab; // Prefab del botón de nivel
    public Transform panelPrefabs; // Panel donde se instanciarán los botones

    [Header("Configuración de Niveles")]
    public int totalLevels = 5; // Cantidad total de niveles
    public int unlockedLevels = 3; // Cantidad de niveles desbloqueados

    void Start()
    {
        if (levelButtonPrefab == null || panelPrefabs == null)
        {
            Debug.LogError("Faltan referencias: arrastra el prefab y el panel en el inspector.");
            return;
        }

        GenerateLevelButtons();
    }

    void GenerateLevelButtons()
    {
        for (int i = 1; i <= totalLevels; i++)
        {
            GameObject newButton = Instantiate(levelButtonPrefab, panelPrefabs);

            TMP_Text label = newButton.GetComponentInChildren<TMP_Text>();
            Button button = newButton.GetComponentInChildren<Button>();
            Image lockIcon = newButton.transform.Find("LockIcon").GetComponent<Image>();        

            label.text = "Nivel " + i;

            bool isUnlocked = (i <= unlockedLevels);
            if (lockIcon != null) lockIcon.gameObject.SetActive(!isUnlocked);
            button.interactable = isUnlocked;

            int levelIndex = i; // Captura para el listener
            button.onClick.AddListener(() => LoadLevel(levelIndex));
        }
    }

    void LoadLevel(int index)
    {
        string sceneName = "Level" + index; // "Level1", "Level2", etc.
        SceneManager.LoadScene(sceneName);
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
