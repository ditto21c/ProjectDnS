using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSturnAni : CBaseAni
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
                break;
            case EEventFrame.Effect:
                break;
            case EEventFrame.Camera:
                break;
        }
    }
}

