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

    [Header("Death Screen")]
    public AudioClip deathMusic;   // Optional

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
            SceneManager.LoadScene(mainMenuSceneName);
            if (destroyPlayerOnDeath)
                Destroy(gameObject, 0.1f);
        }
    }

    // ------------------------------
    // Health Management
    // ------------------------------

    public int GetCurrentHealth() => currentHealth;

    public void TakeDamage(int damageAmount)
    {
        if (Time.time < lastDamageTime + damageCooldown) return;
        lastDamageTime = Time.time;

        currentHealth -= damageAmount;
        if (currentHealth < 0) currentHealth = 0;

        // Update UI
        if (HealthCounter.Instance != null)
            HealthCounter.Instance.UpdateHealthDisplay();

        // Shake camera
        if (CameraShake.Instance != null)
            CameraShake.Instance.Shake(4f);

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

        // Use FindFirstObjectByType to avoid obsolete warning
        DeathScreen deathScreen = DeathScreen.FindFirstObjectByType<DeathScreen>();

        if (deathScreen != null)
        {
            if (deathMusic != null)
                deathScreen.deathSound = deathMusic;

            deathScreen.ShowDeathScreen();
        }
        else
        {
            // Fallback: no death screen
            SceneManager.LoadScene(mainMenuSceneName);
        }

        // Do NOT destroy player immediately — let the scene load naturally
    }
}