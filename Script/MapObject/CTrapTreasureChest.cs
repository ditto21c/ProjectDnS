using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CTrapTreasureChest : CTreasureChest
{
    new void Start()
    {
        base.Start();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string Tag = collision.gameObject.tag;
        if (Tag == "DamageTrigger")
        {
            CDamageTrigger DamageTrigger = collision.gameObject.GetComponent<CDamageTrigger>();
            Hit(DamageTrigger.Damage);
        }
    }
        
    public void Hit(int Damage)
    {
        HP -= Damage;
        if (HP < DefaultHP / 2)
        {
            SpriteRender.sprite = NextSprites[0];

            Vector3 pos = gameObject.transform.position;
            GameObject TriggerObj = Instantiate<GameObject>(CResourceMgr.LoadTrigger("TrapDamageTrigger"), pos, Quaternion.identity);
            CDamageTrigger Trigger = TriggerObj.GetComponent<CDamageTrigger>();
            Trigger.Owner = gameObject;
            Trigger.HitMove = 0.2f;
            Trigger.AliveTime = 0.5f;
            Trigger.Damage = 10;
            Trigger.ApplyAliveTime();

            Invoke("EndSprite", 0.5f);
        }
    }

    void EndSprite()
    {
        SpriteRender.sprite = NextSprites[1];
       
    }

    public override void Write(ref BinaryWriter binaryWriter)
    {
        base.Write(ref binaryWriter);
    }
    public override void Read(ref BinaryReader binaryReader)
    {
        base.Read(ref binaryReader);

    }

    
}
