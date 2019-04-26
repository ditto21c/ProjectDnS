using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CBGMTrigger : CMapObject
{
    // Start is called before the first frame update
    void Start()
    {
        Type = (byte)EMapObjectType.BGMTrigger;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CSoundMgr SoundMgr = GameObject.Find("SoundMgr").GetComponent<CSoundMgr>();
        SoundMgr.PlaySound(PlayBGMName, ESoundType.BGM);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        CSoundMgr SoundMgr = GameObject.Find("SoundMgr").GetComponent<CSoundMgr>();
        SoundMgr.RestoreDefaltBGM();
    }

    public override void Write(ref BinaryWriter binaryWriter)
    {
        base.Write(ref binaryWriter);
        binaryWriter.Write(PlayBGMName);
    }

    public override void Read(ref BinaryReader binaryReader)
    {
        base.Read(ref binaryReader);
        PlayBGMName = binaryReader.ReadString();
    }

    public string PlayBGMName;
}
