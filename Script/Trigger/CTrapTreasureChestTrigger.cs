using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTrapTreasureChestTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string OtherTag = other.gameObject.tag;
        if (OtherTag == "PlayerCharacter" || OtherTag == "EnemyCharacter")
        {
            if (Owner.tag != OtherTag)
            {
                CBaseCharacter DamagedCharacter = other.gameObject.GetComponent<CBaseCharacter>();
                DamagedCharacter.Hit(Damage, Owner, HitMove, false, EHitAniType.Type1);
            }
        }
    }

    public GameObject Owner;
    public int Damage;
    public float HitMove = 0.0f;
    
}
