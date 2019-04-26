using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSkeleton_Arrow : CRangedEnemyCharacter
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
        CCombo1Ani ani = gameObject.GetComponentInChildren<CCombo1Ani>();
        ani.SpriteRendererNone();
        Vector3 TargetVec = RotateAngle == 0.0f ? gameObject.transform.position + Vector3.right * AttackRange : gameObject.transform.position + Vector3.left * AttackRange;
        Quaternion newRotation = CUtility.MakeQuaternion(gameObject.transform.position, TargetVec);

        // 이미지를 반시계방향으로 회전 시켜서 위로 바라 보게 만든다.
        Vector3 eulerAngles = newRotation.eulerAngles;
        eulerAngles.z += 90.0f;
        newRotation = Quaternion.Euler(eulerAngles);

        GameObject ranged_object = Instantiate<GameObject>(CResourceMgr.LoadEffect("Arrow"), gameObject.transform.position, newRotation);
        CArrow arrow = ranged_object.GetComponent<CArrow>();
        arrow.Owner = gameObject;
        arrow.EndPos = TargetVec;
        arrow.HitAniType = EHitAniType.Type1;

        SoundMgr.PlaySound("Missile", ESoundType.Motion);
    }
    public override void Combo2Damage()
    {
        CCombo2Ani ani = gameObject.GetComponentInChildren<CCombo2Ani>();
        ani.SpriteRendererNone();
        Vector3 TargetVec = RotateAngle == 0.0f ? gameObject.transform.position + Vector3.right * AttackRange : gameObject.transform.position + Vector3.left * AttackRange;
        GameObject LoadedObj = Instantiate<GameObject>(CResourceMgr.LoadEffect("Arrows2"), gameObject.transform.position, new Quaternion());
        CArrows Arrows = LoadedObj.GetComponent<CArrows>();
        Arrows.Owner = gameObject;
        Arrows.end_pos = TargetVec;
        Arrows.HitAniType = EHitAniType.Type2;
        SoundMgr.PlaySound("Missile2", ESoundType.Motion);
    }
    public override void Combo3Damage()
    {
        CCombo3Ani ani = gameObject.GetComponentInChildren<CCombo3Ani>();
        ani.SpriteRendererNone();
        Vector3 TargetVec = RotateAngle == 0.0f ? gameObject.transform.position + Vector3.right * AttackRange : gameObject.transform.position + Vector3.left * AttackRange;
        GameObject LoadedObj = Instantiate<GameObject>(CResourceMgr.LoadEffect("Arrows3"), gameObject.transform.position, new Quaternion());
        CArrows Arrows = LoadedObj.GetComponent<CArrows>();
        Arrows.Owner = gameObject;
        Arrows.end_pos = TargetVec;
        Arrows.HitAniType = EHitAniType.Type3;
        SoundMgr.PlaySound("Missile", ESoundType.Motion);
    }
    
}
