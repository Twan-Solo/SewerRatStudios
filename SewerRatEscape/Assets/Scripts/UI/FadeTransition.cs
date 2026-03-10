using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeTransition : MonoBehaviour
{
    [Header("Fade Settings")]
    public Image fadeImage;
    public float fadeDuration = 1f;
    public float displayTime = 0f; // optional delay after fade-in
    public bool fadeOutAfterLoad = true;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip fadeSound;

    private bool isFading = false;

    private void Awake()
    {
        // Ensure fade image exists
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(false);
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;

            // Make sure it renders on top
            Canvas canvas = fadeImage.GetComponentInParent<Canvas>();
            if (canvas != null)
            {
                canvas.overrideSorting = true;
                canvas.sortingOrder = 1000;
            }
        }
    }

    public void FadeToScene(string sceneName)
    {
        if (isFading) return;

        if (audioSource != null && fadeSound != null)
            audioSource.PlayOneShot(fadeSound);

        StartCoroutine(FadeAndLoadScene(sceneName));
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        isFading = true;

        if (fadeImage != null)
            fadeImage.gameObject.SetActive(true);

        // Fade in
        float timer = 0f;
        Color c = fadeImage.color;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            c.a = Mathf.Clamp01(timer / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }
        c.a = 1f;
        fadeImage.color = c;

        // Optional display delay
        if (displayTime > 0f)
            yield return new WaitForSecondsRealtime(displayTime);

        // Load next scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = true;
        while (!asyncLoad.isDone)
            yield return null;

        // Optional fade out after scene loads
        if (fadeOutAfterLoad && fadeImage != null)
        {
            timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.unscaledDeltaTime;
                c.a = Mathf.Clamp01(1f - timer / fadeDuration);
                fadeImage.color = c;
                yield return null;
            }
            c.a = 0f;
            fadeImage.color = c;
            fadeImage.gameObject.SetActive(false);
        }

        isFading = false;
    }
}
