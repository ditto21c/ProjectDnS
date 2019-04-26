using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CParringAttackAni : CBaseAni
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
                CBaseCharacter Character = gameObject.GetComponentInParent<CBaseCharacter>();
                Character.ParringAttackDamage();
                break;
            case EEventFrame.Effect:
                break;
            case EEventFrame.Camera:
                break;
        }
    }
}
