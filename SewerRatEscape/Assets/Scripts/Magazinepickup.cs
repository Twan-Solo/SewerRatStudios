using UnityEngine;

/// <summary>
/// Ammo pickup that adds ammo when the player walks over it.
/// </summary>
public class Magazinepickup : MonoBehaviour
{
    public int ammoAmount = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FireProjectile gun = other.GetComponentInChildren<FireProjectile>();

            if (gun != null)
            {
                gun.AddAmmo(ammoAmount);
            }

            Destroy(gameObject);
        }
    }
}