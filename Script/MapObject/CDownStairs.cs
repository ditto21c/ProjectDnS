using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDownStairs : CBaseStairs {

	// Use this for initialization
	new void Start () {
        base.Start();
        Type = (byte)EMapObjectType.DownStairs;
    }
	
	// Update is called once per frame
	void Update () {
        if (CMapGenerator.bTool)
        {
            float NearestDist = 9999.0f;
            GameObject FindedNearestObj = null;
            GameObject[] FindObjects = GameObject.FindGameObjectsWithTag("UpStairs");
            foreach (GameObject FindObject in FindObjects)
            {
                float Dist = Vector3.Distance(gameObject.transform.position, FindObject.transform.position);
                if(Dist < NearestDist)
                {
                    FindedNearestObj = FindObject;
                    NearestDist = Dist;
                }
            }
            if(FindedNearestObj)
            {
                CUpStairs stairs = FindedNearestObj.GetComponent<CUpStairs>();
                LinkIndex = stairs.Index;

                if (stairs.LinkIndex == Index)
                    Debug.DrawLine(gameObject.transform.position, FindedNearestObj.transform.position);
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "PlayerCharacter" || tag == "EnemyCharacter")
        {
            CDebugLog.Log(ELogType.MapObject, "OnTriggerEnter2D DownStairs");

            CBaseCharacter character = collision.gameObject.GetComponent<CBaseCharacter>();
            if (character.curState == EState.Dodge)
                return;
            if (character.IsTeleport())
            {
                character.Teleport(false);
                CDebugLog.Log(ELogType.MapObject, "OnTriggerEnter2D DownStairs return");
                return;
            }

            
            GameObject[] find_objects = GameObject.FindGameObjectsWithTag("UpStairs");
            foreach(GameObject find_object in find_objects)
            {
                CUpStairs stairs = find_object.GetComponent<CUpStairs>();
                if(stairs.Index == LinkIndex)
                {
                    Vector3 NewPos = find_object.transform.position;
                    NewPos.z = 0.0f;
                    collision.gameObject.transform.position = NewPos;
                    character.Teleport(true);

                    CDebugLog.Log(ELogType.MapObject, "OnTriggerEnter2D DownStairs teleport");
                    break;
                }
            }
        }
    }

  
}
