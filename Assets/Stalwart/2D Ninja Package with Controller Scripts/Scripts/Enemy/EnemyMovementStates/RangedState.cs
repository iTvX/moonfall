using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedState : IEnemyState
{
    private enemy enemy;

    private float fireTimer;
    private float fireCoolDown = 2;
    private bool canFire = true;

    public void Enter(enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        Fire();

        if (enemy.InMeleeRange)
        {
            enemy.ChangeState(new MeleeState());
        }
        else if (enemy.Target != null)
        {
            enemy.Move();
        }
        else
        {
            enemy.ChangeState(new IdleState());
        }
    }

    public void Exit()
    {
        
    }

    private void Fire()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireCoolDown)
        {
            canFire = true;
            fireTimer = 0;
        }

        if (canFire)
        {
            canFire = false;
            enemy.anim.SetTrigger("throw");
        }
    }
}
