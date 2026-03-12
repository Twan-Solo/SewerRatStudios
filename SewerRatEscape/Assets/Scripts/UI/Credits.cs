using UnityEngine;
using System.Collections;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class Credits : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    [Header("Fade System")]
    [SerializeField] private FadeTransition fadeTransition;

    [Header("Scrolling Credits")]
    [SerializeField] private RectTransform creditsText;
    [SerializeField] private float scrollSpeed = 50f;
    [SerializeField] private float startY = -600f;
    [SerializeField] private float endY = 600f;

    [Header("Bounce Animator (optional)")]
    [SerializeField] private Animator bounceAnimator;
    [SerializeField] private GameObject bounceObject;
    [SerializeField] private string bounceTriggerName = "PlayBounce";

    [Header("Bounce Sound")]
    [SerializeField] private AudioClip bounceSound;
    [SerializeField] private AudioSource audioSource;

    [Header("Timing")]
    [SerializeField] private float timeAfterBounce = 3f;

    private bool scrollingFinished = false;
    private bool bounceStarted = false;
    private bool transitioning = false;

    void Start()
    {
        // Hide bounce object
        if (bounceObject != null)
            bounceObject.SetActive(false);

        // Set starting position
        Vector2 pos = creditsText.anchoredPosition;
        pos.y = startY;
        creditsText.anchoredPosition = pos;
    }

    void Update()
    {
        if (!scrollingFinished)
        {
            ScrollCredits();
        }

        // Allow skipping ONLY after credits finished
#if ENABLE_INPUT_SYSTEM
        if (scrollingFinished && !transitioning &&
            Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            GoToMenu();
        }
#else
        if (scrollingFinished && !transitioning && Input.anyKeyDown)
        {
            GoToMenu();
        }
#endif
    }

    private void ScrollCredits()
    {
        creditsText.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        if (creditsText.anchoredPosition.y >= endY)
        {
            scrollingFinished = true;
            StartBounceAndTimer();
        }
    }

    private void StartBounceAndTimer()
    {
        if (bounceStarted) return;

        bounceStarted = true;

        if (bounceObject != null)
            bounceObject.SetActive(true);

        if (bounceAnimator != null)
            bounceAnimator.SetTrigger(bounceTriggerName);

        if (audioSource != null && bounceSound != null)
            audioSource.PlayOneShot(bounceSound);

        StartCoroutine(GoToMenuAfterDelay());
    }

    private IEnumerator GoToMenuAfterDelay()
    {
        yield return new WaitForSeconds(timeAfterBounce);
        GoToMenu();
    }

    private void GoToMenu()
    {
        if (transitioning) return;
        transitioning = true;

        if (fadeTransition != null)
            fadeTransition.FadeToScene(mainMenuSceneName);
    }
}