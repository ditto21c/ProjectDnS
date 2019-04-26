using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CRealTreasureChest : CTreasureChest
{
    new void Start()
    {
        base.Start();
    }
    public override void Write(ref BinaryWriter binaryWriter)
    {
        base.Write(ref binaryWriter);

        binaryWriter.Write(Gold);
       
    }
    public override void Read(ref BinaryReader binaryReader)
    {
        base.Read(ref binaryReader);

        Gold = binaryReader.ReadInt32();
        
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
        if (0 < Gold && HP < DefaultHP / 2)
        {
            SpriteRender.sprite = NextSprites[0];
            Rigidbody2D Rigidbody = ChildObject.GetComponent<Rigidbody2D>();
            Rigidbody.simulated = true;
        }
    }
    public void OnNotifyTrigger()
    {
        SpriteRender.sprite = NextSprites[1];
        Rigidbody2D Rigidbody = ChildObject.GetComponent<Rigidbody2D>();
        Rigidbody.simulated = false;

        CGameInstance GameInstance = GameObject.FindGameObjectWithTag("GameInstance").GetComponent<CGameInstance>();
        GameInstance.Inventory.AddGold(Gold);

        Gold = 0;
    }

    public int Gold;
    public GameObject ChildObject;

}
