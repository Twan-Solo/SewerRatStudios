using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Pause menu that freezes the game with a resume and quit button.
/// Includes fade and sound when quitting to main menu.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject pauseMenuUI;
    public string mainMenuScene = "MainMenu";

    [Header("Fade")]
    public FadeTransition fadeTransition; // Assign in inspector

    [Header("Audio")]
    public AudioClip buttonClickSound;

    private bool isPaused = false;

    private void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        PlayButtonSound();

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        EnablePlayerInput(true);
    }

    public void Pause()
    {
        PlayButtonSound();

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        EnablePlayerInput(false);
    }

    public void QuitToMainMenu()
    {
        PlayButtonSound();

        // Make sure timeScale is reset in case game is paused
        Time.timeScale = 1f;

        if (fadeTransition != null)
        {
            // Pass scene name directly to FadeTransition
            fadeTransition.FadeToScene(mainMenuScene);
        }
        else
        {
            // Fallback if no FadeTransition is assigned
            SceneManager.LoadScene(mainMenuScene);
        }
    }

    private void PlayButtonSound()
    {
        if (buttonClickSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(buttonClickSound);
        }
    }

    private void EnablePlayerInput(bool enable)
    {
        PlayerInput playerInput = FindAnyObjectByType<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.enabled = enable;
        }
    }
}