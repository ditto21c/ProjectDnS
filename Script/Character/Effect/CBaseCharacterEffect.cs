using UnityEngine;

public class CBaseCharacterEffect : Object
{
    public virtual void Update()
    {
    }

    protected GameObject TargetGameObject;
    protected float startTime;
    public float continueTime;
}