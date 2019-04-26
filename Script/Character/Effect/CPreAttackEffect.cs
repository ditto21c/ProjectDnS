using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPreAttackEffect : CBaseCharacterEffect
{
    public override void Update()
    {
        if (continueTime <= 0.0f)
            return;
        
        float preAttackElapsedtime = Time.fixedTime - startTime;
        float plusAlpha = preAttackElapsedtime * 0.3f / startTime;
        SpriteRenderer render = TargetGameObject.GetComponent<SpriteRenderer>();
        Color color = render.color;
        color.a = 0.5f + plusAlpha;
        render.color = color;

        continueTime -= Time.fixedDeltaTime;
        if (continueTime <= 0.0f)
            End();
    }

    public void Init(float InStartTime, float InContinueTime, GameObject InGameobject)
    {
        startTime = InStartTime;
        continueTime = InContinueTime;
        TargetGameObject = InGameobject;

        SpriteRenderer render = TargetGameObject.GetComponent<SpriteRenderer>();
        Color color = Color.red;
        render.color = color;
    }

    void End()
    {
        SpriteRenderer render = TargetGameObject.GetComponent<SpriteRenderer>();
        Color color = Color.white;
        render.color = color;
    }

}