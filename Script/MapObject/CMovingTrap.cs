using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CMovingTrap : CMapObject
{
    // Start is called before the first frame update
    void Start()
    {
        Type = (byte)EMapObjectType.Trap;
        Collider = GetComponent<CircleCollider2D>();
        Sprite = GetComponent<SpriteRenderer>();
        InvokeRepeating("ChangeSprite", 0.3f, 0.3f);

        //DefaultPos = gameObject.transform.position;
        //DirVec = DefaultGoalPos - DefaultPos;
        //DirVec.Normalize();
        //GoalPos = DefaultGoalPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (CMapGenerator.bTool)
            return;

        if (Vector3.Distance(gameObject.transform.position, GoalPos) < 0.1f)
        {
            if (GoalPos == DefaultGoalPos)
            {
                DirVec = DefaultPos - DefaultGoalPos;
                DirVec.Normalize();
                GoalPos = DefaultPos;
            }
            else
            {
                DirVec = DefaultGoalPos - DefaultPos;
                DirVec.Normalize();
                GoalPos = DefaultGoalPos;
            }
        }

        gameObject.transform.Translate(DirVec * Time.deltaTime * Speed, Space.World);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "PlayerCharacter" || tag == "EnemyCharacter")
        {
            CDebugLog.Log(ELogType.MapObject, "OnTriggerEnter2D CMovingTrap");

            CBaseCharacter character = collision.gameObject.GetComponent<CBaseCharacter>();
            if (character.curState == EState.Dodge || character.curState == EState.Die)
                return;

            character.Hit(Damage, gameObject, HitMove, false, EHitAniType.Type1);
        }
    }

    public override void Write(ref BinaryWriter binaryWriter)
    {
        base.Write(ref binaryWriter);

        binaryWriter.Write(DefaultGoalPos.x);
        binaryWriter.Write(DefaultGoalPos.y);
        binaryWriter.Write(Speed);
    }

    public override void Read(ref BinaryReader binaryReader)
    {
        base.Read(ref binaryReader);

        DefaultGoalPos.x = binaryReader.ReadSingle();
        DefaultGoalPos.y = binaryReader.ReadSingle();
        DefaultGoalPos.z = DefaultPosZ;
        Speed = binaryReader.ReadSingle();

        DefaultPos = gameObject.transform.position;
        DirVec = DefaultGoalPos - DefaultPos;
        DirVec.Normalize();
        GoalPos = DefaultGoalPos;
    }

    public void ChangeSprite()
    {
        Sprite.sprite = Sprites[SpriteIdx];
        if ((Sprites.Length -1) == SpriteIdx)
            SpriteIdx = 0;
        else
            ++SpriteIdx;
    }

    CircleCollider2D Collider;
    SpriteRenderer Sprite;
    public Sprite[] Sprites;
    int SpriteIdx = 0;
    public int Damage = 99999;
    public float HitMove = 0.0f;
    public float Speed = 3.0f;
    public Vector3 DefaultGoalPos;
    Vector3 DefaultPos;
    Vector3 DirVec;
    Vector3 GoalPos;

}
