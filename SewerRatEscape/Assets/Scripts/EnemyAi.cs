using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// State machine driven enemy AI. NavMeshAgent handles movement
/// Configure speed, damage, and stun values per enemy type in the Inspector.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Detection")]
    public float detectionRange = 10f;
    
    [Header("Combat")]
    public int lifeDamage = 1;
    public float stunDuration = 3f;
    
    [Header("Movement")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float wanderRadius = 15f;
    public float wanderPause = 2f;
    
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Transform player;
    
    private EnemyState currentState;
    
    public string CurrentStateName { get; private set; }
    
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }
    
    private void Start()
    {
        ChangeState(new PatrolState(this));
    }
    
    private void Update()
    {
        // Player gets spawned at runtime, so grab the reference if it's missing
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
    }

    /// <summary>
    /// All state transistions go through this method
    /// </summary>
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

    /// <summary>
    /// Called by the pellet gun on hit. Forces stunned state no matter what.
    /// </summary>
    public void Stun()
    {
        ChangeState(new StunnedState(this));
    }

    public void CatchPlayer()
    {
        if (PlayerData.Instance != null)
        {
            PlayerData.Instance.TakeDamage(lifeDamage);
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