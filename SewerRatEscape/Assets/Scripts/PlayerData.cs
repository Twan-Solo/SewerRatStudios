using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;

    public int score;
    public GameObject heldObjectPrefab;

    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Damage")]
    public float damageCooldown = 1.5f;
    private float lastDamageTime;

    [Header("Death Settings")]
    public string mainMenuSceneName = "MainMenu";
    public bool destroyPlayerOnDeath = true;

    [Header("Fade on Death")]
    public FadeTransition fadeTransition; // Assign your FadeTransition in inspector
    public AudioClip deathMusic;          // Optional music/sound to play on death

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            currentHealth = maxHealth;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Escape key returns to Main Menu
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (fadeTransition != null)
            {
                if (deathMusic != null)
                    fadeTransition.fadeSound = deathMusic;

                fadeTransition.FadeToScene(mainMenuSceneName);
                if (destroyPlayerOnDeath)
                    Destroy(gameObject, fadeTransition.fadeDuration + 0.1f);
            }
            else
            {
                if (destroyPlayerOnDeath)
                    Destroy(gameObject);

                if (!string.IsNullOrEmpty(mainMenuSceneName))
                    SceneManager.LoadScene(mainMenuSceneName);
            }
        }
    }

    // ------------------------------
    // Health Management
    // ------------------------------

    public int GetCurrentHealth() => currentHealth;

    public void TakeDamage(int damageAmount)
    {
        // Invincibility frames after getting hit
        if (Time.time < lastDamageTime + damageCooldown) return;
        lastDamageTime = Time.time;

        currentHealth -= damageAmount;
        if (currentHealth < 0) currentHealth = 0;

        // Update UI
        if (HealthCounter.Instance != null)
            HealthCounter.Instance.UpdateHealthDisplay();

        // Shake camera on damage
        if (CameraShake.Instance != null)
            CameraShake.Instance.Shake(4f);

        // Check death
        if (currentHealth <= 0)
            HandleDeath();
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        if (HealthCounter.Instance != null)
            HealthCounter.Instance.UpdateHealthDisplay();
    }

    // ------------------------------
    // Death Handling
    // ------------------------------

    private void HandleDeath()
    {
        Debug.Log("Player has died!");

        if (fadeTransition != null)
        {
            // Play optional death music
            if (deathMusic != null)
                fadeTransition.fadeSound = deathMusic;

            // Start fade, then load main menu
            fadeTransition.FadeToScene(mainMenuSceneName);

            // Destroy player AFTER fade finishes
            if (destroyPlayerOnDeath)
                Destroy(gameObject, fadeTransition.fadeDuration + 0.1f);
        }
        else
        {
            // No fade assigned, fallback to immediate load
            if (destroyPlayerOnDeath)
                Destroy(gameObject);

            if (!string.IsNullOrEmpty(mainMenuSceneName))
                SceneManager.LoadScene(mainMenuSceneName);
        }
    }

    // ------------------------------
    // Scene Management
    // ------------------------------

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == mainMenuSceneName && destroyPlayerOnDeath)
        {
            Destroy(gameObject);
        }
    }
}