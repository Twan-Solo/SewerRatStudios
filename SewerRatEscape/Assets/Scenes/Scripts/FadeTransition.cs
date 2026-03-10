using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeTransition : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeToScene(int sceneIndex)
    {
        StartCoroutine(FadeOut(sceneIndex));
    }

    IEnumerator FadeIn()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            // Keep RGB white so the sprite shows, only alpha changes
            fadeImage.color = new Color(1, 1, 1, 1 - (timer / fadeDuration));
            yield return null;
        }
    }

    IEnumerator FadeOut(int sceneIndex)
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            // Keep RGB white so the sprite shows, only alpha changes
            fadeImage.color = new Color(1, 1, 1, timer / fadeDuration);
            yield return null;
        }

        SceneManager.LoadScene(sceneIndex);
    }
}
