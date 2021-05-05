using System.Collections;
using UnityEngine;

public class UITriggerZone : MonoBehaviour
{
    [SerializeField] private CanvasGroup UI;
    private Coroutine coroutine;
    private const float minAlpha = 0f;
    private const float maxAlpha = 0.8f;
    private const float speed = 0.7f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(FadeIn());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeIn()
    {
        while (UI.alpha < maxAlpha)
        {
            UI.alpha += speed * Time.deltaTime;
            if (UI.alpha > maxAlpha) 
            {
                UI.alpha = maxAlpha;
            }
            yield return null;
        }
    }
    private IEnumerator FadeOut()
    {
        while (UI.alpha > minAlpha)
        {
            UI.alpha -= speed * Time.deltaTime;
            yield return null;
        }
    }
}
