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
        if (other.GetComponentInParent<PlayerData>() != null) return; 
        if (hasHit) return;

        Debug.Log("projectile hit: " + other.gameObject.name);

        // Stun enemy AI if it has one
        EnemyAI enemy = other.GetComponentInParent<EnemyAI>();
        if (enemy != null)
        {
            enemy.Stun();
            Destroy(gameObject);
            return;
        }

        // Assign the attack tag so ModularDamageDealer detects it
        gameObject.tag = attackTag;

        // Call ModularDamageDealer if present
        var dealer = other.GetComponent<ModularDamageDealer>();
        if (dealer != null)
            dealer.SendMessage("HandleInteraction", gameObject, SendMessageOptions.DontRequireReceiver);

        // Stick to surface then despawn
        hasHit = true;
        Destroy(gameObject, stickDuration);
    }
}