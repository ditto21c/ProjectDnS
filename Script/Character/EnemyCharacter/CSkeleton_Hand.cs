using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSkeleton_Hand : CMeleeEnemyCharacter
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
        if (curState == EState.Idle || curState == EState.Move)
        {
            AllStopAni();
            CCombo3Ani ani = gameObject.GetComponentInChildren<CCombo3Ani>();
            ani.InitAngle(RotateAngle);
            ani.PlayAni();

            Vector3 EndPos = gameObject.transform.position;
            EndPos.x = RotateAngle == 0.0f ? EndPos.x + ani.AttackMove : EndPos.x - ani.AttackMove;
            Move(gameObject.transform.position, EndPos);
        }
    }

    public override void Combo1Damage()
    {
        Vector3 TargetVec = RotateAngle == 0.0f ? gameObject.transform.position + Vector3.right * AttackRange : gameObject.transform.position + Vector3.left * AttackRange;
        Quaternion newRotation = CUtility.MakeQuaternion(gameObject.transform.position, TargetVec);

        // 이미지를 반시계방향으로 회전 시켜서 위로 바라 보게 만든다.
        Vector3 eulerAngles = newRotation.eulerAngles;
        eulerAngles.z += 90.0f;
        newRotation = Quaternion.Euler(eulerAngles);

        GameObject LoadedObj = Instantiate<GameObject>(CResourceMgr.LoadEffect("Bone"), gameObject.transform.position, newRotation);
        CBone Effect = LoadedObj.GetComponent<CBone>();
        Effect.Owner = gameObject;
        Effect.EndPos = TargetVec;
        CCombo1Ani ani = gameObject.GetComponentInChildren<CCombo1Ani>();
        Effect.HitMove = ani.HitMove;
        Effect.HitAniType = EHitAniType.Type1;

        //Vector3 TargetVec = RotateAngle == 0.0f ? gameObject.transform.position + Vector3.right : gameObject.transform.position + Vector3.left;
        //Quaternion newRotation = CUtility.MakeQuaternion(gameObject.transform.position, TargetVec);

        //Vector3 pos = gameObject.transform.position;
        //pos.x = gameObject.transform.position.x + (TargetVec.x - pos.x) / 2.0f;
        //pos.z = 1.0f;
        //GameObject melee_trail = Instantiate<GameObject>(CResourceMgr.LoadEffect("SwordTrail"), pos, newRotation);

        //CTrail trail = melee_trail.GetComponent<CTrail>();
        //trail.Owner = gameObject;
        //CCombo1Ani ani = gameObject.GetComponentInChildren<CCombo1Ani>();
        //trail.HitMove = ani.HitMove;
        //trail.HitAniType = EHitAniType.Type1;
        ani.SpriteRendererNone();

        SoundMgr.PlaySound("Attack", ESoundType.Motion);
    }
    public override void Combo2Damage()
    {
        Vector3 TargetVec = RotateAngle == 0.0f ? gameObject.transform.position + Vector3.right * AttackRange : gameObject.transform.position + Vector3.left * AttackRange;
        Quaternion newRotation = CUtility.MakeQuaternion(gameObject.transform.position, TargetVec);

        // 이미지를 반시계방향으로 회전 시켜서 위로 바라 보게 만든다.
        Vector3 eulerAngles = newRotation.eulerAngles;
        eulerAngles.z += 90.0f;
        newRotation = Quaternion.Euler(eulerAngles);

        GameObject LoadedObj = Instantiate<GameObject>(CResourceMgr.LoadEffect("Bone"), gameObject.transform.position, newRotation);
        CBone Effect = LoadedObj.GetComponent<CBone>();
        Effect.Owner = gameObject;
        Effect.EndPos = TargetVec;
        CCombo2Ani ani = gameObject.GetComponentInChildren<CCombo2Ani>();
        Effect.HitMove = ani.HitMove;
        Effect.HitAniType = EHitAniType.Type1;
        //Vector3 TargetVec = RotateAngle == 0.0f ? gameObject.transform.position + Vector3.right : gameObject.transform.position + Vector3.left;
        //Quaternion newRotation = CUtility.MakeQuaternion(gameObject.transform.position, TargetVec);
        //Vector3 pos = gameObject.transform.position;
        //pos.x = gameObject.transform.position.x + (TargetVec.x - pos.x) / 2.0f;
        //pos.z = 1.0f;
        //GameObject melee_trail = Instantiate<GameObject>(CResourceMgr.LoadEffect("MeleeTrail"), pos, newRotation);

        //CTrail trail = melee_trail.GetComponent<CTrail>();
        //trail.Owner = gameObject;
        //CCombo2Ani ani = gameObject.GetComponentInChildren<CCombo2Ani>();
        //trail.HitMove = ani.HitMove;
        //trail.HitAniType = EHitAniType.Type2;
        ani.SpriteRendererNone();

        SoundMgr.PlaySound("Attack2", ESoundType.Motion);
    }
    public override void Combo3Damage()
    {
        Vector3 TargetVec = RotateAngle == 0.0f ? gameObject.transform.position + Vector3.right : gameObject.transform.position + Vector3.left;
        Quaternion newRotation = CUtility.MakeQuaternion(gameObject.transform.position, TargetVec);
        Vector3 pos = gameObject.transform.position;
        pos.x = gameObject.transform.position.x + (TargetVec.x - pos.x) / 2.0f;
        pos.z = 1.0f;
        GameObject melee_trail = Instantiate<GameObject>(CResourceMgr.LoadEffect("SwordTrail"), pos, newRotation);
        melee_trail.transform.localScale = new Vector3(2.0f, 2.0f, 1.0f);
        CTrail trail = melee_trail.GetComponent<CTrail>();
        trail.Owner = gameObject;
        CCombo3Ani ani = gameObject.GetComponentInChildren<CCombo3Ani>();
        trail.HitMove = ani.HitMove;
        trail.HitAniType = EHitAniType.Type3;
        ani.SpriteRendererNone();

        SoundMgr.PlaySound("Attack", ESoundType.Motion);
    }


    
}
