using UnityEngine;

/// <summary>
/// Freezes enemy in place until the stun wears off, then goes back to patrol.
/// </summary>
public class StunnedState : EnemyState
{
    private float stunTimer;
    private float duration;

    public StunnedState(EnemyAI enemy) : base(enemy)
    {
        duration = enemy.stunDuration;
    }

    public StunnedState(EnemyAI enemy, float customDuration) : base(enemy)
    {
        duration = customDuration;
    }

    public override void Enter()
    {
        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;
        stunTimer = duration;

        Debug.Log(enemy.gameObject.name + " is stunned!" + stunTimer + "s");
    }

    public override void Execute()
    {
        stunTimer -= Time.deltaTime;

        if (stunTimer <= 0)
        {
            enemy.ChangeState(new PatrolState(enemy));
        }
    }

    public override void Exit()
    {
        enemy.agent.isStopped = false;
    }
}