using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CProjectile : CBaseEffect
{
    protected void Start ()
    {
        DirVec = EndPos - gameObject.transform.position;
        DirVec.Normalize();
    }

    protected void Update ()
    {
        if (Vector3.Distance(gameObject.transform.position, EndPos) <= 0.1f)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.transform.Translate(DirVec * Time.deltaTime * Speed, Space.World);
        }
    }
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if("BlockImage" == other.gameObject.tag)
        {
            Destroy(gameObject);
        }
    }

    public Vector3 EndPos { get; set; }
    public float Speed;
    public Vector3 DirVec;
    public EHitAniType HitAniType = EHitAniType.Type1;
}
