using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTrail : CBaseEffect
{
    void Start()
    {
        Invoke("Destroy", AliveTime);
        eulerAngles = gameObject.transform.rotation.eulerAngles;
    }

    private void Update()
    {
        eulerAngles.z -= Time.deltaTime * 1000.0f;
        Quaternion newRotation = Quaternion.Euler(eulerAngles);
        gameObject.transform.SetPositionAndRotation(gameObject.transform.position, newRotation);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string OtherTag = other.gameObject.tag;
        if (OtherTag == "PlayerCharacter" || OtherTag == "EnemyCharacter")
        {
            if (Owner.tag != OtherTag)
            {
                CBaseCharacter damaged_character = other.gameObject.GetComponent<CBaseCharacter>();
                damaged_character.Hit(Damage, Owner, HitMove, false, HitAniType);
            }
        }
    }

    public float AliveTime;
    Vector3 eulerAngles = new Vector3();
    public EHitAniType HitAniType = EHitAniType.Type1;
    
}
