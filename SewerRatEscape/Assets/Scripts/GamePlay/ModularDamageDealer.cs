using UnityEngine;
using System.Collections;

public class ModularDamageDealer : MonoBehaviour
{
    [Header("Hazard Settings (Hurt Player)")]
    public bool dealsDamageToPlayer = true;
    public int damageToPlayer = 10;

    [Header("Stun Settings")]
    public bool isDestructible = true;
    public int hitsToStun = 3;
    public float stunDuration = 3f;

    [Header("Destroy Settings")]
    public int hitsToDestroy = 6;

    public string vulnerableToTag = "PlayerAttack";
    public bool destroyAttackerOnHit = true;

    [Header("Score Settings")]
    public int scoreValue = 10;

    private int currentHits = 0;
    private bool isStunned = false;
    private Coroutine stunRoutine;

    private Rigidbody rb3D;
    private Rigidbody2D rb2D;
    private MonoBehaviour[] enemyScripts;

    void Awake()
    {
        rb3D = GetComponent<Rigidbody>();
        rb2D = GetComponent<Rigidbody2D>();

        enemyScripts = GetComponents<MonoBehaviour>();
    }

    private void OnTriggerEnter(Collider other) => HandleInteraction(other.gameObject);
    private void OnTriggerEnter2D(Collider2D other) => HandleInteraction(other.gameObject);

    // ? Make this public so projectiles can call it
    public void HandleInteraction(GameObject otherObj)
    {
        if (otherObj == null) return;

        // Damage player
        if (dealsDamageToPlayer && otherObj.CompareTag("Player"))
        {
            PlayerData.Instance?.TakeDamage(damageToPlayer);
        }

        // Enemy hit by attack
        if (isDestructible && otherObj.CompareTag(vulnerableToTag))
        {
            currentHits++;

            // Destroy enemy if max hits reached
            if (currentHits >= hitsToDestroy)
            {
                DestroyEnemy();
                return;
            }

            // Stun enemy if stun threshold reached
            if (!isStunned && currentHits >= hitsToStun)
            {
                StunEnemy();
            }

            if (destroyAttackerOnHit)
                Destroy(otherObj);
        }
    }

    // ------------------------------
    // STUN
    // ------------------------------

    private void StunEnemy()
    {
        if (isStunned) return;

        isStunned = true;

        Debug.Log($"<color=yellow>[Stunned]</color> {gameObject.name}");

        if (rb3D != null) rb3D.isKinematic = true;
        if (rb2D != null) rb2D.simulated = false;

        foreach (var script in enemyScripts)
        {
            if (script != this) script.enabled = false;
        }

        stunRoutine = StartCoroutine(StunTimer());
    }

    private IEnumerator StunTimer()
    {
        yield return new WaitForSeconds(stunDuration);
        RecoverFromStun();
    }

    public void RecoverFromStun()
    {
        if (!isStunned) return;

        isStunned = false;

        Debug.Log($"<color=green>[Recovered]</color> {gameObject.name}");

        if (rb3D != null) rb3D.isKinematic = false;
        if (rb2D != null) rb2D.simulated = true;

        foreach (var script in enemyScripts)
        {
            if (script != this) script.enabled = true;
        }

        if (stunRoutine != null)
        {
            StopCoroutine(stunRoutine);
            stunRoutine = null;
        }
    }

    // ------------------------------
    // DESTROY
    // ------------------------------

    private void DestroyEnemy()
    {
        Debug.Log($"<color=red>[Destroyed]</color> {gameObject.name}");

        // Score ONLY happens here now
        ScoreCounter.Instance?.AddScore(scoreValue);

        Destroy(gameObject);
    }
}