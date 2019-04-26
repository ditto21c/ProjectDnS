using UnityEngine;
//using UnityEditor;

public class CDieEffect : CBaseCharacterEffect
{
    public override void Update()
    {
        if (continueTime <= 0.0f)
            return;

        SpriteRenderer render = TargetGameObject.GetComponent<SpriteRenderer>();
        Color color = render.color;
        color.a -= Time.fixedDeltaTime; 
        color.a = Mathf.Max(0.0f, color.a);
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
        Color color = render.color;
        color.a = 1.0f;
    }

    void End()
    {
        SpriteRenderer render = TargetGameObject.GetComponent<SpriteRenderer>();
        Color color = render.color;
        color.a = 0.0f;
    }
}