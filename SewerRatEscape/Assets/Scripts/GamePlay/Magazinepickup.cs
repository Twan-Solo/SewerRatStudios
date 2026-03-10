using UnityEngine;

/// <summary>
/// Ammo pickup that adds ammo when the player walks over it. Respawns after a delay.
/// </summary>
public class Magazinepickup : MonoBehaviour
{
    public int ammoAmount = 5;
    public float respawnTime = 30f;

    [Header("Audio")]
    public AudioClip pickupSound;

    private Collider pickupCollider;
    private Renderer[] renderers;

    private void Awake()
    {
        pickupCollider = GetComponent<Collider>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FireProjectile gun = other.GetComponentInChildren<FireProjectile>();

            if (gun != null)
            {
                gun.AddAmmo(ammoAmount);
            }

            if (pickupSound != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(pickupSound);
            }

            StartCoroutine(Respawn());
        }
    }

    private System.Collections.IEnumerator Respawn()
    {
        // Hides it but keep the GameObject active so the timer runs
        pickupCollider.enabled = false;
        foreach (Renderer r in renderers)
        {
            r.enabled = false;
        }

        yield return new WaitForSeconds(respawnTime);

        pickupCollider.enabled = true;
        foreach (Renderer r in renderers)
        {
            r.enabled = true;
        }
    }
}