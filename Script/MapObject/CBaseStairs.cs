using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CBaseStairs : CMapObject
{
	// Use this for initialization
	protected void Start () {
        if(CMapGenerator.bTool)
            Index = IndexCount++;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Write(ref BinaryWriter binaryWriter)
    {
        base.Write(ref binaryWriter);

        binaryWriter.Write(Index);
        binaryWriter.Write(LinkIndex);
    }
    public override void Read(ref BinaryReader binaryReader)
    {
        base.Read(ref binaryReader);

        Index = binaryReader.ReadByte();
        LinkIndex = binaryReader.ReadByte();
    }

    static byte IndexCount = 0;
    public byte Index;
    public byte LinkIndex;
    
    

}
