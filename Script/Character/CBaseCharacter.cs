using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public enum EReservedParringType
{
    None = 0,
    Start,
    Attack,
}

public class CBaseCharacter : MonoBehaviour {
    
    protected void Start () {
        CBaseGameMode GameMode = GameObject.FindGameObjectWithTag("Mode").GetComponent<CBaseGameMode>();
        if (GameMode != null)
        {
            AStar = new CAStar();
            bool[] Tiles = GameMode.GetMap().GetTiles();
            AStar.InitTiles(ref Tiles);

            //((CAStar2)AStar).WayPoints = GameMode.GetMap().WayPoints;
            //((CAStar2)AStar).WayPointPaths = GameMode.GetMap().WayPointPaths;

            BattleGameMode = GameMode as CBattleGameMode;
        }

        InitController();
        ID = GetInstanceID();
        CachedSpeed = Speed;
        SoundMgr = GameObject.FindGameObjectWithTag("SoundMgr").GetComponent<CSoundMgr>();

        Invoke("Idle", 0.1f);
    }

    
    protected void Update()
    {
        UpdateEffects();
        
    }

    private void FixedUpdate()
    {
        UpdateTransForm();
    }
    void UpdateTransForm()
    {
        if(MovingPos != NoneVec)
        {
            translation = MovingPos - gameObject.transform.position;
            translation.Normalize();
            
            Translate(translation);

            Vector3 cur_pos = gameObject.transform.position;

            if (Vector2.Distance(MovingPos, cur_pos) <= 0.1f)
            {
                gameObject.transform.position = MovingPos;
                MovingPos = NoneVec;
                Speed = CachedSpeed;
            }
            return;
        }

        if (isMoving)
        {
            Debug.DrawLine(gameObject.transform.position, NextGoalPos);
            if (Vector3.Distance(NextGoalPos, gameObject.transform.position) <= 0.3f)
            {
                gameObject.transform.position = NextGoalPos;
                isMoving = false;
            }
            else
            {
                Translate(translation);
            }
            
        }

        if (!isMoving && 0 < paths.Count)
        {
            int pos_index = (int)paths[0];
            ArrayList tiles = AStar.GetTiles();
            Tile tile = (Tile)tiles[pos_index];
            NextGoalPos.x = tile.PosX;
            NextGoalPos.y = tile.PosY;
            paths.RemoveAt(0);

            if (!bTeleport)
            {
                if(paths.Count == 0)
                {
                    NextGoalPos = GoalPos;
                    translation = NextGoalPos - gameObject.transform.position;
                }
                else
                {
                    translation = NextGoalPos - gameObject.transform.position;
                }
                
                translation.Normalize();
                Translate(translation);
                isMoving = true;
            }
        }
    }
    void UpdateEffects()
    {
        for (int i = 0; i < Effects.Count; ++i)
        {
            if (((CBaseCharacterEffect)Effects[i]).continueTime <= 0.0f)
            {
                Effects[i] = null;
                Effects.RemoveAt(i);
            }
            else
            {
                ((CBaseCharacterEffect)Effects[i]).Update();
            }
        }
    }
    public bool Move(Vector3 InStartPos, Vector3 InEndPos, bool bForce = false)
    {
        // rotate
        if (!bForce)
        {
            RotateAngle = 0.0f <= InEndPos.x - InStartPos.x ? 0.0f : 180.0f;
            SpriteRenderer sr = (SpriteRenderer)gameObject.GetComponent("SpriteRenderer");
            sr.flipX = 0.0f == RotateAngle ? true : false;
        }

       

        GoalPos = InEndPos;
        GoalPos.z = 0.0f;

        InStartPos.x = Mathf.Round(InStartPos.x);
        InStartPos.y = Mathf.Round(InStartPos.y);

        InEndPos.x = Mathf.Round(InEndPos.x);
        InEndPos.y = Mathf.Round(InEndPos.y);

        if (false == AStar.IsValidVec(ref InEndPos))
            return false;

        ArrayList tiles = AStar.GetTiles();
        int TileSize = (int)Mathf.Sqrt(tiles.Count);

        int StartIdx = (int)(InStartPos.x + InStartPos.y * (float)TileSize);
        int EndIdx = (int)(InEndPos.x + InEndPos.y * (float)TileSize);

        if (AStar.IsValidPosIndex(EndIdx) == false)
            return false;

        Tile CheckTile = (Tile)tiles[EndIdx];
        if (CheckTile.bBlock)
            return false;

        isMoving = false;
        if (StartIdx == EndIdx)
        {
            paths.Add(EndIdx);
        }
        else
        {
            paths = AStar.FindPaths(StartIdx, EndIdx, ID);
            if(0 < paths.Count)
                paths.RemoveAt(0);
        }
        bool bMove = 0 < paths.Count ? true : false;
        return bMove;
    }
    
    public bool Move(Vector3 InEndPos)
    {
        //if (curState != EState.Idle) return false;

        translation = InEndPos - gameObject.transform.position;
        translation.Normalize();
        translation *= 1.5f;
        Vector3 CalcGoalPos = translation * Time.deltaTime * Speed + gameObject.transform.position;

        if (!AStar.IsValidVec(ref CalcGoalPos))
            return false;

        int PosX = (int)Mathf.Round(CalcGoalPos.x);
        int PosY = (int)Mathf.Round(CalcGoalPos.y);

        ArrayList tiles = AStar.GetTiles();
        int TileSize = (int)Mathf.Sqrt(tiles.Count);

        int endPosIndex = PosX + PosY * TileSize;

        if (AStar.IsValidPosIndex(endPosIndex) == false)
            return false;

        Tile CheckTile = (Tile)tiles[endPosIndex];
        if (CheckTile.bBlock)
        {
            return false;
        }

        Stop();

        

        // rotate
        float MovingAngle = 0.0f <= translation.x ? 0.0f : 180.0f;
        Vector3 eulerAngles = new Vector3(0.0f, MovingAngle, 0.0f);
        Quaternion newRotation = Quaternion.Euler(eulerAngles);
        //gameObject.transform.SetPositionAndRotation(gameObject.transform.position, newRotation);
        RotateAngle = MovingAngle;

        SpriteRenderer sr = (SpriteRenderer)gameObject.GetComponent("SpriteRenderer");
        sr.flipX = 0.0f <= translation.x ? true : false;

        // move
        Translate(translation);

        return true;
    }
    public void Stop()
    {
        Speed = CachedSpeed;
        isMoving = false;
        paths.Clear();
    }
    public void RandomMove()
    {
        Vector3 EndPos = gameObject.transform.position + new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f), 0.0f);
        EndPos = CUtility.RoundVector3(EndPos);
        bool bMove = Move(gameObject.transform.position, EndPos);
        if (bMove)
        {
            curState = EState.Move;
        }
    }
    public virtual bool Hit(int inDamage, GameObject Attacker, float HitMove, bool bSturn, EHitAniType HitAniType)
    {
        if (curState == EState.Die)
            return false;

        if (bSuperArmor)
            return false;

        if (curState == EState.Defence)
        {
            if (bParringEnable)
            {
                ParringEffect();
                ReservedParringType = EReservedParringType.Start;
            }
            return false;
        }
        
        curHealthPoint -= inDamage;

        if (curHealthPoint <= 0.0f)
        {
            AllStopAni();
            CDieAni ani = gameObject.GetComponentInChildren<CDieAni>();
            ani.PlayAni();
            curState = EState.Die;

            //CDieEffect Effect = new CDieEffect();
            //Effect.Init(Time.fixedTime, 1.0f, gameObject);
            //Effects.Add(Effect);
            //Invoke("DestroyCharacter", 1.0f);

            CircleCollider2D collider = gameObject.GetComponent<CircleCollider2D>();
            collider.isTrigger = false;

            Stop();
        }
        else
        {
            if (bSturn)
            {
                AllStopAni();
                CSturnAni ani = gameObject.GetComponentInChildren<CSturnAni>();
                ani.PlayAni();
                curState = EState.Sturn;
            }
            else if (curState == EState.Idle)
            {
                AllStopAni();
                CBaseAni ani = null;
                if (EHitAniType.Type1 == HitAniType)
                    ani = gameObject.GetComponentInChildren<CHitAni>();
                else if(EHitAniType.Type2 == HitAniType)
                    ani = gameObject.GetComponentInChildren<CHit2Ani>();
                else 
                    ani = gameObject.GetComponentInChildren<CHit3Ani>();
                ani.PlayAni();
                curState = EState.Hit;
            }
            
            CHitEffect hitEffect = new CHitEffect();
            hitEffect.Init(Time.fixedTime, 1.0f, gameObject);
            Effects.Add(hitEffect);

            float DiffPosX = gameObject.transform.position.x - Attacker.transform.position.x;
            Vector3 GoalPos = 0 <= DiffPosX ? new Vector3(HitMove, 0.0f, 0.0f) : new Vector3(-HitMove, 0.0f, 0.0f);
            GoalPos = gameObject.transform.position + GoalPos;
            Move(gameObject.transform.position, GoalPos, true);
        }
        return true;
    }

    public void Idle()
    {
        curState = EState.Idle;

        AllStopAni();
        CStandAni ani = gameObject.GetComponentInChildren<CStandAni>();
        ani.InitAngle(RotateAngle);
        ani.PlayAni();
    }

    public void ParringEffect()
    {
        CParringEffect Effect = new CParringEffect();
        Effect.Init(Time.fixedTime, 0.3f, gameObject);
        Effects.Add(Effect);
    }
    public void DefenceEffect()
    {
        CDefenceEffect Effect = new CDefenceEffect();
        Effect.Init(Time.fixedTime, 0.6f, gameObject);
        Effects.Add(Effect);
    }
    public virtual void DestroyCharacter()
    {
        Destroy(gameObject);
    }
    private void OnBecameVisible()
    {
        isBecameVisible = true;
    }
    private void OnBecameInvisible()
    {
        isBecameVisible = false;
    }
    public void Teleport(bool inIsTeleport)
    {
        bTeleport = inIsTeleport;
        isMoving = false;
        paths.Clear();
    }
    public bool IsTeleport()
    {
        return bTeleport;
    }
    void InitController()
    {
        switch (controller_type)
        {
            case EControllerType.Player1:
                controller = gameObject.AddComponent<CPlayerController>();
                //controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<CPlayerController>();
                Camera camera = gameObject.GetComponentInChildren<Camera>();
                camera.enabled = true;
                ((CPlayerController)controller).SetCameara(camera);
                break;
            case EControllerType.Player2:
                controller = gameObject.AddComponent<CPlayerController2>();
                //controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<CPlayerController2>();
                break;
            case EControllerType.Melee:
                controller = gameObject.AddComponent<CMeleeAIController>();
                break;
            case EControllerType.Ranged:
                controller = gameObject.AddComponent<CRangedAIController>();
                break;
            case EControllerType.RunAway:
                controller = gameObject.AddComponent<CRunAwayAIController>();
                break;
        };
        controller.enabled = true;
        controller.Init(this);
    }
    virtual public bool FindNearestTarget() { return false; }
    public void MoveAttackRange()
    {
        Vector3 CurPos = gameObject.transform.position;
        Vector3 TargetPos = TargetObj.transform.position;
        float TargetDist = Vector3.Distance(CurPos, TargetPos);
        if (AttackRange < TargetDist)
        {
            Vector3 GoalPos = TargetPos - CurPos;
            float Rate = 1.0f - AttackRange / TargetDist;
            GoalPos = CurPos + (GoalPos * Rate);
            GoalPos = CUtility.RoundVector3(GoalPos);
            bool bMove = Move(CurPos, GoalPos);
            if (bMove)
            {
                curState = EState.Move;
            }
            else
            {
                for (int y = -1; y < 2; ++y)
                {
                    for (int x = -1; x < 2; ++x)
                    {
                        if (x != 0 && y != 0)
                        {
                            Vector3 AddPos = new Vector3(x, y, 0);
                            Vector3 NewGoalPos = GoalPos + AddPos;
                            NewGoalPos = CUtility.RoundVector3(NewGoalPos);
                            bool bNewMove = Move(CurPos, NewGoalPos);
                            if (bNewMove)
                            {
                                curState = EState.Move;
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
    virtual public void AttackTarget() { }
    virtual public void Attack1() { }
    virtual public void Attack2() { }
    virtual public void Skill() { }
    public virtual bool Dodge(Vector3 GoalPos)
    {
        if (curState != EState.Idle) return false;

        Vector3 Radius = new Vector3(0.5f, 0.5f, 0.0f);
        Radius.x = GoalPos.x == 0 ? 0 : 0 < GoalPos.x ? Radius.x : -Radius.x;
        Radius.y = GoalPos.y == 0 ? 0 : 0 < GoalPos.y ? Radius.y : -Radius.y;

        Vector3 EndPos = gameObject.transform.position;
        ArrayList tiles = AStar.GetTiles();
        int TileSize = (int)Mathf.Sqrt(tiles.Count);
        for (int i=0; i<2; ++i)
        {
             Vector3 NextPos = EndPos + GoalPos;
            int PosX = (int)Mathf.Round(NextPos.x /*+ Radius.x*/);
            int PosY = (int)Mathf.Round(NextPos.y /*+ Radius.y*/);

            if (AStar.IsValidVec(ref NextPos) == false)
                break;

            int EndPosIdx = PosX + PosY * TileSize;

            if (AStar.IsValidPosIndex(EndPosIdx) == false)
                break;

            Tile CheckTile = (Tile)tiles[EndPosIdx];
            if (CheckTile.bBlock)
                break;

            EndPos = NextPos;

        }
        Stop();

        MovingPos.x = EndPos.x;
        MovingPos.y = EndPos.y;

        AllStopAni();
        CDodgeAni ani = gameObject.GetComponentInChildren<CDodgeAni>();
        ani.PlayAni();
        curState = EState.Dodge;
        Speed = 10.0f;

        
        return true;
    }
    public virtual void Combo1Damage() { }
    public virtual void Combo2Damage() { }
    public virtual void Combo3Damage() { }
    public virtual void SkillDamage() { }
    public virtual void ComboEnable()  {     bComboEnable = true;   }
    public virtual void ComboDisable() {     bComboEnable = false;  }
    public virtual void ComboEnd() { }
    public virtual void ParringEnable() {   bParringEnable = true; }
    public virtual void ParringDisable() {  bParringEnable = false; }
    public virtual void ParringEnd() { }
    public virtual void ParringAttackDamage() { }
    public virtual void SpecialPointEnable() { }
    public virtual void SpecialPointDisable() { }
    public virtual void HitedTarget(bool bHit) { }

    protected void AllStopAni()
    {
        CBaseAni[] Anis = gameObject.GetComponentsInChildren<CBaseAni>();
        foreach (var Ani in Anis)
        {
            Ani.StopAni();
        }
    }
    public void PlayEffect_PreEffect()
    {
        float PreAttackStartTime = 0 < PreAttackTime ? Time.fixedTime : 0.0f;
        if (0 < PreAttackTime)
        {
            CPreAttackEffect preAttackEffect = new CPreAttackEffect();
            preAttackEffect.Init(PreAttackStartTime, PreAttackTime, gameObject);
            Effects.Add(preAttackEffect);
        }
    }
    void Translate(Vector3 InTranslate)
    {
        if (bLockTranslate)
            return;

        PrePos = gameObject.transform.position;

        gameObject.transform.Translate(InTranslate * Time.deltaTime * Speed, Space.World);
    }

    public int GetCurHP() { return curHealthPoint; }
    public int GetMaxHP() { return maxHealthPoint; }
    

    public void RotateTarget()
    {
        if (TargetObj == null)
            return;

        RotateAngle = 0.0f <= TargetObj.transform.position.x - gameObject.transform.position.x ? 0.0f : 180.0f;
        SpriteRenderer sr = (SpriteRenderer)gameObject.GetComponent("SpriteRenderer");
        sr.flipX = 0.0f == RotateAngle ? true : false;
    }

    Vector3 translation;
    public bool bTeleport = false;
    public float EyeSight = 5.0f;
    public float AttackRange = 1.5f;

    public bool isBecameVisible = false;

    public int curHealthPoint = 100;
    public int maxHealthPoint = 100;
    

    protected CBaseAStar AStar;
    public ArrayList paths = new ArrayList();
    public bool isMoving = false;

    public EControllerType controller_type;
    CBaseController controller;
    
    public EState curState;
    public float PreAttackTime = 0.0f;

    protected ArrayList Effects = new ArrayList();
    public int ID;
    public float Speed = 2.0f;
    float CachedSpeed = 0.0f;
    public float RotateAngle = 0.0f;
    public float DodgeDist = 3.0f;

    static Vector3 NoneVec = new Vector3(-1.0f, -1.0f, -1.0f);
    Vector3 MovingPos = NoneVec;
    public Vector3 GoalPos = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 NextGoalPos = new Vector3(0.0f, 0.0f, 0.0f);
    public int ComboCount = 0;
    public bool bComboEnable = false;
    public bool bReservedCombo = false;
    
    public bool bParringEnable = false;
    public EReservedParringType ReservedParringType = EReservedParringType.None;
    public bool bSuperArmor = false;
    public ETeamType TeamType;
    public GameObject TargetObj;
    public bool bLockTranslate = false;
    public Vector3 PrePos = new Vector3();

    protected CSoundMgr SoundMgr;
    protected CBattleGameMode BattleGameMode;
    
}
