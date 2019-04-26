using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBlockingObject : CMapObject
{
    // Start is called before the first frame update
    void Start()
    {
        Type = (byte)EMapObjectType.BlockingObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string Tag = collision.gameObject.tag;
        if (Tag == "PlayerCharacter" || Tag == "EnemyCharacter")
        {
            CBaseCharacter Character = collision.gameObject.GetComponent<CBaseCharacter>();
            Character.Stop();
            collision.gameObject.transform.position = Character.PrePos;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        string Tag = collision.gameObject.tag;
        if (Tag == "PlayerCharacter" || Tag == "EnemyCharacter")
        {
            CBaseCharacter Character = collision.gameObject.GetComponent<CBaseCharacter>();
            Character.Stop();
            collision.gameObject.transform.position = Character.PrePos;
        }
    }
}
