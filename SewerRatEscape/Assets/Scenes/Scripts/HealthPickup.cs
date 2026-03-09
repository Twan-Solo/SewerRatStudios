using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public int healthValue = 1;   // Amount of health to give the player

    [Header("Audio")]
    public AudioClip pickupSound;  // Sound to play when picked up

    // Called when something enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Add health to player
            if (HealthCounter.Instance != null)
                HealthCounter.Instance.AddHealth(healthValue);

            // Play pickup sound via AudioManager
            if (AudioManager.Instance != null && pickupSound != null)
            {
                AudioManager.Instance.PlaySFX(pickupSound);
            }

            // Destroy the health pickup
            Destroy(gameObject);
        }

        // Destroy the pickup if an enemy touches it
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}