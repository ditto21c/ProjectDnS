using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAttackAni : CBaseAni
{
    public override void FrameEvent(int Frame)
    {
        switch (EventFrames[Frame])
        {
            case EEventFrame.Damage:
                break;
            case EEventFrame.Effect:
                break;
            case EEventFrame.Camera:
                break;
        }
    }
}
