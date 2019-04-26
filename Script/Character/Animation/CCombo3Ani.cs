using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCombo3Ani : CBaseAni
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
                    Character.Combo3Damage();
                }
                break;
            case EEventFrame.Effect:
                break;
            case EEventFrame.Camera:
                break;
            case EEventFrame.ComboEnd:
                {
                    CBaseCharacter Character = gameObject.GetComponentInParent<CBaseCharacter>();
                    Character.ComboEnd();
                }
                break;
        }
    }
    public float AttackMove = 0.6f;
    public float HitMove = 1.0f;
   
}
