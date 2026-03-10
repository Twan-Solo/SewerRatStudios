using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeTransition : MonoBehaviour
{
    [Header("Fade Settings")]
    public Image fadeImage;           // Assign your fullscreen sprite here
    public float fadeDuration = 1f;   // Duration of fade-in

    [Header("Audio")]
    public AudioSource audioSource;   // Assign in inspector
    public AudioClip fadeSound;       // Sound to play when fade starts

    void Start()
    {
        // Make sure sprite starts invisible
        if (fadeImage != null)
            fadeImage.color = new Color(1, 1, 1, 0f);
    }

    // --- Overload for build index ---
    public void FadeToScene(int sceneIndex)
    {
        if (audioSource != null && fadeSound != null)
            audioSource.PlayOneShot(fadeSound);

        StartCoroutine(FadeInAndLoad(sceneIndex));
    }

    // --- Overload for scene name ---
    public void FadeToScene(string sceneName)
    {
        if (audioSource != null && fadeSound != null)
            audioSource.PlayOneShot(fadeSound);

        StartCoroutine(FadeInAndLoad(sceneName));
    }

    // --- Coroutine for int index ---
    private IEnumerator FadeInAndLoad(int sceneIndex)
    {
        yield return FadeInRoutine();
        SceneManager.LoadScene(sceneIndex);
    }

    // --- Coroutine for string name ---
    private IEnumerator FadeInAndLoad(string sceneName)
    {
        yield return FadeInRoutine();
        SceneManager.LoadScene(sceneName);
    }

    // --- Shared fade routine ---
    private IEnumerator FadeInRoutine()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime; // works even if game is paused

            if (fadeImage != null)
            {
                Color c = fadeImage.color;
                c.a = Mathf.Clamp01(timer / fadeDuration);
                fadeImage.color = c;
            }

            yield return null;
        }

        // Ensure fully visible at the end
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 1f;
            fadeImage.color = c;
        }

        // Make sure timeScale is reset in case we were paused
        Time.timeScale = 1f;
    }
}
