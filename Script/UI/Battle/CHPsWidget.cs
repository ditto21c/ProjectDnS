using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CHPsWidget : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Player == null)
            Player = GameObject.FindGameObjectWithTag("PlayerCharacter").GetComponent<CPlayerCharacter>();

        if (Player == null)
            return;

        int CurHP = Player.GetCurHP();
        int MaxHP = Player.GetMaxHP();
        HPWidgets = GetComponentsInChildren<CHPWidget>();

        int MaxHPCount = MaxHP / 10;
        int CurHPCount = CurHP / 10;
        int RemainHP = CurHP % 10;

        for (int i = 0; i < 10; ++i)
        {
            bool bEnable = i < MaxHPCount ? true : false;
            HPWidgets[i].enabled = bEnable;
            HPWidgets[i].gameObject.GetComponent<Image>().enabled = bEnable;
        }

        for (int i = 0; i < MaxHPCount; ++i)
        {
            if (i < CurHPCount)
                HPWidgets[i].SetImage(CHPWidget.EHPWidgetType.Full);
            else if (i == CurHPCount)
            { 
                if (5 < RemainHP)
                    HPWidgets[i].SetImage(CHPWidget.EHPWidgetType.NotFull);
                else if (0 < RemainHP)
                    HPWidgets[i].SetImage(CHPWidget.EHPWidgetType.Half);
                else
                    HPWidgets[i].SetImage(CHPWidget.EHPWidgetType.Zero);
            }
            else
                HPWidgets[i].SetImage(CHPWidget.EHPWidgetType.Zero);
        }

    }

    CHPWidget[] HPWidgets;
    CPlayerCharacter Player = null;
}
