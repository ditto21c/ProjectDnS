using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSkillAni : CBaseAni
{
    protected new void Start()
    {
        base.Start();
        DefaultPos = gameObject.transform.localPosition;
    }
    public override void FrameEvent(int Frame)
    {
        switch (EventFrames[Frame])
        {
            case EEventFrame.Damage:
                {
                    CBaseCharacter Character = gameObject.GetComponentInParent<CBaseCharacter>();
                    Character.SkillDamage();
                }
                break;
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
