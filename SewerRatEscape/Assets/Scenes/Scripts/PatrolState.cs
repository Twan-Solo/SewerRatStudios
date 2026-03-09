
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Wanders to random points on the NavMesh instead of following set waypoints.
/// </summary>
public class PatrolState : EnemyState
{
    private const float ARRIVAL_THRESHOLD = 1.5f;
    private const int MAX_SAMPLE_ATTEMPTS = 10;

    private float pauseTimer;
    private bool isWaiting;

    public PatrolState(EnemyAI enemy) : base(enemy) { }

    public override void Enter()
    {
        enemy.agent.speed = enemy.patrolSpeed;
        enemy.agent.isStopped = false;
        isWaiting = false;

        PickNewDestination();
    }

    public override void Execute()
    {
        if (enemy.DistanceToPlayer() <= enemy.detectionRange)
        {
            enemy.ChangeState(new ChaseState(enemy));
            return;
        }

        if (isWaiting)
        {
            pauseTimer -= Time.deltaTime;

            if (pauseTimer <= 0f)
            {
                isWaiting = false;
                PickNewDestination();
            }

            return;
    }

        if (!enemy.agent.pathPending && enemy.agent.remainingDistance <= ARRIVAL_THRESHOLD)
        {
            isWaiting = true;
            pauseTimer = enemy.wanderPause;
        }
    }

    public override void Exit() { }

    /// <summary>
    /// Samples a random NavMesh point within wander radius.
    /// Retries if the point lands off the mesh.
    /// </summary>
    private void PickNewDestination()
    {
        for (int i = 0; i < MAX_SAMPLE_ATTEMPTS; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * enemy.wanderRadius;
            randomDirection += enemy.transform.position;

            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomDirection, out hit, enemy.wanderRadius, NavMesh.AllAreas))
            {
                enemy.agent.SetDestination(hit.position);
                return;
            }
        }

        Debug.LogWarning(enemy.gameObject.name + " couldn't find a wander point");
    }
}
