using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMapObjectType : byte
{
    PlayerStart = 0,
    DownStairs,
    UpStairs,
    SpawnZone,
    PlayerSpawnZone,
    Trap,
    BreakBlock,
    TreasureChest,
    BlockingObject,
    BGMTrigger,
}

public enum EControllerType
{
    Player1 = 0,
    Player2,
    Melee,
    Ranged,
    RunAway,
}

public enum ECharacterType
{
    Player = 0,
    Enemy,
}

public enum ETeamType
{
    A = 0,
    B,
}

public enum ESpawnCharType
{
    SkeletonNoWeapon = 0,
    SkeletonOneHand,
    SkeletonArrow,
    SkeletonWizard,
}

public enum EState
{
    Idle = 0,
    Move,
    Combo1,
    Combo2,
    Combo3,
    Skill,
    Defence,
    Dodge,
    Hit,
    Die,
    ParringAttack,
    Sturn,
}

public enum EEventFrame
{
    None = 0,
    Damage,
    Effect,
    Camera,
    Projectile,
    ComboEnable,
    ComboDisable,
    ComboEnd,
    ParringEnable,
    ParringDisable,
    ParringEnd,
    SuperArmorEnable,
    SuperArmorDisable,
    SpecialPointEnable,
    SpecialPointDisable,
}

public enum EHitAniType
{
    Type1 = 0,
    Type2,
    Type3,
}
