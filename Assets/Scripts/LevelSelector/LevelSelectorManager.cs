using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelSelectorManager : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject levelButtonPrefab;
    public RectTransform buttonsContainer;

    [Header("Configuración de Niveles")]
    public int unlockedLevels = 3;

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

            TMP_Text label = newButton.GetComponentInChildren<TMP_Text>();
            Button btn = newButton.GetComponentInChildren<Button>();
            Image lockIcon = newButton.transform.Find("LockIcon").GetComponent<Image>();

            label.text = level.levelNumber.ToString();

            bool isUnlocked = level.levelNumber <= unlockedLevels;
            btn.interactable = isUnlocked;
            if (lockIcon != null) lockIcon.gameObject.SetActive(!isUnlocked);

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
