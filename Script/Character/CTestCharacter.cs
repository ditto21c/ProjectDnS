using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTestCharacter : CBaseCharacter {

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (curState == EState.Die)
            return;

        CBaseCharacter character = collision.gameObject.GetComponent<CBaseCharacter>();
        string tag = collision.gameObject.tag;
        bool bCharacter = tag == "EnemyCharacter" || tag == "PlayerCharacter" ? true : false;
        if (bCharacter && character.curState != EState.Die)
        {
            character.Idle();
        }

        // 충돌한 위치에 가장 가까운 정수 위치를 구한다.
        Vector3 cur_pos = gameObject.transform.position;
        cur_pos.x = Mathf.Round(cur_pos.x);
        cur_pos.y = Mathf.Round(cur_pos.y);
        gameObject.transform.position = cur_pos;

        paths.Clear();
        isMoving = false;
    }

}
