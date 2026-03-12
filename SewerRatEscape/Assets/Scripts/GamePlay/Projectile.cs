
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 12f;
    public float lifetime = 5f;
    public float stickDuration = 3f;
    public string attackTag = "PlayerAttack";

    private Vector3 moveDirection;
    private bool hasHit;

    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction == Vector3.zero ? transform.forward : direction.normalized;
        transform.rotation = Quaternion.LookRotation(moveDirection);
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (!hasHit)
        {
            transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        // Ignore the player
        if (other.GetComponentInParent<PlayerData>() != null) return;

        // Ignore score statues completely
        if (other.GetComponent<StatueScoreTrigger>() != null)
            return;

        Debug.Log("Projectile hit: " + other.gameObject.name);

        // ------------------------
        // Enemy Interaction
        // ------------------------
        EnemyAI enemy = other.GetComponentInParent<EnemyAI>();
        if (enemy != null)
        {
            enemy.Stun();
            Destroy(gameObject);
            return;
        }

        // Damageable objects
        ModularDamageDealer dealer = other.GetComponent<ModularDamageDealer>();
        if (dealer != null)
        {
            gameObject.tag = attackTag;
            dealer.HandleInteraction(gameObject);
            Destroy(gameObject);
            return;
        }

        // ------------------------
        // Everything else (walls etc.)
        // ------------------------
        hasHit = true;
        Destroy(gameObject, stickDuration);
    }
}