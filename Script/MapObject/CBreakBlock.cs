using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CBreakBlock : CMapObject
{
    // Start is called before the first frame update
    void Start()
    {
        Type = (byte)EMapObjectType.BreakBlock;
        SpriteRender = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Write(ref BinaryWriter binaryWriter)
    {
        base.Write(ref binaryWriter);

        binaryWriter.Write(DefaultHP);
    }
    public override void Read(ref BinaryReader binaryReader)
    {
        base.Read(ref binaryReader);

        DefaultHP = binaryReader.ReadInt32();
        HP = DefaultHP;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string Tag = collision.gameObject.tag;
        if (Tag == "DamageTrigger")
        {
            CDamageTrigger DamageTrigger = collision.gameObject.GetComponent<CDamageTrigger>();
            Hit(DamageTrigger.Damage);
        }
        else if (Tag == "PlayerCharacter" || Tag == "EnemyCharacter")
        {
            CBaseCharacter Character = collision.gameObject.GetComponent<CBaseCharacter>();
            Character.Stop();
            collision.gameObject.transform.position = Character.PrePos;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        string Tag = collision.gameObject.tag;
        if (Tag == "PlayerCharacter" || Tag == "EnemyCharacter")
        {
            CBaseCharacter Character = collision.gameObject.GetComponent<CBaseCharacter>();
            Character.Stop();
            collision.gameObject.transform.position = Character.PrePos;
        }
    }


    public void Hit(int Damage)
    {
        HP -= Damage;
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
        else if (HP < DefaultHP / 2)
        {
            SpriteRender.sprite = NextSprite;
        }
    }

    public int DefaultHP = 30;
    private int HP = 30;
    private SpriteRenderer SpriteRender;
    public Sprite NextSprite;
   

}
