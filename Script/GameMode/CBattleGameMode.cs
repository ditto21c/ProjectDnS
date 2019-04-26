using UnityEngine;
using System;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class CBattleGameMode : CBaseGameMode
{
    private void Awake()
    {
        CDebugLog.Log(ELogType.GameMode, "BattleGameMode Awake");
        Map.FileName = FileName;
        CMapGenerator.bTool = false;
        Map.bShowAllTile = false;
        Map.OnAllLoad();

        CDebugLog.Log(ELogType.GameMode, "BattleGameMode Map AllLoad");
    }
    // Use this for initialization
    new private void Start()
    {
        base.Start();

        CDebugLog.Log(ELogType.GameMode, "BattleGameMode Start");

        GameObject PlayerStart = GameObject.FindGameObjectWithTag("PlayerStart");
        Vector3 PlayerPosition = PlayerStart.transform.position;
        PlayerPosition.z = 0.0f;
        GameObject PlayerObj = Instantiate<GameObject>(CResourceMgr.LoadCharacter(PlayerName, ECharacterType.Player), PlayerPosition, new Quaternion());
        Player = PlayerObj.GetComponent<CBaseCharacter>();
        Vector3 position = Camera.main.transform.position;
        position.x = PlayerPosition.x;
        position.y = PlayerPosition.y;
        Camera.main.transform.position = position;

        //GameObject Canvas = GameObject.FindGameObjectWithTag("Canvas");
        //GameObject BattleWidget = UIMgr.OpenUI(EUIType.BattleWidget);
        //GameObject InsBattleWidget = Instantiate(BattleWidget);
        //InsBattleWidget.transform.parent = Canvas.transform;
        //Debug.Log(BattleWidget);

        InitUIKeyEvent();

        GameObject FindObj = GameObject.Find("TouchInput");
        DefaultTouchInputPos = FindObj.transform.position;

        
    }

    void InitUIKeyEvent()
    {
        EventTrigger eventTrigger = GameObject.Find("Button_A").GetComponent<EventTrigger>();
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => OnButtonAOnPointerDown());
        eventTrigger.triggers.Add(pointerDown);

        var pointerUp = new EventTrigger.Entry();
        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener((e) => OnButtonAOnPointerUp());
        eventTrigger.triggers.Add(pointerUp);

        EventTrigger eventTriggerB = GameObject.Find("Button_B").GetComponent<EventTrigger>();
        var pointerDownB = new EventTrigger.Entry();
        pointerDownB.eventID = EventTriggerType.PointerDown;
        pointerDownB.callback.AddListener((e) => OnButtonB());
        eventTriggerB.triggers.Add(pointerDownB);

        EventTrigger eventTriggerC = GameObject.Find("Button_C").GetComponent<EventTrigger>();
        var pointerDownC = new EventTrigger.Entry();
        pointerDownC.eventID = EventTriggerType.PointerDown;
        pointerDownC.callback.AddListener((e) => OnButtonC());
        eventTriggerC.triggers.Add(pointerDownC);

        EventTrigger eventTriggerD = GameObject.Find("Button_D").GetComponent<EventTrigger>();
        var pointerDownD = new EventTrigger.Entry();
        pointerDownD.eventID = EventTriggerType.PointerDown;
        pointerDownD.callback.AddListener((e) => OnButtonD());
        eventTriggerD.triggers.Add(pointerDownD);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Transform transform = Camera.main.transform;
            Vector3 position = transform.position;
            position.x -= 1.0f;
            Camera.main.transform.position = position;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
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
        UpdateOnButtonAOnPointerDown();

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
    void UpdateOnButtonAOnPointerDown()
    {
        if (!bButtonAOnPointerDown)
            return;

        if (Player.curState == EState.Die)
            return;

        Player.Attack1();
    }

    public void OnButtonAOnPointerDown()
    {
        if (Player == null) return;

        if (Player.curState == EState.Die)
            return;

        Player.Attack1();

        bButtonAOnPointerDown = true;
    }
    public void OnButtonAOnPointerUp()
    {
        bButtonAOnPointerDown = false;
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
        GameObject FindObj = GameObject.Find("TouchInput");
        FindObj.transform.position = DefaultTouchInputPos;
        bDrag = false;

        if (Player == null) return;

        Player.Stop();
    }

    public void OnButtonA()
    {
        if (Player == null) return;

        if (Player.curState == EState.Die)
            return;

        CDebugLog.Log(ELogType.Default, "OnButtonA");
        Player.Attack1();
    }
    public void OnButtonB()
    {
        if (Player == null) return;

        if (Player.curState == EState.Die)
            return;

        Player.Attack2();
    }
    public void OnButtonC()
    {
        if (Player == null) return;

        if (Player.curState == EState.Die)
            return;

        Player.Skill();
    }
    public void OnButtonD()
    {
        if (Player == null) return;

        if (Player.curState == EState.Die)
            return;

        Player.Dodge(CachedMovePos);
    }

    public void OnPlayerCharacterDie(CPlayerCharacter Character)
    {
        if(EControllerType.Player1 == Character.controller_type )
        {
            SceneManager.LoadScene("Lobby");
        }
    }

    public void OnPlayerCharacterHit(CPlayerCharacter Character)
    {
        if (EControllerType.Player1 == Character.controller_type)
        {

        }
    }

    CBaseCharacter Player;
    Vector3 CachedMovePos;
    Vector3 DragStartPos;
    bool bDrag = false;
    static public string PlayerName = "SwordMan";
    Vector3 DefaultTouchInputPos = new Vector3();
    bool bButtonAOnPointerDown = false;
}
