using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// State machine driven enemy AI. NavMeshAgent handles movement.
/// Configure speed, damage, and stun values per enemy type in the Inspector.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    private Animator animator;
    [Header("Detection")]
    public float detectionRange = 10f;
    
    [Header("Combat")]
    public int lifeDamage = 1;
    public float stunDuration = 3f;
    public float catchCooldown = 2f;
    
    [Header("Movement")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float wanderRadius = 15f;
    public float wanderPause = 2f;

    [Header("Audio")]
    public AudioClip attackSound;
    public AudioSource scurrySource;
    
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Transform player;
    
    private EnemyState currentState;
    private float lastCatchTime;
    
    public string CurrentStateName { get; private set; }
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }
    
    private void Start()
    {
        ChangeState(new PatrolState(this));
    }
    
    private void Update()
    {
        if (animator != null)
        {
            bool isMoving = agent.velocity.sqrMagnitude > 0.1f;
            animator.SetBool("IsRunning", isMoving);
        }
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        if (currentState != null)
        {
            currentState.Execute();
        }

        // Scurry audio matches movement
        if (scurrySource != null)
        {
            bool moving = agent.velocity.sqrMagnitude > 0.1f;
            if (moving && !scurrySource.isPlaying)
            {
                scurrySource.Play();
            }
            else if (!moving && scurrySource.isPlaying)
            {
                scurrySource.Stop();
            }
        }
    }

    public void ChangeState(EnemyState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        
        currentState = newState;
        CurrentStateName = newState.GetType().Name;
        currentState.Enter();

        Debug.Log(gameObject.name + "-> " + CurrentStateName);
    }

    public float DistanceToPlayer()
    {
        if (player == null)
        {
            return Mathf.Infinity;
        }

        return Vector3.Distance(transform.position, player.position);
    }

    public void Stun()
    {
        ChangeState(new StunnedState(this));
    }

    public void CatchPlayer()
    {
        if (Time.time < lastCatchTime + catchCooldown) return;
        lastCatchTime = Time.time;

        if (PlayerData.Instance != null)
        {
            PlayerData.Instance.TakeDamage(lifeDamage);
        }

        if (attackSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(attackSound);
        }

        Debug.Log(gameObject.name + " caught the player, dmg: " + lifeDamage);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
}