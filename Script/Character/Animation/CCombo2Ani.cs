using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCombo2Ani : CBaseAni
{
    protected new void Start()
    {
        base.Start();
        DefaultPos = gameObject.transform.localPosition;
        ComboEffectSpriteRenderer = (SpriteRenderer)ComboEffect.GetComponent("SpriteRenderer");
    }

    protected new void Update()
    {
        base.Update();
        if (EventFrames.Length == 0) return;

        if (bPlayingAni)
        {
            float CurTime = Time.time - AniStartTime;
            int CurFrame = (int)(CurTime * EventFrames.Length / AllAniTime);
            if (CurFrame < EventFrames.Length)
            {
                if (CurFrame < ComboEffectSprites.Length)
                {
                    if (ComboEffectSprites[CurFrame] != null)
                        ComboEffectSpriteRenderer.sprite = ComboEffectSprites[CurFrame];
                    else
                        ComboEffectSpriteRenderer.sprite = null;
                }
            }
            else
            {
                ComboEffectSpriteRenderer.sprite = null;
            }
        }
    }

    public override void FrameEvent(int Frame)
    {
        switch (EventFrames[Frame])
        {
            case EEventFrame.Damage:
                {
                    CBaseCharacter Character = gameObject.GetComponentInParent<CBaseCharacter>();
                    Character.Combo2Damage();
                }
                break;
            case EEventFrame.Effect:
                break;
            case EEventFrame.Camera:
                break;
            case EEventFrame.ComboEnable:
                {
                    CBaseCharacter Character = gameObject.GetComponentInParent<CBaseCharacter>();
                    Character.ComboEnable();
                }
                break;
            case EEventFrame.ComboDisable:
                {
                    CBaseCharacter Character = gameObject.GetComponentInParent<CBaseCharacter>();
                    Character.ComboDisable();
                }
                break;
            case EEventFrame.ComboEnd:
                {
                    CBaseCharacter Character = gameObject.GetComponentInParent<CBaseCharacter>();
                    Character.ComboEnd();
                }
                break;
            case EEventFrame.SpecialPointEnable:
                {
                    CBaseCharacter Character = gameObject.GetComponentInParent<CBaseCharacter>();
                    Character.SpecialPointEnable();
                }
                break;
            case EEventFrame.SpecialPointDisable:
                {
                    CBaseCharacter Character = gameObject.GetComponentInParent<CBaseCharacter>();
                    Character.SpecialPointDisable();
                }
                break;
        }
    }
    public float AttackMove = 0.4f;
    public float HitMove = 0.4f;
    public GameObject ComboEffect;
    SpriteRenderer ComboEffectSpriteRenderer;
    public Sprite[] ComboEffectSprites;
}
