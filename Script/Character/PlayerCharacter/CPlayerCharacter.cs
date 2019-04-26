using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CPlayerCharacter : CBaseCharacter
{
    new protected void Update()
    {
        base.Update();
        
        if(Stamina < 100.0f)
        {
            Stamina += Time.fixedDeltaTime * 5.0f;
            Stamina = Mathf.Min(Stamina, 100.0f);
        }
    }

    public override bool Hit(int inDamage, GameObject Attacker, float HitMove, bool bSturn, EHitAniType HitAniType)
    {
        CDebugLog.Log(ELogType.Character, "Hit PlayerCharacter");

        bool bHit = base.Hit(inDamage, Attacker, HitMove, bSturn, HitAniType);
        if (bHit)
        {
            if (curState == EState.Die)
                SoundMgr.PlaySound("Die1", ESoundType.Motion);
            else
            {
                SoundMgr.PlaySound("Digital_Sword", ESoundType.Motion);
                BattleGameMode.OnPlayerCharacterHit(this);
            }

            
        }
        return bHit;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (curState == EState.Die)
            return;

        string tag = collision.gameObject.tag;
        if (tag == "EnemyCharacter" || tag == "PlayerCharacter")
        {
            if (tag == "EnemyCharacter")
            {
                CBaseCharacter character = collision.gameObject.GetComponent<CBaseCharacter>();
                if (character.curState != EState.Die)
                    Idle();
            }
            Stop();
        }
    }

    public override bool FindNearestTarget()
    {
        if (TargetObj) return true;

        bool bFinded = false;
        float CheckTargetDist = 100.0f;
        Vector3 CurPos = gameObject.transform.position;

        GameObject[][] FindedObjs = new GameObject[2][];
        FindedObjs[0] = GameObject.FindGameObjectsWithTag("PlayerCharacter");
        FindedObjs[1] = GameObject.FindGameObjectsWithTag("EnemyCharacter");

        for (int i = 0; i < 2; ++i)
        {
            for (int j = 0; j < FindedObjs[i].Length; ++j)
            {
                if (FindedObjs[i][j].GetInstanceID() == gameObject.GetInstanceID())
                    continue;

                CBaseCharacter FindChar = FindedObjs[i][j].GetComponent<CBaseCharacter>();
                if (FindChar.curState != EState.Die)
                {
                    // PlayerCharacter 일때, 팀체크
                    if (i == 0 && FindChar.TeamType == TeamType)
                        continue;

                    float TargetDist = Vector3.Distance(FindedObjs[i][j].transform.position, CurPos);
                    if (TargetDist < CheckTargetDist && TargetDist <= EyeSight)
                    {
                        CheckTargetDist = TargetDist;
                        TargetObj = FindedObjs[i][j];
                        bFinded = true;
                    }
                }
            }
        }
        return bFinded;
    }

    public override void DestroyCharacter()
    {
        CBattleGameMode BattleGameMode = GameObject.FindGameObjectWithTag("Mode").GetComponent<CBattleGameMode>();
        BattleGameMode.OnPlayerCharacterDie(this);

        base.DestroyCharacter();
    }
    public float GetStamina() { return Stamina; }
    public int GetSpecialPoint() { return SpecialPoint; }

    public override bool Dodge(Vector3 GoalPos)
    {
        if (Stamina < 40.0f)
            return false;

        bool bDodge = base.Dodge(GoalPos);
        if (bDodge)
        {
            Stamina -= 40.0f;
            SoundMgr.PlaySound("Jump", ESoundType.Motion);
        }
        return bDodge;
    }

    public override void SpecialPointEnable()
    {
        bSpecialPoint = true;
    }
    public override void SpecialPointDisable()
    {
        bSpecialPoint = false;
    }

    public override void HitedTarget(bool bHit)
    {
        if(bHit)
        {
            if (bReservedSpecialPoint)
            {
                SpecialPoint += 10;
                SpecialPoint = Mathf.Min(SpecialPoint, 100);
            }
        }
        bReservedSpecialPoint = false;
    }

    float Stamina = 100.0f;
    int SpecialPoint = 0;

    protected bool bSpecialPoint = false;
    protected bool bReservedSpecialPoint = false;

}