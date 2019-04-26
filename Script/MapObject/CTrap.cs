using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CTrap : CMapObject
{
    // Start is called before the first frame update
    void Start()
    {
        Type = (byte)EMapObjectType.Trap;
        BoxCollider = GetComponent<BoxCollider2D>();
        Sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "PlayerCharacter" || tag == "EnemyCharacter")
        {
            CDebugLog.Log(ELogType.MapObject, "OnTriggerEnter2D DownStairs");

            CBaseCharacter character = collision.gameObject.GetComponent<CBaseCharacter>();
            if (character.curState == EState.Dodge || character.curState == EState.Die)
                return;

            character.Hit(Damage, gameObject, HitMove, false, EHitAniType.Type1);
        }
    }

    public override void Write(ref BinaryWriter binaryWriter)
    {
        base.Write(ref binaryWriter);
    }

    public override void Read(ref BinaryReader binaryReader)
    {
        base.Read(ref binaryReader);
    }

    public void ExcuteTrigger()
    {
        if (!bStartTrigger)
        {
            bStartTrigger = true;
            BoxCollider.enabled = false;
            Invoke("BoxColliderEnable", DamageAbleTime);
        }
    }

    public void BoxColliderEnable()
    {
        BoxCollider.enabled = true;
        Invoke("BoxColliderDisable", DamageDisableTime);
        Invoke("AttackSprite", 0.1f);
        Sprite.sprite = Sprites[1];
    }
    public void AttackSprite()
    {
        Sprite.sprite = Sprites[2];
    }

    public void BoxColliderDisable()
    {
        Sprite.sprite = Sprites[0];
        BoxCollider.enabled = false;
        bStartTrigger = false;
    }

    BoxCollider2D BoxCollider;
    SpriteRenderer Sprite;
    public Sprite[] Sprites;
    public int Damage = 99999;
    public bool bStartTrigger = false;
    public float HitMove = 0.0f;
    public float DamageAbleTime = 2.0f;
    public float DamageDisableTime = 1.0f;

}
