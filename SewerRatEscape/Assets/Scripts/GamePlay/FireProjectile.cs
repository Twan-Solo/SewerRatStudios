using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class FireProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float shootCooldown = 0.5f;

    [Header("Player Input")]
    private PlayerInput playerInput;
    private InputAction fireAction;

    private float lastShotTime;

    [Header("Ammo")]
    public int maxAmmo = 10;
    public int currentAmmo = 10;

    [Header("Audio")]
    public AudioClip shootSound;

    private TextMeshProUGUI ammoText;

    public void Init(PlayerInput input)
    {
        playerInput = input;

        if (playerInput != null)
        {
            fireAction = playerInput.actions["Fire"];
        }
        else
        {
            Debug.LogError("FireProjectile: PlayerInput is null!");
        }
    }

    void Start()
    {
        // Find ammo UI automatically
        GameObject ammoObj = GameObject.Find("AmmoText");

        if (ammoObj != null)
        {
            ammoText = ammoObj.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogWarning("AmmoText UI not found in scene!");
        }

        UpdateAmmoUI();
    }

    private void Update()
    {
        if (fireAction == null) return;

        if (fireAction.triggered && Time.time >= lastShotTime + shootCooldown && currentAmmo > 0)
        {
            Shoot();
            lastShotTime = Time.time;
            currentAmmo--;

            UpdateAmmoUI();
        }
    }

    private void Shoot()
    {
        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogError("Projectile Prefab or FirePoint missing!");
            return;
        }

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Collider pelletCol = projectile.GetComponent<Collider>();
        Collider[] playerCols = GetComponentsInParent<Collider>(true);

        for (int i = 0; i < playerCols.Length; i++)
        {
            Physics.IgnoreCollision(pelletCol, playerCols[i]);
        }

        if (projectile.TryGetComponent<Projectile>(out Projectile proj))
        {
            proj.SetDirection(firePoint.forward);
        }

        if (shootSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(shootSound);
        }
    }

    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);

        UpdateAmmoUI();

        Debug.Log("Ammo added. Current ammo: " + currentAmmo);
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo + " / " + maxAmmo;
        }
    }
}