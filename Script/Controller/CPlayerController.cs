using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerController : CBaseController {

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (CurCamera == null)
        //        return;

        //    Ray ray = CurCamera.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit ray_cast_hit;
        //    if (Physics.Raycast(ray, out ray_cast_hit))
        //    {
        //        if ("TileImage" == ray_cast_hit.collider.tag)
        //        {
        //            Vector3 player_position = gameObject.transform.position;
        //            Vector3 end_position = ray_cast_hit.collider.transform.position;
        //            Character.Move(player_position, end_position);
        //        }
        //    }
        //}

        bool[] bKeyDown = {
            false,
            false,
            false,
            false,
        };
        Vector3 startPos = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            startPos.y += 1.0f;
            bKeyDown[0] = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            startPos.y -= 1.0f;
            bKeyDown[1] = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            startPos.x -= 1.0f;
            bKeyDown[2] = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            startPos.x += 1.0f;
            bKeyDown[3] = true;
        }
        if (startPos.x != 0 || startPos.y != 0)
        {
            if (Character.curState == EState.Idle)
            {
                Vector3 EndPos = gameObject.transform.position + startPos;
                Character.Move(EndPos);
                CachedMovePos = startPos;
            }
        }

        bool bAllKeyDown = true;
        for (int i = 0; i < 4; ++i)
        {
            if (bKeyDown[i])
            {
                bAllKeyDown = false;
                break;
            }
        }
        bool bKeyUp = Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D);
        if (bKeyUp && bAllKeyDown)
        {
            Character.Stop();
        }

        if (Character.curState == EState.Die)
            return;

        if (Input.GetKey(KeyCode.J))
        {
            Character.Attack1();
        }
        if (Input.GetKey(KeyCode.K))
        {
            Character.Attack2();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Character.Skill();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Character.Dodge(CachedMovePos);
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            //Texture2D tex2d = Resources.Load<Texture2D>("IT");
            //Rect rect= new Rect(32, 288, 32, 32);
            //Vector2 Pivot = new Vector2(0.5f, 0.5f);
            //Sprite cs = Sprite.Create(tex2d, rect, Pivot);
            CCombo1Ani ani = Character.GetComponentInChildren<CCombo1Ani>();
            Sprite[] FindSprites = CResourceMgr.LoadSprite("IT");
            Sprite FindSprite = FindSprites[184];

            ani.Sprites[0] = FindSprite;
            ani.Sprites[1] = FindSprite;
            ani.Sprites[2] = FindSprite;
            ani.Sprites[3] = FindSprite;

        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Character.Attack1();
        //}
        //if (Input.GetMouseButtonDown(1))
        //{
        //    Character.Attack2();
        //}
    }

    public void SetCameara(Camera InCamera)
    {
        CurCamera = InCamera;
    }

   
    Vector3 CachedMovePos;
    Camera CurCamera;
}
