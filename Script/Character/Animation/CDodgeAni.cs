using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDodgeAni : CBaseAni
{
    protected new void Start()
    {
        base.Start();
        DefaultPos = new Vector3(0.0f, 0.0f, -0.1f);
    }
    public override void FrameEvent(int Frame)
    {
        switch (EventFrames[Frame])
        {
            case EEventFrame.SuperArmorEnable:
                {
                    CBaseCharacter Character = gameObject.GetComponentInParent<CBaseCharacter>();
                    Character.bSuperArmor = true;
                }
                break;
            case EEventFrame.SuperArmorDisable:
                {
                    CBaseCharacter Character = gameObject.GetComponentInParent<CBaseCharacter>();
                    Character.bSuperArmor = false;
                }
                break;
        }
    }
}
