using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Buttons")]
    public Button playButton;
    public Button quitButton;

    [Header("Fade Transition")]
    public FadeTransition fadeTransition; // assign in inspector

    [Header("Scenes")]
    public string levelToLoad = "Level1"; // scene name as string

    private void Awake()
    {
        if (playButton != null)
            playButton.onClick.AddListener(OnPlayClicked);

        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitClicked);
    }

    private void OnPlayClicked()
    {
        if (fadeTransition != null)
        {
            // Use string version of FadeToScene
            fadeTransition.FadeToScene(levelToLoad);
        }
        else
        {
            // fallback if no fade assigned
            UnityEngine.SceneManagement.SceneManager.LoadScene(levelToLoad);
        }
    }

    private void OnQuitClicked()
    {
        Application.Quit();
    }
}
