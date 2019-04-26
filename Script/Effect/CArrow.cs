using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CArrow : CProjectile
{
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
        else if("BreakBlock" == OtherTag)
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
}
