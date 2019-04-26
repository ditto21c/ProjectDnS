using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSpawnZone : CMapObject
{

	// Use this for initialization
	void Start () {
        Type = (byte)EMapObjectType.SpawnZone;

        RemainTime = SpawnTime;
        CurSpawnCount = MaxSpawnCount;

    }
	
	// Update is called once per frame
	void Update () {
        if (CMapGenerator.bTool) return;

        if (!bSpawnStart) return;

        if (CurSpawnCount == 0)
        {
            if(false == bLoopSpawn)
            {
                bSpawnStart = false;
            }
            else if(bLoopSpawn && 0 == SpawnedObjects.Count)
            {
                RemainTime = SpawnTime;
                CurSpawnCount = MaxSpawnCount;
            }
            return;
        }

        if(RemainTime <= 0)
        {
            GameObject spawnObj = Instantiate(CResourceMgr.LoadCharacter(GetLoadCharacterName(), ECharacterType.Enemy)/*, gameObject.transform*/);
            spawnObj.transform.position = gameObject.transform.position;
            SpawnedObjects.Add(spawnObj);
            RemainTime = SpawnTime;
            --CurSpawnCount;
        }
        else
        {
            RemainTime -= Time.fixedDeltaTime;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string Tag = collision.gameObject.tag;
        if(Tag == "PlayerCharacter")
        {
            bSpawnStart = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        string Tag = collision.gameObject.tag;
        if (Tag == "PlayerCharacter")
        {
            bSpawnStart = false;
        }
    }

    string GetLoadCharacterName()
    {
        string name = string.Empty;
        switch (CharType)
        {
            case ESpawnCharType.SkeletonNoWeapon:
                name = "Skeleton00";
                break;
            case ESpawnCharType.SkeletonOneHand:
                name = "Skeleton01";
                break;
            case ESpawnCharType.SkeletonArrow:
                name = "Skeleton02";
                break;
            case ESpawnCharType.SkeletonWizard:
                name = "EnemyWizard";
                break;
        }
        return name;
    }

    public override void Write(ref BinaryWriter binaryWriter)
    {
        base.Write(ref binaryWriter);

        binaryWriter.Write((int)CharType);
        binaryWriter.Write(SpawnTime);
        binaryWriter.Write(MaxSpawnCount);
        binaryWriter.Write(bLoopSpawn);

        float Radius = GetComponent<CircleCollider2D>().radius;
        binaryWriter.Write(Radius);
    }
    public override void Read(ref BinaryReader binaryReader)
    {
        base.Read(ref binaryReader);

        CharType = (ESpawnCharType)binaryReader.ReadInt32();
        SpawnTime = binaryReader.ReadSingle();
        MaxSpawnCount = binaryReader.ReadInt32();
        bLoopSpawn = binaryReader.ReadBoolean();

        float Radius = binaryReader.ReadSingle();
        GetComponent<CircleCollider2D>().radius = Radius;
    }

    public ESpawnCharType CharType;
    public bool bLoopSpawn;
    public float SpawnTime = 3.0f;
    public int MaxSpawnCount = 1;
    int CurSpawnCount = 1;
    ArrayList SpawnedObjects = new ArrayList();
    float RemainTime;
    bool bSpawnStart = false;
}
