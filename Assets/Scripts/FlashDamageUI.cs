using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlashDamageUI : MonoBehaviour
{
    public Image flashImage;
    public float flashDuration = 0.2f;
    public float maxAlpha = 0.6f;

    Coroutine flashRoutine;

    void Awake()
    {
        if (flashImage == null)
            flashImage = GetComponent<Image>();

        flashImage.color = new Color(1, 0, 0, 0);
        gameObject.SetActive(false);
    }

    public void Flash()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        gameObject.SetActive(true);
        flashRoutine = StartCoroutine(FlashCoroutine());
    }

    IEnumerator FlashCoroutine()
    {
        float t = 0f;

        // Fade IN
        while (t < flashDuration)
        {
            t += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(0, maxAlpha, t / flashDuration);
            flashImage.color = new Color(1, 0, 0, a);
            yield return null;
        }

        t = 0f;

        // Fade OUT
        while (t < flashDuration)
        {
            t += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(maxAlpha, 0, t / flashDuration);
            flashImage.color = new Color(1, 0, 0, a);
            yield return null;
        }

        flashImage.color = new Color(1, 0, 0, 0);
        gameObject.SetActive(false);
    }
}
