using UnityEngine;
using TMPro;
using System.Collections;

public class StoryScroller : MonoBehaviour
{
    [Header("Referencias")]
    public RectTransform textRect; // rect transform del TMP_Text
    public RectTransform viewportRect; // rect transform del área visible (mask)
    public float duration = 30f; // tiempo que tarda en recorrer

    void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(ScrollRoutine());
    }

    IEnumerator ScrollRoutine()
    {
        float contentHeight = textRect.rect.height;
        float viewportHeight = viewportRect.rect.height;
        //float distance = contentHeight + viewportHeight; // distancia total a recorrer

        float startY = -(viewportHeight / 2f + contentHeight / 2f);
        float endY = (viewportHeight / 2f + contentHeight / 2f);

        Vector2 start = new Vector2(textRect.anchoredPosition.x, startY);
        Vector2 end = new Vector2(textRect.anchoredPosition.x, endY);

        float t = 0f;
        textRect.anchoredPosition = start;

        while (t < duration)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / duration);
            textRect.anchoredPosition = Vector2.Lerp(start, end, p);
            yield return null;
        }
    }

    public void VolverMenu() => UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
}
