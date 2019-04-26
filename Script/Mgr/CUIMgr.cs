using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EUIType
{
    BattleWidget = 0,
}


public class CUIMgr : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject OpenUI(EUIType UIType)
    {
        string name = null;
        switch (UIType)
        {
            case EUIType.BattleWidget:
                name = "UI/Battle/BattleWidget";
                break;
        }

        return CResourceMgr.LoadUI(name);
    }
}
