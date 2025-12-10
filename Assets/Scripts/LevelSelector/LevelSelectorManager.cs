using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelSelectorManager : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject levelButtonPrefab;
    public RectTransform buttonsContainer;

    [Header("Configuración de Niveles")]
    public int unlockedLevels = 3;

    [Header("Sprites de cada nivel")]
    public List<Sprite> levelSprites;

    [Header("Posiciones de niveles")]
    public List<LevelButtonData> levels = new List<LevelButtonData>();

    void Start()
    {
        if (levelButtonPrefab == null || buttonsContainer == null)
        {
            Debug.LogError("Faltan referencias en el inspector.");
            return;
        }

        GenerateLevelButtons();
    }

    void GenerateLevelButtons()
    {
        foreach (LevelButtonData level in levels)
        {
            GameObject newButton = Instantiate(levelButtonPrefab, buttonsContainer);

            // Posición manual
            RectTransform rt = newButton.GetComponent<RectTransform>();
            rt.anchoredPosition = level.position;

            Button btn = newButton.GetComponent<Button>();

            // Asignar imagen del nivel
            Image mainImage = newButton.GetComponentInChildren<Image>();

            bool isUnlocked = level.levelNumber <= unlockedLevels;
            btn.interactable = isUnlocked;

            // Cambia de imagen según el nivel
            int spriteIndex = level.levelNumber - 1;

            if (mainImage != null && spriteIndex >= 0 && spriteIndex < levelSprites.Count)
            {
                mainImage.sprite = levelSprites[spriteIndex];
            }
            else
            {
                Debug.LogWarning("No hay sprite asignado para el nivel " + level.levelNumber);
            }

            // Escala de grises si está bloqueado
            if (mainImage != null)
            {
                mainImage.color = isUnlocked
                    ? Color.white
                    : new Color(0.35f, 0.35f, 0.35f, 1f);
            }

            // Click para carga el nivel
            int levelIndex = level.levelNumber;
            btn.onClick.AddListener(() => LoadLevel(levelIndex));
        }
    }

    void LoadLevel(int index)
    {
        SceneManager.LoadScene("Level" + index);
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
