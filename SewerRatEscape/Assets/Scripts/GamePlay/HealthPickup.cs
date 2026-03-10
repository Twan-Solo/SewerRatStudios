using UnityEngine;

public class Health : MonoBehaviour
{
    public int healthValue = 1;
    public float respawnTime = 30f;

    [Header("Sound")]
    public AudioClip collectSound;

    private Collider pickupCollider;
    private Renderer[] renderers;

    private void Awake()
    {
        pickupCollider = GetComponent<Collider>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (HealthCounter.Instance != null)
                HealthCounter.Instance.AddHealth(healthValue);

            // Play sound at this object's position
            if (collectSound != null)
                AudioSource.PlayClipAtPoint(collectSound, transform.position);

            StartCoroutine(Respawn());
        }
    }

    private System.Collections.IEnumerator Respawn()
    {
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