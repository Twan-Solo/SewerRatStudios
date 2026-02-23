using UnityEngine;
using System.Collections;

public class ModularDamageDealer : MonoBehaviour
{
    [Header("Hazard Settings (Hurt Player)")]
    public bool dealsDamageToPlayer = true;
    public int damageToPlayer = 10;

    [Header("Stun Settings")]
    public bool isDestructible = true;
    public int hitsToStun = 3;               // Hits required to stun
    public string vulnerableToTag = "PlayerAttack";
    public bool destroyAttackerOnHit = true;
    public float stunDuration = 3f;          // Seconds enemy is stunned

    [Header("Score Settings")]
    public int scoreValue = 10;              // Points awarded per stun

    private int currentHits = 0;
    private bool isStunned = false;          // Prevent double scoring
    private Coroutine stunRoutine;

    private Rigidbody rb3D;
    private Rigidbody2D rb2D;
    private MonoBehaviour[] enemyScripts;    // Enemy AI/movement scripts

    void Awake()
    {
        rb3D = GetComponent<Rigidbody>();
        rb2D = GetComponent<Rigidbody2D>();

        // Cache all enemy scripts you want to disable while stunned
        enemyScripts = GetComponents<MonoBehaviour>();
    }

    // ------------------------------
    // Trigger Handling
    // ------------------------------

    private void OnTriggerEnter(Collider other) => HandleInteraction(other.gameObject);

    private void OnTriggerEnter2D(Collider2D other) => HandleInteraction(other.gameObject);

    // ------------------------------
    // Interaction Logic
    // ------------------------------

    private void HandleInteraction(GameObject otherObj)
    {
        if (otherObj == null) return;

        // 1. Hurt Player
        if (dealsDamageToPlayer && otherObj.CompareTag("Player"))
        {
            PlayerData.Instance?.TakeDamage(damageToPlayer);
        }

        // 2. Check destructible / stun
        if (isDestructible && otherObj.CompareTag(vulnerableToTag))
        {
            if (!isStunned)
            {
                currentHits++;
                if (currentHits >= hitsToStun)
                {
                    StunEnemy();
                }
            }

            if (destroyAttackerOnHit)
                Destroy(otherObj);
        }
    }

    // ------------------------------
    // Stun Logic
    // ------------------------------

    private void StunEnemy()
    {
        if (isStunned) return;

        isStunned = true;

        Debug.Log($"<color=yellow>[Stunned]</color> {gameObject.name} is stunned!");

        // Award points once
        ScoreCounter.Instance?.AddScore(scoreValue);

        // Stop movement & AI
        if (rb3D != null) rb3D.isKinematic = true;
        if (rb2D != null) rb2D.simulated = false;

        foreach (var script in enemyScripts)
        {
            if (script != this) script.enabled = false;
        }

        // Start stun recovery timer
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
        currentHits = 0;

        Debug.Log($"<color=green>[Recovered]</color> {gameObject.name} is no longer stunned.");

        // Restore movement & AI
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
}