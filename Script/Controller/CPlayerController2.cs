using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerController2 : CBaseController
{

    private void Update()
    {
        Vector3 startPos = new Vector3();
        float AxisX = Input.GetAxis("JoystickAxisX");
        float AxisY = Input.GetAxis("JoystickAxisY");

        float AxisXRight = Input.GetAxis("JoystickAxisXRight");
        float AxisYRight = Input.GetAxis("JoystickAxisYRight");

        if (-0.5f < AxisX && AxisX < 0.5f)
            AxisX = 0.0f;
        else
            AxisX = 0 < AxisX ? 1.0f : -1.0f;

        if (-0.5f < AxisY && AxisY < 0.5f)
            AxisY = 0.0f;
        else
            AxisY = 0 < AxisY ? 1.0f : -1.0f;

        startPos.x = AxisX;
        startPos.y = AxisY;

        if (0.0f < startPos.x || 0.0f < startPos.y)
            CachedMovePos = startPos;

        Vector3 EndPos = gameObject.transform.position + startPos;
        Character.Move(EndPos);

        if (-0.5f < AxisXRight && AxisXRight < 0.5f)
            AxisXRight = 0.0f;
        else
            AxisXRight = 0 < AxisXRight ? 0.3f : -0.3f;

        if (-0.5f < AxisYRight && AxisYRight < 0.5f)
            AxisYRight = 0.0f;
        else
            AxisYRight = 0 < AxisYRight ? 0.3f : -0.3f;

        string str = "Axis {0}  {1}  {2}  {3}";
        object[] param = { AxisX, AxisY, AxisXRight, AxisYRight };
        CDebugLog.LogFormat(ELogType.Default, str, param);

        Vector3 CameraPos = new Vector3(AxisXRight, AxisYRight);
        Transform transform = Camera.main.transform;
        Vector3 position = transform.position;
        position += CameraPos;
        Camera.main.transform.position = position;

        if (Character.curState == EState.Die)
            return;

        if (Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            Character.Attack1();
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            Character.Attack2();
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            Character.Skill();
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            Character.Dodge(CachedMovePos);
        }

    }
    Vector3 CachedMovePos;
}
