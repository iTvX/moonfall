using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    private enemy enemy;

    private float patrolTimer;

    private float patrolDuration;

    public void Enter(enemy enemy)
    {
        patrolDuration = enemy.walkingduration;
        if (patrolDuration == 0)
        {
            patrolDuration = UnityEngine.Random.Range(1, 10);
        }
        this.enemy = enemy;
    }

    public void Execute()
    {
        Patrol();

        enemy.Move();

        if (enemy.Target != null && enemy.InThrowRange)
        {
            enemy.ChangeState(new RangedState());
        }
    }

    public void Exit()
    {

    }


    private void Patrol()
    {
        patrolTimer += Time.deltaTime;

        if (patrolTimer >= patrolDuration)
        {
            enemy.ChangeState(new IdleState());
        }
    }
}
