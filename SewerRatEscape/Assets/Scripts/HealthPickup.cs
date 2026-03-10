using UnityEngine;

public class Health : MonoBehaviour
{
    public int healthValue = 1;

    [Header("Sound")]
    public AudioClip collectSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (HealthCounter.Instance != null)
                HealthCounter.Instance.AddHealth(healthValue);

            // Play sound at this object's position
            if (collectSound != null)
                AudioSource.PlayClipAtPoint(collectSound, transform.position);

            Destroy(gameObject);
        }

        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
