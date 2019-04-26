using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ELogType
{
    Default = 0,
    Character,
    AStar,
    MapGenerator,
    GameMode,
    MapObject,
    Battle,
    Animation,
};

public class CDebugLog : UnityEngine.Object
{
    static bool[] bEnableLog =
    {
        false,
        false,
        true,
        false,
        false,
        false,
        false,
        false,
    };

    public static void LogFormat(ELogType Type, string format, params object[] args)
    {
        if(bEnableLog[(int)Type] == true)
        {
            string TypeStr = GetLogTypeStr(Type);
            format = TypeStr + " " + format;
            Debug.LogFormat(format, args);
        }
    }

    public static void Log(ELogType Type, object message)
    {
        if(bEnableLog[(int)Type] == true)
        {
            string TypeStr = GetLogTypeStr(Type);
            string format = "{0} {1}";
            object[] args = {TypeStr, message};
            
            Debug.LogFormat(format, args);
        }
    }

    static string GetLogTypeStr(ELogType Type)
    {
        switch(Type)
        {
            case ELogType.AStar:
                return "[AStar]";
            case ELogType.Battle:
                return "[Battle]";
            case ELogType.Character:
                return "[Character]";
            case ELogType.GameMode:
                return "[GameMode]";
            case ELogType.MapGenerator:
                return "[MapGenerator]";
            case ELogType.MapObject:
                return "[MapObject]";
            case ELogType.Animation:
                return "[Animation]";
        };

        return "[Default]";
    }
}
