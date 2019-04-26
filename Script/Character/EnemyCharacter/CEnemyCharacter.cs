using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemyCharacter : CBaseCharacter
{
    public override bool Hit(int inDamage, GameObject Attacker, float HitMove, bool bSturn, EHitAniType HitAniType) 
    {
        CDebugLog.Log(ELogType.Character, "Hit EnemyCharacter");

        bool bHit = base.Hit(inDamage, Attacker, HitMove, bSturn, HitAniType);

        if (bHit)
        {
            if (curState == EState.Die)
                SoundMgr.PlaySound("Die2", ESoundType.Motion);
            else
                SoundMgr.PlaySound("Digital_Sword", ESoundType.Motion);
        }

        return bHit;
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (curState == EState.Die)
            return;

        string OtherTag = other.gameObject.tag;

        if (OtherTag == "EnemyCharacter" || OtherTag == "PlayerCharacter")
        {
            if (OtherTag == "EnemyCharacter")
            {
                Idle();
            }
            Stop();
        }
    }
    
    protected int GetAttackDamage()
    {
        int damagae = 1;
        
        return damagae;
    }

    public override bool FindNearestTarget()
    {
        if (TargetObj) return true;

        bool bFinded = false;
        float CheckTargetDist = 100.0f;
        Vector3 CurPos = gameObject.transform.position;
        GameObject[] FindObjs = GameObject.FindGameObjectsWithTag("PlayerCharacter");
        foreach (GameObject FindObj in FindObjs)
        {
            CPlayerCharacter FindChar = FindObj.GetComponent<CPlayerCharacter>();
            if (FindChar.curState != EState.Die)
            {
                float TargetDist = Vector3.Distance(FindObj.transform.position, CurPos);
                if (TargetDist < CheckTargetDist && TargetDist <= EyeSight)
                {
                    CheckTargetDist = TargetDist;
                    TargetObj = FindObj;
                    bFinded = true;
                }
            }
        }
        return bFinded;
    }

    public override void ComboEnable()
    {
        base.ComboEnable();

        if(0 == Random.Range(0, 2))
            Attack1();
    }
}
