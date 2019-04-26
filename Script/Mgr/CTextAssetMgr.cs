using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTextAssetMgr : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    void Load(TextAsset TextAsset)
    {
        Stream stream = new MemoryStream(TextAsset.bytes);
        BinaryReader BinaryReader = new BinaryReader(stream);
    }

    public BinaryReader LoadTextAsset(EFileType Type, string FileName)
    {
        TextAsset Asset = GetAsset(Type, FileName);
        if (Asset == null)
            return null;
        Stream stream = new MemoryStream(Asset.bytes);
        return new BinaryReader(stream);
    }

    TextAsset GetAsset(EFileType Type, string FileName)
    {
        switch(Type)
        {
            case EFileType.Map:
                {
                    for(int i=0; i< MapFiles.Length; ++i)
                    {
                        if (MapFiles[i].ToString() == FileName)
                            return MapFiles[i];
                    }
                }
                break;
            case EFileType.MapWP:
                return MapFile_WP;
            case EFileType.MapWPP:
                return MapFile_WPP;
        }
        return null;
    }

    public enum EFileType
    {
        Map = 0,
        MapWP,
        MapWPP,
    }

    public TextAsset[] MapFiles;
    public TextAsset MapFile_WP;
    public TextAsset MapFile_WPP;
}
