using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHealer : CRangedPlayerCharacter
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
                curState = EState.Combo2;
                if (0.0f != ani.AttackMove)
                {
                    Vector3 EndPos = gameObject.transform.position;
                    EndPos.x = RotateAngle == 0.0f ? EndPos.x + ani.AttackMove : EndPos.x - ani.AttackMove;
                    Move(gameObject.transform.position, EndPos, true);
                }

            }
            else if (curState == EState.Combo2)
            {
                AllStopAni();
                CCombo3Ani ani = gameObject.GetComponentInChildren<CCombo3Ani>();
                ani.InitAngle(RotateAngle);
                ani.PlayAni();
                curState = EState.Combo3;
                if (0.0f != ani.AttackMove)
                {
                    Vector3 EndPos = gameObject.transform.position;
                    EndPos.x = RotateAngle == 0.0f ? EndPos.x + ani.AttackMove : EndPos.x - ani.AttackMove;
                    Move(gameObject.transform.position, EndPos, true);
                }
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
            curState = EState.Combo1;

            if (ani.AttackMove != 0.0f)
            {
                Vector3 EndPos = gameObject.transform.position;
                EndPos.x = RotateAngle == 0.0f ? EndPos.x + ani.AttackMove : EndPos.x - ani.AttackMove;
                Move(gameObject.transform.position, EndPos, true);
            }

        }
        else if (curState == EState.Combo1 || curState == EState.Combo2)
        {
            if (bComboEnable)
            {
                bReservedCombo = true;
            }
        }
        else if (curState == EState.Defence)
        {
            if (ReservedParringType == EReservedParringType.Start)
                ReservedParringType = EReservedParringType.Attack;
        }

    }
    public override void Attack2()
    {
        if (curHealthPoint < 6)
            return;

        if (curState == EState.Idle || curState == EState.Move)
        {
            AllStopAni();
            CCombo3Ani ani = gameObject.GetComponentInChildren<CCombo3Ani>();
            ani.PlayAni();
            curState = EState.Combo3;

            if (ani.AttackMove != 0.0f)
            {
                Vector3 EndPos = gameObject.transform.position;
                EndPos.x = RotateAngle == 0.0f ? EndPos.x + ani.AttackMove : EndPos.x - ani.AttackMove;
                Move(gameObject.transform.position, EndPos, true);
            }

            curHealthPoint -= 5;
        }
    }
    public override void Skill()
    {
        if (curState != EState.Idle) return;

        CSkillAni ani = gameObject.GetComponentInChildren<CSkillAni>();
        ani.PlayAni();
        curState = EState.Skill;
    }

    public override void SkillDamage()
    {
        Vector3 TargetVec = RotateAngle == 0.0f ? gameObject.transform.position + Vector3.right * AttackRange : gameObject.transform.position + Vector3.left * AttackRange;
        GameObject LoadedObj = Instantiate<GameObject>(CResourceMgr.LoadEffect("ArrowsCircle"), gameObject.transform.position, new Quaternion());
        CArrows Arrows = LoadedObj.GetComponent<CArrows>();
        Arrows.Owner = gameObject;
        Arrows.end_pos = TargetVec;
    }

    public override void Combo1Damage()
    {
        Vector3 TargetVec = RotateAngle == 0.0f ? gameObject.transform.position + Vector3.right * AttackRange : gameObject.transform.position + Vector3.left * AttackRange;
        Quaternion newRotation = CUtility.MakeQuaternion(gameObject.transform.position, TargetVec);

        // 이미지를 반시계방향으로 회전 시켜서 위로 바라 보게 만든다.
        Vector3 eulerAngles = newRotation.eulerAngles;
        eulerAngles.z += 90.0f;
        newRotation = Quaternion.Euler(eulerAngles);

        GameObject LoadedObj = Instantiate<GameObject>(CResourceMgr.LoadEffect("IceBolt"), gameObject.transform.position, newRotation);
        CIceBolt Effect = LoadedObj.GetComponent<CIceBolt>();
        Effect.Owner = gameObject;
        Effect.EndPos = TargetVec;
        CCombo1Ani ani = gameObject.GetComponentInChildren<CCombo1Ani>();
        Effect.HitMove = ani.HitMove;
        Effect.HitAniType = EHitAniType.Type1;
        SoundMgr.PlaySound("Razor", ESoundType.Motion);
    }
    public override void Combo2Damage()
    {
        Vector3 TargetVec = RotateAngle == 0.0f ? gameObject.transform.position + Vector3.right * AttackRange : gameObject.transform.position + Vector3.left * AttackRange;
        Quaternion newRotation = CUtility.MakeQuaternion(gameObject.transform.position, TargetVec);

        // 이미지를 반시계방향으로 회전 시켜서 위로 바라 보게 만든다.
        Vector3 eulerAngles = newRotation.eulerAngles;
        eulerAngles.z += 90.0f;
        newRotation = Quaternion.Euler(eulerAngles);

        GameObject LoadedObj = Instantiate<GameObject>(CResourceMgr.LoadEffect("IceBolt"), gameObject.transform.position, newRotation);
        CIceBolt Effect = LoadedObj.GetComponent<CIceBolt>();
        Effect.Owner = gameObject;
        Effect.EndPos = TargetVec;
        CCombo1Ani ani = gameObject.GetComponentInChildren<CCombo1Ani>();
        Effect.HitMove = ani.HitMove;
        Effect.HitAniType = EHitAniType.Type2;
        SoundMgr.PlaySound("Razor2", ESoundType.Motion);
    }
    public override void Combo3Damage()
    {
        Vector3 TargetVec = RotateAngle == 0.0f ? gameObject.transform.position + Vector3.right * AttackRange : gameObject.transform.position + Vector3.left * AttackRange;
        GameObject LoadedObj = Instantiate<GameObject>(CResourceMgr.LoadEffect("IceBolts"), gameObject.transform.position, new Quaternion());
        CIceBolts Arrows = LoadedObj.GetComponent<CIceBolts>();
        Arrows.Owner = gameObject;
        Arrows.EndPos = TargetVec;
        CCombo3Ani ani = gameObject.GetComponentInChildren<CCombo3Ani>();
        Arrows.HitMove = ani.HitMove;
        Arrows.HitAniType = EHitAniType.Type3;
        SoundMgr.PlaySound("Razor", ESoundType.Motion);
    }
}
