using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeState : IEnemyState
{

    private enemy enemy;

    private float attackTimer;
    private float attackCoolDown = 1;
    private bool canAttack = true;


    public void Enter(enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        Attack();
        if (enemy.InThrowRange && !enemy.InMeleeRange)
        {
            enemy.ChangeState(new RangedState());
        }
        else if (enemy.Target == null)
        {
            enemy.ChangeState(new IdleState());
        }
    }

    public void Exit()
    {
        
    }
    private void Attack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCoolDown)
        {
            canAttack = true;
            attackTimer = 0;
        }

        if (canAttack)
        {
            int random = UnityEngine.Random.Range(1, 3);
            if (random == 1)
            {
                enemy.anim.SetTrigger("attack");
            }
            else
            {
                enemy.anim.SetTrigger("attack2");
            }
            canAttack = false;
        }
    }
}
