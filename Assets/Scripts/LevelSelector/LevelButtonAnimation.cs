using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private RectTransform rect;
    private Button button;

    // Escalas
    private Vector3 normalScale;
    private Vector3 hoverScale;
    private Vector3 clickScale;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        button = GetComponent<Button>();

        normalScale = Vector3.one;
        hoverScale = Vector3.one * 1.08f;
        clickScale = Vector3.one * 0.92f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!button.interactable) return; // No animar si está bloqueado
        StopAllCoroutines();
        StartCoroutine(ScaleTo(hoverScale, 0.08f));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!button.interactable) return;
        StopAllCoroutines();
        StartCoroutine(ScaleTo(normalScale, 0.08f));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!button.interactable) return;
        StopAllCoroutines();
        StartCoroutine(ClickAnimation());
    }

    private System.Collections.IEnumerator ScaleTo(Vector3 target, float duration)
    {
        Vector3 start = rect.localScale;
        float t = 0;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            rect.localScale = Vector3.Lerp(start, target, t / duration);
            yield return null;
        }

        rect.localScale = target;
    }

    private System.Collections.IEnumerator ClickAnimation()
    {
        yield return ScaleTo(clickScale, 0.06f);
        yield return ScaleTo(hoverScale, 0.08f);
    }
}