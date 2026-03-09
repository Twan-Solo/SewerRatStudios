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
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            string menuScene = "MainMenu";

            if (!string.IsNullOrEmpty(mainMenuSceneName))
                menuScene = mainMenuSceneName;

            Debug.Log("Escape pressed: loading Main Menu -> " + menuScene);
            SceneManager.LoadScene(menuScene);
        }
    }

    // ------------------------------
    // Health Management
    // ------------------------------

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

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
        {
            CameraShake.Instance.Shake(4f);
        }

        // Check death
        if (currentHealth <= 0)
            HandleDeath();
            
            Debug.Log("shake instance: " + (CameraShake.Instance != null));
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

        if (destroyPlayerOnDeath)
            Destroy(gameObject);

        if (!string.IsNullOrEmpty(mainMenuSceneName))
            SceneManager.LoadScene(mainMenuSceneName);
    }

    // ------------------------------
    // Scene Management
    // ------------------------------

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu" && destroyPlayerOnDeath)
        {
            Destroy(gameObject);
        }
    }
}