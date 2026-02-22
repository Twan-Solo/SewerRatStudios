using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles firing pellet projectiles from the player's camera.
/// Tracks ammo count and shoots pellet prefab on left click.
/// </summary>
public class StunGun : MonoBehaviour
{
    [Header("Projectile")]
    public GameObject pelletPrefab;
    public float pelletSpeed = 30f;
    public Transform firePoint;

    [Header("Ammo")]
    public int currentAmmo = 10;
    public int maxAmmo = 10;

    [Header("Firing")]
    public float fireRate = 0.3f;

    private float fireCooldown;
    private Camera mainCam;
    private CharacterController playerController;

    private void Start()
    {
        mainCam = Camera.main;
        playerController = GetComponentInParent<CharacterController>();
    }

    private void Update()
    {
        fireCooldown -= Time.deltaTime;

        if (Mouse.current.leftButton.wasPressedThisFrame && fireCooldown <= 0f && currentAmmo > 0)
        {
            Fire();
        }
    }

    private void Fire()
    {
        currentAmmo--;
        fireCooldown = fireRate;

        // Spawn the pellet at firepoint, if there is no firepoint set it uses camera position.
        Vector3 spawnPos = firePoint != null ? firePoint.position : mainCam.transform.position;
        Quaternion spawnRot = mainCam.transform.rotation;

        GameObject pellet = Instantiate(pelletPrefab, spawnPos, spawnRot);

        // Ignore collisions between pellet and player.
        Collider pelletCol = pellet.GetComponent<Collider>();
        if (playerController != null && pelletCol != null)
        {
        Physics.IgnoreCollision(playerController, pelletCol);
        }
        Rigidbody rb = pellet.GetComponent<Rigidbody>();
        rb.linearVelocity = mainCam.transform.forward * pelletSpeed;

        Debug.Log("pellet velocity: " + rb.linearVelocity);
        Debug.Log("fired, ammo: " + currentAmmo);
    }

    /// <summary>
    /// Called by ammo pickups to add ammo, capped at maxAmmo.
    /// </summary>
    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);
        Debug.Log("picked up ammo, current: " + currentAmmo);
    }
}