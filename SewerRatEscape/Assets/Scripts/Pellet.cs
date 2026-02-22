using UnityEngine;

/// <summary>
/// Projectile that stuns enemies on contact and sticks to surfaces before despawning.
/// </summary>
public class Pellet : MonoBehaviour
{
    public float lifetime = 5f;
    public float stickDuration = 3f;

    private bool hasHit;

    private void Awake()
    {
        //Ignore player collisions immediately on spawn.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Collider myCol = GetComponent<Collider>();
            Collider[] playerCols = player.GetComponentsInChildren<Collider>();

            for (int i = 0; i < playerCols.Length; i++)
            {
                Physics.IgnoreCollision(myCol, playerCols[i]);
            }
        }
    }
    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit)
        {
            return;
        }

        hasHit = true;

        // Check for if it hit an enemy.
        EnemyAI enemy = collision.collider.GetComponent<EnemyAI>();

        if (enemy != null)
        {
            enemy.Stun();
            Destroy(gameObject);
            return;
        }

        // If pellet hits a different surface it sticks in place then despawns.
        StickToSurface();
    }

    private void StickToSurface()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        Destroy(gameObject, stickDuration);
    }
}