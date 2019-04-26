using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSkeleton_Sword : CMeleeEnemyCharacter
{

    public override void ComboEnable()
    {
        base.ComboEnable();

    }
    public override void ComboDisable()
    {
        base.ComboDisable();

    }
    public override void ComboEnd()
    {
        base.ComboEnd();

        if (bReservedCombo)
        {
            if (curState == EState.Combo1)
            {
                AllStopAni();
                CCombo2Ani ani = gameObject.GetComponentInChildren<CCombo2Ani>();
                ani.InitAngle(RotateAngle);
                ani.PlayAni();

                Vector3 EndPos = gameObject.transform.position;
                EndPos.x = RotateAngle == 0.0f ? EndPos.x + ani.AttackMove : EndPos.x - ani.AttackMove;
                Move(gameObject.transform.position, EndPos);

                curState = EState.Combo2;
            }
            else if (curState == EState.Combo2)
            {
                AllStopAni();
                CCombo3Ani ani = gameObject.GetComponentInChildren<CCombo3Ani>();
                ani.InitAngle(RotateAngle);
                ani.PlayAni();

                Vector3 EndPos = gameObject.transform.position;
                EndPos.x = RotateAngle == 0.0f ? EndPos.x + ani.AttackMove : EndPos.x - ani.AttackMove;
                Move(gameObject.transform.position, EndPos);

                curState = EState.Combo3;
            }
        }

        bReservedCombo = false;
    }

    public override void Attack1()
    {
        if (curState == EState.Idle || curState == EState.Move)
        {
            AllStopAni();
            CCombo1Ani ani = gameObject.GetComponentInChildren<CCombo1Ani>();
            ani.InitAngle(RotateAngle);
            ani.PlayAni();

            Vector3 EndPos = gameObject.transform.position;
            EndPos.x = RotateAngle == 0.0f ? EndPos.x + ani.AttackMove : EndPos.x - ani.AttackMove;
            Move(gameObject.transform.position, EndPos);

            curState = EState.Combo1;
        }
        else if (curState == EState.Combo1 || curState == EState.Combo2)
        {
            if (bComboEnable)
            {
                bReservedCombo = true;
            }
        }
    }

    public override void Attack2()
    {
        if (curState != EState.Idle) return;


    }

    public override void Combo1Damage()
    {
        Vector3 pos = RotateAngle == 0.0f ? gameObject.transform.position + Vector3.right : gameObject.transform.position + Vector3.left;
        GameObject TriggerObj = Instantiate<GameObject>(CResourceMgr.LoadTrigger("DamageCircleTrigger"), pos, Quaternion.identity);
        CDamageTrigger Trigger = TriggerObj.GetComponent<CDamageTrigger>();
        Trigger.Owner = gameObject;
        CCombo1Ani ani = gameObject.GetComponentInChildren<CCombo1Ani>();
        Trigger.HitMove = ani.HitMove;
        Trigger.AliveTime = 0.1f;
        Trigger.Damage = 10;
        Trigger.HitAniType = EHitAniType.Type1;
        Trigger.ApplyAliveTime();

        SoundMgr.PlaySound("Attack", ESoundType.Motion);
    }
    public override void Combo2Damage()
    {
        Vector3 pos = RotateAngle == 0.0f ? gameObject.transform.position + Vector3.right : gameObject.transform.position + Vector3.left;
        GameObject TriggerObj = Instantiate<GameObject>(CResourceMgr.LoadTrigger("DamageCircleTrigger"), pos, Quaternion.identity);
        CDamageTrigger Trigger = TriggerObj.GetComponent<CDamageTrigger>();
        Trigger.Owner = gameObject;
        CCombo1Ani ani = gameObject.GetComponentInChildren<CCombo1Ani>();
        Trigger.HitMove = ani.HitMove;
        Trigger.AliveTime = 0.1f;
        Trigger.Damage = 10;
        Trigger.ApplyAliveTime();
        Trigger.HitAniType = EHitAniType.Type2;
        SoundMgr.PlaySound("Attack2", ESoundType.Motion);
    }
    public override void Combo3Damage()
    {
        Vector3 pos = RotateAngle == 0.0f ? gameObject.transform.position + Vector3.right : gameObject.transform.position + Vector3.left;
        GameObject TriggerObj = Instantiate<GameObject>(CResourceMgr.LoadTrigger("DamageCircleTrigger"), pos, Quaternion.identity);
        CDamageTrigger Trigger = TriggerObj.GetComponent<CDamageTrigger>();
        Trigger.Owner = gameObject;
        CCombo1Ani ani = gameObject.GetComponentInChildren<CCombo1Ani>();
        Trigger.HitMove = ani.HitMove;
        Trigger.AliveTime = 0.1f;
        Trigger.Damage = 20;
        Trigger.ApplyAliveTime();
        Trigger.HitAniType = EHitAniType.Type3;
        SoundMgr.PlaySound("Attack", ESoundType.Motion);
    }
   
}
