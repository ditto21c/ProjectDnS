using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAIController : CBaseController
{
    private void Update()
    {
        EState curState = Character.curState;
        switch (curState)
        {
            case EState.Idle:
                if (!IsInvoking("FindNearestTarget"))
                    Invoke("FindNearestTarget", Random.Range(0.0f, 1.0f));
                break;
            case EState.Move:
                if (Character.paths.Count == 0 && !Character.isMoving)
                {
                    Character.Idle();
                    if (Character.TargetObj)
                    {
                        AttackTarget();
                    }
                }
                break;
            default:
                break;
        };
    }

    protected void FindNearestTarget()
    {
        bool bFindTarget = Character.FindNearestTarget();
        if (bFindTarget)
        {
            Character.MoveAttackRange();
            if (Character.curState == EState.Move)
            {
                return;
            }
            else if (Character.curState == EState.Idle)
            {
                AttackTarget();
                return;
            }
        }

        Invoke("RandomMove", Random.Range(0.0f, 1.0f));
    }

    protected void AttackTarget()
    {
        Character.RotateTarget();
        Character.Attack1();
    }

    void RandomMove()
    {
        Character.RandomMove();
    }
}
