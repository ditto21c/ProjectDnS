using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBone : CProjectile
{
    protected new void Start()
    {
        base.Start();

        MaxSpriteCount = Sprite.Length;
        CurSpriteCount = 0;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Sprite[CurSpriteCount];
        ++CurSpriteCount;
        if (CurSpriteCount == MaxSpriteCount)
            CurSpriteCount = 0;
        InvokeRepeating("ChangeSprite", 0.3f, 0.3f);
    }

    void ChangeSprite()
    {
        base.Update();

        spriteRenderer.sprite = Sprite[CurSpriteCount];
        ++CurSpriteCount;
        if (CurSpriteCount == MaxSpriteCount)
            CurSpriteCount = 0;
    }

    protected new void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        string OtherTag = other.gameObject.tag;
        if ("PlayerCharacter" == OtherTag || "EnemyCharacter" == OtherTag)
        {
            if (Owner.GetInstanceID() != other.gameObject.GetInstanceID())
            {
                CBaseCharacter character = other.gameObject.GetComponent<CBaseCharacter>();
                character.Hit(Damage, Owner, HitMove, false, HitAniType);
                Destroy(gameObject);
            }
        }
        else if ("BreakBlock" == OtherTag)
        {
            CBreakBlock BreakBlock = other.gameObject.GetComponent<CBreakBlock>();
            BreakBlock.Hit(Damage);
            Destroy(gameObject);
        }
        else if ("RealTreasureChest" == OtherTag)
        {
            CRealTreasureChest RealTreasureChest = other.gameObject.GetComponent<CRealTreasureChest>();
            RealTreasureChest.Hit(Damage);
            Destroy(gameObject);
        }
        else if ("TrapTreasureChest" == OtherTag)
        {
            CTrapTreasureChest TrapTreasureChest = other.gameObject.GetComponent<CTrapTreasureChest>();
            TrapTreasureChest.Hit(Damage);
            Destroy(gameObject);
        }
    }

    public Sprite[] Sprite;
    int MaxSpriteCount = 0;
    int CurSpriteCount = 0;
    SpriteRenderer spriteRenderer;
}
