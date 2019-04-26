using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDefenceAni : CBaseAni
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
            case EEventFrame.Effect:
                {
                    CBaseCharacter Character = gameObject.GetComponentInParent<CBaseCharacter>();
                    Character.DefenceEffect();
                }
                break;
            case EEventFrame.ParringEnable:
                {
                    CBaseCharacter Character = gameObject.GetComponentInParent<CBaseCharacter>();
                    Character.ParringEnable();
                }
                break;
            case EEventFrame.ParringDisable:
                {
                    CBaseCharacter Character = gameObject.GetComponentInParent<CBaseCharacter>();
                    Character.ParringDisable();
                }
                break;
            case EEventFrame.ParringEnd:
                {
                    CBaseCharacter Character = gameObject.GetComponentInParent<CBaseCharacter>();
                    Character.ParringEnd();
                }
                break;
        }
    }
}
