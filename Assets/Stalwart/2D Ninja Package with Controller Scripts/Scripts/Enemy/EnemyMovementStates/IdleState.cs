using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState
{

    private enemy enemy;

    private float idleTimer;

    private float idleDuration;

    public void Enter(enemy enemy)
    {
        idleDuration = enemy.idleduration;
        if (idleDuration == 0)
        {
            idleDuration = UnityEngine.Random.Range(1, 10);
        }
        this.enemy = enemy;
    }

    public void Execute()
    {
        Idle();
        if (enemy.Target != null)
        {
            enemy.ChangeState(new PatrolState());
        }
    }

    public void Exit()
    {

    }
    private void Idle()
    {        
        enemy.anim.SetFloat("speed", 0);

        idleTimer += Time.deltaTime;

        if (idleTimer >= idleDuration)
        {
            enemy.ChangeState(new PatrolState());
        }
    }
}
