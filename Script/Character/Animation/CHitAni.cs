using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHitAni : CBaseAni
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
            case EEventFrame.Damage:
                break;
            case EEventFrame.Effect:
                spriteRenderer.sprite = null;
                break;
            case EEventFrame.Camera:
                break;
        }
    }
}
