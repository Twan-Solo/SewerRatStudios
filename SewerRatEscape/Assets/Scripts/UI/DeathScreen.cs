using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathScreen : MonoBehaviour
{
    [Header("Death Screen Settings")]
    public Image deathImage;          // Assign your death image
    public float fadeDuration = 1f;   // Fade-in/out duration
    public float displayTime = 2f;    // Fully visible time
    public bool fadeOut = true;       // Fade out before hiding

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip deathSound;      // Optional sound

    public string mainMenuSceneName = "MainMenu";

    private void Awake()
    {
        // Keep this canvas alive across scene loads
        DontDestroyOnLoad(gameObject);

        if (deathImage != null)
        {
            deathImage.gameObject.SetActive(false);
            Color c = deathImage.color;
            c.a = 0f;
            deathImage.color = c;
        }
    }

    private void Update()
    {
        // Force canvas to stay on top while visible
        if (deathImage != null && deathImage.gameObject.activeSelf)
        {
            Canvas canvas = deathImage.GetComponentInParent<Canvas>();
            if (canvas != null)
            {
                canvas.overrideSorting = true;
                canvas.sortingOrder = 1000;
            }
        }
    }

    public void ShowDeathScreen()
    {
        if (deathImage != null)
            deathImage.gameObject.SetActive(true);

        // Play death sound if assigned
        if (audioSource != null && deathSound != null)
            audioSource.PlayOneShot(deathSound);

        // Start fade and display coroutine
        StartCoroutine(FadeAndLoadMenu());
    }

    private IEnumerator FadeAndLoadMenu()
    {
        if (deathImage == null)
        {
            // Fallback: just load main menu if no image
            if (SceneManager.GetActiveScene().name != mainMenuSceneName)
                SceneManager.LoadScene(mainMenuSceneName);
            yield break;
        }

        Color c = deathImage.color;
        c.a = 0f;
        deathImage.color = c;

        // --- Fade In ---
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            c.a = Mathf.Clamp01(timer / fadeDuration);
            deathImage.color = c;
            yield return null;
        }
        c.a = 1f;
        deathImage.color = c;

        // --- Display ---
        yield return new WaitForSecondsRealtime(displayTime);

        // --- Optional Fade Out ---
        if (fadeOut)
        {
            timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.unscaledDeltaTime;
                c.a = Mathf.Clamp01(1f - timer / fadeDuration);
                deathImage.color = c;
                yield return null;
            }
            c.a = 0f;
            deathImage.color = c;
        }

        // --- Load Main Menu only if not already there ---
        if (SceneManager.GetActiveScene().name != mainMenuSceneName)
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }

        // Destroy the death screen after fade
        Destroy(gameObject);
    }
}