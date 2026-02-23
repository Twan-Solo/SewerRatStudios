
using UnityEngine;

/// <summary>
/// Abstract base for all enemy states. EnemyAI manages transitions between these.
/// </summary>
public abstract class EnemyState
{
    protected EnemyAI enemy;

    public EnemyState(EnemyAI enemy)
    {
        this.enemy = enemy;
    }

    public abstract void Enter();
    public abstract void Execute();
    public abstract void Exit();
}
