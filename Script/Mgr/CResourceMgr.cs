using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CResourceMgr : UnityEngine.Object
{
    public static GameObject LoadMapObject(string name)
    {
        name = "MapObject/" + name;
        return Resources.Load<GameObject>(name);
    }

    public static GameObject LoadCharacter(string name, ECharacterType type)
    {
        switch (type)
        {
            case ECharacterType.Enemy:
                name = "Character/EnemyCharacter/" + name;
                break;
            case ECharacterType.Player:
                name = "Character/PlayerCharacter/" + name;
                break;
        }

        return Resources.Load<GameObject>(name);
    }

    public static GameObject LoadMapImage(string name)
    {
        name = "MapImage/" + name;
        return Resources.Load<GameObject>(name);
    }

    public static GameObject LoadEffect(string name)
    {
        name = "Effect/" + name;
        return Resources.Load<GameObject>(name);
    }
    public static GameObject LoadTrigger(string name)
    {
        name = "Trigger/" + name;
        return Resources.Load<GameObject>(name);
    }

    public static AudioClip LoadSound(string name, ESoundType SoundType)
    {
        switch(SoundType)
        {
            case ESoundType.BGM:
                name = "Sound/BGM/" + name;
                break;
            case ESoundType.Item:
                name = "Sound/SFX/Item/" + name;
                break;
            case ESoundType.Motion:
                name = "Sound/SFX/Motion/" + name;
                break;
            case ESoundType.UI:
                name = "Sound/SFX/UI/" + name;
                break;
        }
        return Resources.Load<AudioClip>(name);
    }

    public static Sprite[] LoadSprite(string name)
    {
        return Resources.LoadAll<Sprite>(name);
    }

    public static GameObject LoadUI(string name)
    {
        return Resources.Load<GameObject>(name);
    }
   
}
