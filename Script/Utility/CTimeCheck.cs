using UnityEngine;
using System;
using System.Collections;

public class CTimeCheck : UnityEngine.Object
{
    public static void Start(int InID = 0)
    {
        check_start_time = Environment.TickCount;
        ID = InID;
    }

    public static void End(ELogType log_type, string time_check_name)
    {
        int elapsed_time = Environment.TickCount - check_start_time;
        object[] arg = { time_check_name, elapsed_time, ID };
        CDebugLog.LogFormat(log_type, "{0} elapsed_time = {1}ms [ID:{2}]", arg);
    }
    static int check_start_time = 0;
    static int ID;
}
