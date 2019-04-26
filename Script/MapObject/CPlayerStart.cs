using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CPlayerStart : CMapObject {

	// Use this for initialization
	void Start () {
        Type = (byte)EMapObjectType.PlayerStart;
	}
	
	// Update is called once per frame
	void Update () {
		
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
