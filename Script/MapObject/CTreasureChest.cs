using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CTreasureChest : CMapObject
{
    protected void Start()
    {
        Type = (byte)EMapObjectType.TreasureChest;
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

    public Sprite[] NextSprites;
    public int DefaultHP = 30;
    public int HP = 30;
    protected SpriteRenderer SpriteRender;
}
