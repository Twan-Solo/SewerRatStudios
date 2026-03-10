using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeTransition : MonoBehaviour
{
    [Header("Fade Settings")]
    public Image fadeImage;
    public float fadeDuration = 1f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip fadeSound;

    private bool isFading = false;

    private void Awake()
    {
        // Start invisible
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }

    public void FadeToScene(int sceneIndex)
    {
        if (isFading) return;

        if (audioSource != null && fadeSound != null)
            audioSource.PlayOneShot(fadeSound);

        StartCoroutine(FadeInAndLoad(sceneIndex));
    }

    public void FadeToScene(string sceneName)
    {
        if (isFading) return;

        if (audioSource != null && fadeSound != null)
            audioSource.PlayOneShot(fadeSound);

        StartCoroutine(FadeInAndLoad(sceneName));
    }

    private IEnumerator FadeInAndLoad(int sceneIndex)
    {
        isFading = true;
        yield return FadeRoutine();
        SceneManager.LoadScene(sceneIndex);
    }

    private IEnumerator FadeInAndLoad(string sceneName)
    {
        isFading = true;
        yield return FadeRoutine();
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator FadeRoutine()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;

            if (fadeImage != null)
            {
                Color c = fadeImage.color;
                c.a = Mathf.Clamp01(timer / fadeDuration);
                fadeImage.color = c;
            }

            yield return null;
        }

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 1f;
            fadeImage.color = c;
        }
    }
}
