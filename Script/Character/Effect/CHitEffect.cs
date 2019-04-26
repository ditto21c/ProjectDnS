using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class CHitEffect : CBaseCharacterEffect
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
        Color color = Color.cyan;
        render.color = color;
    }

    IEnumerator Fade()
    {
        //CDebugLog.Log(ELogType.Default, "Fade Function End");

        SpriteRenderer render = TargetGameObject.GetComponent<SpriteRenderer>();

        float i = 0; 
        for (float f = 1f; f >= 0; f -= 0.1f)
        {
            Color c = render.color;
            c.a = f;
            render.color = c;
            i = f;
            CDebugLog.Log(ELogType.Default, i);
            yield return new WaitForSeconds(.1f);
        }


    }


    void End()
    {
        SpriteRenderer render = TargetGameObject.GetComponent<SpriteRenderer>();
        Color color = Color.white;
        render.color = color;
    }
}