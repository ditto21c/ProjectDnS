using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CStandAni : CBaseAni
{
    protected new void Start()
    {
        base.Start();

        DefaultPos = gameObject.transform.localPosition;
    }

    protected new void Update()
    {
        if (bPlayingAni)
        {
            CBaseCharacter Char = gameObject.GetComponentInParent<CBaseCharacter>();
            InitAngle(Char.RotateAngle);
            //spriteRenderer.flipX = 0.0f == Char.RotateAngle ? true : false;
            //Vector3 NewPos = DefaultPos;
            //NewPos.x = 0.0f == Char.RotateAngle ? -DefaultPos.x : DefaultPos.x;
            //transform.localPosition = NewPos;
            //CharRotateAngle = Char.RotateAngle;
        }

        base.Update();

        
    }

    public override void FrameEvent(int Frame)
    {
        
    }
}
