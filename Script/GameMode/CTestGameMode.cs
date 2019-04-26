using UnityEngine;
using System;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CTestGameMode : CBaseGameMode
{
    private void Awake()
    {
        CDebugLog.Log(ELogType.GameMode, "TestGameMode Awake");
        Map.FileName = FileName;
        Map.OnAllLoad();

        CDebugLog.Log(ELogType.GameMode, "TestGameMode Map AllLoad");
    }
    // Use this for initialization
    new private void Start ()
    {
        base.Start();

        CDebugLog.Log(ELogType.GameMode, "TestGameMode Start");

        GameObject PlayerStart = GameObject.FindGameObjectWithTag("PlayerStart");
        Vector3 PlayerPosition = PlayerStart.transform.position;
        PlayerPosition.z = 0.0f;
        GameObject PlayerObj = Instantiate<GameObject>(CResourceMgr.LoadCharacter(player_name, ECharacterType.Player), PlayerPosition, new Quaternion());
        Player = PlayerObj.GetComponent<CBaseCharacter>();
        Vector3 position = Camera.main.transform.position;
        position.x = PlayerPosition.x;
        position.y = PlayerPosition.y;
        Camera.main.transform.position = position;

        InitUIKeyEvent();

        GameObject FindObj = GameObject.Find("TouchInput");
        DefaultTouchInputPos = FindObj.transform.position;
    }
	
    void InitUIKeyEvent()
    {
        Button FindButton = GameObject.Find("Button_A").GetComponent<Button>();
        FindButton.onClick.AddListener(() => OnButtonA());
        FindButton = GameObject.Find("Button_B").GetComponent<Button>();
        FindButton.onClick.AddListener(() => OnButtonB());
        FindButton = GameObject.Find("Button_C").GetComponent<Button>();
        FindButton.onClick.AddListener(() => OnButtonC());
        FindButton = GameObject.Find("Button_D").GetComponent<Button>();
        FindButton.onClick.AddListener(() => OnButtonD());
        Image FindImage = GameObject.Find("Touch").GetComponent<Image>();
        //FindImage.on.AddListener(() => OnDrag());
    }

	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            Transform transform = Camera.main.transform;
            Vector3 position = transform.position;
            position.x -= 1.0f;
            Camera.main.transform.position = position;
        }
        else if(Input.GetKey(KeyCode.DownArrow))
        {
            Transform transform = Camera.main.transform;
            Vector3 position = transform.position;
            position.y -= 1.0f;
            Camera.main.transform.position = position;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Transform transform = Camera.main.transform;
            Vector3 position = transform.position;
            position.x += 1.0f;
            Camera.main.transform.position = position;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            Transform transform = Camera.main.transform;
            Vector3 position = transform.position;
            position.y += 1.0f;
            Camera.main.transform.position = position;
        }

        if (Player == null)
            return;

        UpdateDrag();

        Map.RenderTileObject(Player.gameObject.transform.position);
    }

    void UpdateDrag()
    {
        if (!bDrag) return;

        if (Player.curState == EState.Idle)
        {
            Vector3 EndPos = Player.gameObject.transform.position + CachedMovePos;
            Player.Move(EndPos);
        }
    }
    public void PointerDown(BaseEventData InData)
    {
        PointerEventData Data = InData as PointerEventData;
        Vector3 Pos = Data.position;
        GameObject FindObj = GameObject.Find("TouchInput");
        FindObj.transform.position = Pos;
        DragStartPos = DefaultTouchInputPos;
        Vector3 MoveVec = Pos - DragStartPos;
        MoveVec.Normalize();

        if (Player.curState == EState.Idle)
        {
            CachedMovePos = MoveVec;
        }
        bDrag = true;
    }

    public void Drag(BaseEventData InData)
    {
        PointerEventData Data = InData as PointerEventData;
        Vector3 Pos = Data.position;
        GameObject FindObj = GameObject.Find("TouchInput");
        FindObj.transform.position = Pos;

        Vector3 MoveVec = Pos - DragStartPos;
        MoveVec.Normalize();

        if (Player.curState == EState.Idle)
        {
            CachedMovePos = MoveVec;
        }

        bDrag = true;
    }
    public void OnEndDrag()
    {
        if (Player == null)
            return;

        GameObject FindObj = GameObject.Find("TouchInput");
        FindObj.transform.position = DefaultTouchInputPos;

        Player.Stop();
        bDrag = false;
    }

    public void OnButtonA()
    {
        if (Player == null)
            return;

        if (Player.curState == EState.Die)
            return;

        CDebugLog.Log(ELogType.Default, "OnButtonA");
        Player.Attack1();
    }
    public void OnButtonB()
    {
        if (Player == null)
            return;

        if (Player.curState == EState.Die)
            return;

        Player.Attack2();
    }
    public void OnButtonC()
    {
        if (Player == null)
            return;

        if (Player.curState == EState.Die)
            return;

        Player.Skill();
    }
    public void OnButtonD()
    {
        if (Player == null)
            return;

        if (Player.curState == EState.Die)
            return;

        Player.Dodge(CachedMovePos);
    }

    CBaseCharacter Player;
    Vector3 CachedMovePos;
    Vector3 DragStartPos;
    bool bDrag = false;
    public string player_name = "SwordMan";
    Vector3 DefaultTouchInputPos = new Vector3();
}
