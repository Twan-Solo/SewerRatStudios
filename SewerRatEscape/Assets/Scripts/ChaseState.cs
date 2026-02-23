using UnityEngine;

/// <summary>
/// Pursues the player until they escape or get caught.
/// <summary>
public class ChaseState : EnemyState
{
    private const float CATCH_DISTANCE = 1.5f;

    // Made the lose range wider than the detection range so it doesn't drop the chase instantly at the edge
    private float loseRange;

    public ChaseState(EnemyAI enemy) : base(enemy) { }

    public override void Enter()
    {
        enemy.agent.speed = enemy.chaseSpeed;
        enemy.agent.isStopped = false;
        loseRange = enemy.detectionRange * 1.5f;
    }

    public override void Execute()
    {
        if (enemy.player == null)
        {
            enemy.ChangeState(new PatrolState(enemy));
            return;
        }

        float distance = enemy.DistanceToPlayer();

        if (distance > loseRange)
        {
            enemy.ChangeState(new PatrolState(enemy));
            return;
        }

        if (distance <= CATCH_DISTANCE)
        {
            enemy.CatchPlayer();
            enemy.ChangeState(new PatrolState(enemy));
            return;
    }
    
        enemy.agent.SetDestination(enemy.player.position);
    }

    public override void Exit() { }
}