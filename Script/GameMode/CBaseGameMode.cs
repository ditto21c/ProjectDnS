using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBaseGameMode : MonoBehaviour
{
    protected void Start()
    {
        SoundMgr = GameObject.FindGameObjectWithTag("SoundMgr").GetComponent<CSoundMgr>();
        UIMgr = GameObject.FindGameObjectWithTag("UIMgr").GetComponent<CUIMgr>();

    }
    public CMapGenerator GetMap() { return Map; }

    public string FileName = "Map00";
    protected CMapGenerator Map = new CMapGenerator();
    protected CSoundMgr SoundMgr;
    protected CUIMgr UIMgr;
}
