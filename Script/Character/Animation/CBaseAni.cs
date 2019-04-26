using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBaseAni : MonoBehaviour
{
    protected void Start()
    {
        AllAniTime = EventFrames.Length / 60.0f;
        Character = gameObject.GetComponentInParent<CBaseCharacter>();
        spriteRenderer = (SpriteRenderer)gameObject.GetComponent("SpriteRenderer");
        SoundMgr = GameObject.FindGameObjectWithTag("SoundMgr").GetComponent<CSoundMgr>();
    }

    public void InitAngle(float Angle)
    {
        spriteRenderer.flipX = 0.0f == Angle ? true : false;
        Vector3 NewPos = DefaultPos;
        NewPos.x = 0.0f == Angle ? -DefaultPos.x : DefaultPos.x;
        transform.localPosition = NewPos;
        CharRotateAngle = Angle;
    }

    protected void Update()
    {
        if (EventFrames.Length == 0) return;

        if (bPlayingAni)
        {
            float CurTime = Time.time - AniStartTime;
            int CurFrame = (int)(CurTime * EventFrames.Length / AllAniTime);
            if (CurFrame < EventFrames.Length)
            {
                // 프레임 이벤트 중복 체크
                if (CurFrame != PreFrame)
                {
                    for (int i = PreFrame + 1; i <= CurFrame; ++i)
                    {
                        FrameEvent(i);
                    }

                    PreFrame = CurFrame;

                    string str = "CurFrame{0} CurTime{1}";
                    object[] args = { CurFrame, CurTime};
                    CDebugLog.LogFormat(ELogType.Animation, str, args);
                }

                if (CurFrame < Sprites.Length)
                {
                    if (Sprites[CurFrame] != null)
                        spriteRenderer.sprite = Sprites[CurFrame];
                }
                if(CurFrame < SpritesAngles.Length)
                {
                    float Angle = CharRotateAngle == -1.0f ? SpritesAngles[CurFrame] : CharRotateAngle == 0.0f ? -SpritesAngles[CurFrame] : SpritesAngles[CurFrame];
                    Vector3 eulerAngles = new Vector3(0.0f, 0.0f, Angle);
                    Quaternion newRotation = Quaternion.Euler(eulerAngles);
                    gameObject.transform.rotation = newRotation;
                }
            }
            else
            {
                if (bLoopAni)
                {
                    AniStartTime = Time.time;
                }
                else
                {
                    bPlayingAni = false;
                    spriteRenderer.sprite = null;

                    if (EState.Die == Character.curState)
                    {
                        Character.DestroyCharacter();
                    }
                    else
                    {
                        Character.Idle();
                    }
                }
            }
        }
    }
    public void PlayAni()
    {
        bPlayingAni = true;
        AniStartTime = Time.time;
        PreFrame = -1;

        if(CharRotateAngle == -1.0f)
            gameObject.transform.localPosition = DefaultPos;
    }
    public void StopAni()
    {
        bPlayingAni = false;
        PreFrame = -1;
    }
    public virtual void FrameEvent(int Frame)
    {
        //switch (EventFrames[Frame])
        //{
        //    case EEventFrame.Damage:
        //        break;
        //    case EEventFrame.Effect:
        //        break;
        //    case EEventFrame.Camera:
        //        break;
        //}
    }
    public void SpriteRendererNone()
    {
        spriteRenderer.sprite = null;
    }

    public Sprite[] Sprites;                // 스프라이트 이미지
    public float[] SpritesAngles;                // 스프라이트 이미지
    public EEventFrame[] EventFrames;       // 프레임 이벤트
    public float AniStartTime = 0;          // 애니 시작 시간
    public bool bLoopAni = false;           // 애니메이션을 루프 시킬것인가?
    public bool bPlayingAni = false;        // 애니메이션 중인가?
    public float AllAniTime = 0.0f;         // 60프레임 1초 기준으로 총 시간 (s)
    int PreFrame = -1;

    protected CBaseCharacter Character;
    protected SpriteRenderer spriteRenderer;
    protected Vector3 DefaultPos;
    protected float CharRotateAngle = -1.0f;
    protected CSoundMgr SoundMgr;
    
}
