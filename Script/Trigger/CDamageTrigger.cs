using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDamageTrigger : MonoBehaviour
{
    void Start()
    {
       
    }

    void Destroy()
    {
        Owner.gameObject.GetComponent<CBaseCharacter>().HitedTarget(bHitedTarget);
        Destroy(gameObject);
    }

    public void ApplyAliveTime()
    {
        Invoke("Destroy", AliveTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string OtherTag = other.gameObject.tag;
        if (OtherTag == "PlayerCharacter" || OtherTag == "EnemyCharacter")
        {
            if (Owner.tag != OtherTag)
            {
                CBaseCharacter DamagedCharacter = other.gameObject.GetComponent<CBaseCharacter>();
                bHitedTarget = DamagedCharacter.Hit(Damage, Owner, HitMove, bSturn, HitAniType);
            }
        }
    }

    public GameObject Owner { get; set; }
    public int Damage;
    public float HitMove = 0.0f;
    public float AliveTime;
    public bool bSturn = false;
    public EHitAniType HitAniType = EHitAniType.Type1;
    bool bHitedTarget = false;

}
