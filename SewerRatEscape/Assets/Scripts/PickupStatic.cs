using UnityEngine;

public class PickupStatic : MonoBehaviour
{
    public int scoreValue = 1;      // Score added when player collects
    public bool destroyOnEnemy = false; // Static pickups might stay even if enemy touches
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            Destroy(gameObject); // Collected by player
            Debug.Log("Trigger hit by: " + other.name);
        }

        if (destroyOnEnemy && other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}