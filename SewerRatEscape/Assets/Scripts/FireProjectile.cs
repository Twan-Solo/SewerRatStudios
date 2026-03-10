using UnityEngine;
using UnityEngine.InputSystem;

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

    private void Update()
    {
        if (fireAction == null) return;

        if (fireAction.triggered && Time.time >= lastShotTime + shootCooldown && currentAmmo > 0)
        {
            Shoot();
            lastShotTime = Time.time;
            currentAmmo--;
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
        Debug.Log("Ammo added. Current ammo: " + currentAmmo);
    }
}